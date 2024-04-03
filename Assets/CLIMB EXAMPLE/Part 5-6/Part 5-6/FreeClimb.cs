using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class FreeClimb : MonoBehaviour
    {
        public Animator anim;
        public bool isClimbing;

        bool inPosition;
        bool isLerping;
        float t;
        Vector3 startPos;
        Vector3 targetPos;
        Quaternion startRot;
        Quaternion targetRot;
        public float possitionOffset;
        public float offsetFromWall = 0.3f;
        public float speed_multiplier = 0.2f;
        public float climbSpeed = 3;
        public float rotateSpeed = 5;

        public float rayTowardsMoveDir = 0.5f;
        public float rayForwardTowardsWall = 1;

        public float horizontal;
        public float vertical;
        public bool isMid;

        public IKSnapshot baseIKsnapshot;

        public FreeClimbAnimHook a_hook;
        ThirdPersonController tpc;

        Transform helper;
        float delta;

        LayerMask ignoreLayers;

        void Start()
        {
            tpc = GetComponent<ThirdPersonController>();
            Init();
        }

        public void Init()
        {
            helper = new GameObject().transform;
            helper.name = "climb helper";
            a_hook.Init(this, helper);
            ignoreLayers = ~(1 << 8);
         //   CheckForClimb();
        }

        public bool CheckForClimb()
        {
            Vector3 origin = transform.position;
            origin.y += 0.02f;
            Vector3 dir = transform.forward;
            RaycastHit hit;
            if(Physics.Raycast(origin,dir, out hit, 0.5f, ignoreLayers))
            {
                helper.position = PosWithOffset(origin, hit.point);
                InitForClimb(hit);
                
                return true;
            }

            return false;
        }

        void InitForClimb(RaycastHit hit)
        {
            isClimbing = true;
            a_hook.enabled = true;    
            helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
            startPos = transform.position;
            targetPos = hit.point + (hit.normal * offsetFromWall);
            t = 0;
            inPosition = false;
            anim.CrossFade("climb_idle", 2);
        }

        public void Tick(float d_time)
        {
            this.delta = d_time;
            if(!inPosition)
            {
                GetInPosition();
                return;
            }

            if(!isLerping)
            {
                bool cancel = Input.GetKeyUp(KeyCode.X);
                if(cancel)
                {
                    CancelClimb();
                    return;
                }

                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
                float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

                Vector3 h = helper.right * horizontal;
                Vector3 v = helper.up * vertical;
                Vector3 moveDir = (h + v).normalized;

                if (isMid)
                {
                    if (moveDir == Vector3.zero)
                        return;
                }
                else
                {
                    bool canMove = CanMove(moveDir);
                    if (!canMove || moveDir == Vector3.zero)
                        return;
                }

                isMid = !isMid;

                t = 0;
                isLerping = true;
                startPos = transform.position;
                Vector3 tp = helper.position - transform.position;
                float d = Vector3.Distance(helper.position, startPos) / 2;
                tp *= possitionOffset;
                tp += transform.position;
                targetPos = (isMid) ? tp : helper.position;

                a_hook.CreatePositions(targetPos, moveDir, isMid);

            }
            else
            {
                t += delta * climbSpeed;
                if(t > 1)
                {
                    t = 1;
                    isLerping = false;
                }

                Vector3 cp = Vector3.Lerp(startPos, targetPos, t);
                transform.position = cp;
                transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);

                LookForGround();
            }
        }

        bool CanMove(Vector3 moveDir)
        {
            Vector3 origin = transform.position;
            float dis = rayTowardsMoveDir;
            Vector3 dir = moveDir;

            DebugLine.singleton.SetLine(origin, origin + (dir * dis), 0);

            //Raycast towards the direction you want to move
            RaycastHit hit;
            if(Physics.Raycast(origin,dir, out hit ,dis))
            {
                //Check if it's a corner
                return false;
            }

            origin += moveDir * dis;
            dir = helper.forward;
            float dis2 = rayForwardTowardsWall;

            //Raycast forward towards the wall
            DebugLine.singleton.SetLine(origin, origin + (dir * dis2), 1);
            if(Physics.Raycast(origin,dir, out hit, dis2))
            {
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }

            origin = origin + (dir * dis2);
            dir = -moveDir;
            DebugLine.singleton.SetLine(origin, origin + dir, 1);
            if (Physics.Raycast(origin,dir,out hit, rayForwardTowardsWall))
            {
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }

           // return false;

            origin += dir * dis2;
            dir = -Vector3.up;

            DebugLine.singleton.SetLine(origin, origin + dir, 2);
          //  Debug.DrawRay(origin, dir, Color.yellow);
            if(Physics.Raycast(origin,dir, out hit, dis2))
            {
                float angle = Vector3.Angle(-helper.forward, hit.normal);
                if(angle < 40)
                {
                    helper.position = PosWithOffset(origin, hit.point);
                    helper.rotation = Quaternion.LookRotation(-hit.normal);
                    return true;
                }
            } 

            return false;
        }

        void GetInPosition()
        {
            t += delta * 3;

            if(t > 1)
            {
                t = 1;
                inPosition = true;
                horizontal = 0;
                vertical = 0;
                a_hook.CreatePositions(targetPos, Vector3.zero, false);
            }

            Vector3 tp = Vector3.Lerp(startPos, targetPos, t);
            transform.position = tp;
            transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
        }

        Vector3 PosWithOffset(Vector3 origin, Vector3 target)
        {
            Vector3 direction = origin - target;
            direction.Normalize();
            Vector3 offset = direction * offsetFromWall;
            return target + offset;
        }

        void LookForGround()
        {
            Vector3 origin = transform.position;
            Vector3 direction = -transform.up;
            RaycastHit hit;
            if(Physics.Raycast(origin,direction,out hit, rayTowardsMoveDir + 0.05f, ignoreLayers))
            {
                CancelClimb();
            }
        }

        void CancelClimb()
        {
            isClimbing = false;
            tpc.EnableController();
            a_hook.enabled = false;
        }
     
    }


    [System.Serializable]
    public class IKSnapshot
    {
        public Vector3 rh, lh, lf, rf;
    }
}
