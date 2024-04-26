using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class FreeClimbMC : MonoBehaviour
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

        public FreeClimbAnimHookMC a_hook;

        Transform helper;
        float delta;

        [SerializeField] InputReader _inputReader;

        private void OnEnable()
        {
            _inputReader.MoveEvent += UpdateMove;
        }

        private void OnDisable()
        {
            _inputReader.MoveEvent += UpdateMove;
        }

        void UpdateMove(Vector2 move)
        {
            horizontal = move.x;
            vertical = move.y;
        }


        void Start()
        {
            Init();
        }

        public void Init()
        {
            helper = new GameObject().transform;
            helper.name = "climb helper";
            a_hook.Init(this, helper);
            CheckForClimb();
        }

        public void CheckForClimb()
        {
            Vector3 origin = transform.position;
            origin.y += 1.4f;
            Vector3 dir = transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, 5))
            {
                helper.position = PosWithOffset(origin, hit.point);
                InitForClimb(hit);
            }
        }

        void InitForClimb(RaycastHit hit)
        {
            isClimbing = true;

            helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
            startPos = transform.position;
            targetPos = hit.point + (hit.normal * offsetFromWall);
            t = 0;
            inPosition = false;
            anim.CrossFade("climb_idle", 2);
        }

        void Update()
        {
            delta = Time.deltaTime;
            Tick(delta);
        }

        public void Tick(float delta)
        {
            if (!inPosition)
            {
                GetInPosition();
                return;
            }

            if (!isLerping)
            {
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
                if (t > 1)
                {
                    t = 1;
                    isLerping = false;
                }

                Vector3 cp = Vector3.Lerp(startPos, targetPos, t);
                transform.position = cp;
                transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
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
            if (Physics.Raycast(origin, dir, out hit, dis))
            {
                Debug.Log("CORNER");
                //Check if it's a corner
                return false;
            }

            origin += moveDir * dis;
            dir = helper.forward;
            float dis2 = rayForwardTowardsWall;

            //Raycast forward towards the wall
            DebugLine.singleton.SetLine(origin, origin + (dir * dis2), 1);
            if (Physics.Raycast(origin, dir, out hit, dis2))
            {
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                Debug.Log("Towards Wall" + " " + -hit.normal + " " + helper.position);
                return true;
            }

            Debug.Log("AFTER TOWARDS WALL");
            origin = origin + (dir * dis2);
            dir = -moveDir;
            DebugLine.singleton.SetLine(origin, origin + dir, 1);
            if (Physics.Raycast(origin, dir, out hit, rayForwardTowardsWall))
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
            if (Physics.Raycast(origin, dir, out hit, dis2))
            {
                float angle = Vector3.Angle(-helper.forward, hit.normal);
                if (angle < 40)
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
            t += delta;

            if (t > 1)
            {
                t = 1;
                inPosition = true;

                a_hook.CreatePositions(targetPos, Vector3.zero, false);
            }

            Vector3 tp = Vector3.Lerp(startPos, targetPos, t);
            transform.position = tp;
            transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
        }

        Vector3 PosWithOffset(Vector3 origin, Vector3 target)
        {
            // origin -= OffsetPosition;
            Vector3 direction = origin - target;
            direction.Normalize();
            Vector3 offset = direction * offsetFromWall;
            return target + offset;
        }

    }
}
