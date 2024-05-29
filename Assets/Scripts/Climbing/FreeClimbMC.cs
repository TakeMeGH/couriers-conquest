using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

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
        [SerializeField] LayerMask ignoreLayers;
        LayerMask UsedLayer;
        [SerializeField] InputReader _inputReader;
        public Rig Rig;
        public UnityAction OnAboveStandAble;
        public float UpOffsetMultiplier = 1f;
        public UnityAction OnMCMove;
        Vector3 _lastMoveDir;
        Vector2 rawMoveDir;
        Vector2 lastRawMoveDir;

        float epsilon = 1e-5f;

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
            rawMoveDir = move;
        }


        void Start()
        {
            Init();
            UsedLayer = ~ignoreLayers;
        }

        public void Init()
        {
            helper = new GameObject().transform;
            helper.name = "climb helper";
            a_hook.Init(this, helper);
        }

        public bool CheckForClimb()
        {
            Vector3 origin = transform.position;
            origin.y += 0.02f;
            Vector3 dir = transform.forward;
            RaycastHit hit;

            if (DebugLine.singleton != null) DebugLine.singleton.SetLine(origin, origin + dir * 1f, 0);
            if (DebugLine.singleton != null) DebugLine.singleton.SetLine(origin + Vector3.up, origin + Vector3.up + dir * 0.5f, 1);

            if (Physics.Raycast(origin + Vector3.up, dir, 1.5f, UsedLayer))
            {
                if (Physics.Raycast(origin, dir, out hit, 1f, UsedLayer))
                {
                    helper.position = PosWithOffset(origin, hit.point);
                    a_hook.Init(this, helper);
                    InitForClimb(hit);
                    Rig.weight = 1;
                    return true;
                }
                return false;
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
            anim.CrossFade("Climb Idle", 2);
        }
        public void Tick(float d_time)
        {
            this.delta = d_time;
            if (!inPosition)
            {
                GetInPosition();
                return;
            }

            if (!isLerping)
            {
                Vector3 h = helper.right * horizontal;
                Vector3 v = helper.up * vertical;
                Vector3 moveDir = (h + v).normalized;
                _lastMoveDir = moveDir;
                lastRawMoveDir = rawMoveDir;

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
                targetPos = isMid ? tp : helper.position;
                OnMCMove.Invoke();
                a_hook.CreatePositions(targetPos, moveDir, rawMoveDir, isMid);

            }
            else
            {
                t += delta * climbSpeed;
                if (t > 1)
                {
                    t = 1;
                    isLerping = false;
                }
                CheckUp(lastRawMoveDir);
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

            if (DebugLine.singleton != null) DebugLine.singleton.SetLine(origin, origin + (dir * dis), 0);

            // Raycast towards the direction you want to move
            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, dis, UsedLayer))
            {
                Debug.Log("CORNER " + hit.transform.gameObject.name);
                // Check if it's a corner
                return false;
            }

            origin += moveDir * dis;
            dir = helper.forward;
            float dis2 = rayForwardTowardsWall;

            // Raycast forward towards the wall
            if (DebugLine.singleton != null) DebugLine.singleton.SetLine(origin, origin + (dir * dis2), 1);

            if (!CheckUp(rawMoveDir))
            {
                return false;
            }

            if (Physics.Raycast(origin, dir, out hit, dis2, UsedLayer))
            {
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                // Debug.Log("Towards Wall" + " " + -hit.normal + " " + helper.position);
                return true;
            }

            origin = origin + (dir * dis2);
            dir = -moveDir;
            if (DebugLine.singleton != null) DebugLine.singleton.SetLine(origin, origin + dir, 1);
            if (Physics.Raycast(origin, dir, out hit, rayForwardTowardsWall, UsedLayer))
            {
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }

            origin += dir * dis2;
            dir = -Vector3.up;

            if (DebugLine.singleton != null) DebugLine.singleton.SetLine(origin, origin + dir, 2);
            if (Physics.Raycast(origin, dir, out hit, dis2, UsedLayer))
            {
                float angle = Vector3.Angle(-helper.forward, hit.normal);
                Debug.Log("ANGLE " + angle);
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
            t += delta * 10;

            if (t > 1)
            {
                t = 1;
                inPosition = true;
                horizontal = 0;
                vertical = 0;
                a_hook.CreatePositions(targetPos, Vector3.zero, Vector3.zero, false);
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

        bool CheckUp(Vector3 moveDir)
        {
            // Kalau turun tidak dipedulikan
            if (moveDir.y <= -epsilon) return true;
            if(moveDir.x > epsilon || moveDir.x < -epsilon) return true;
            Vector3 origin = transform.position;
            float dis = rayTowardsMoveDir;

            origin += moveDir * dis;
            Vector3 dir = helper.forward;
            float dis2 = rayForwardTowardsWall;

            RaycastHit hit;

            if (DebugLine.singleton != null) DebugLine.singleton.SetLine(origin + moveDir * UpOffsetMultiplier, origin + moveDir * UpOffsetMultiplier + (dir * dis2), 2);
            if (DebugLine.singleton != null) DebugLine.singleton.SetLine(origin + moveDir * UpOffsetMultiplier + (dir * dis2), origin + moveDir * UpOffsetMultiplier + (dir * dis2) + -helper.up * dis2, 3);

            if (!Physics.Raycast(origin + moveDir * UpOffsetMultiplier, dir, out hit, dis2, UsedLayer))
            {
                if (Physics.Raycast(origin + moveDir * UpOffsetMultiplier + (dir * dis2), -helper.up, out hit, dis2, UsedLayer))
                {
                    OnAboveStandAble.Invoke();
                    return false;
                }
            }
            return true;
        }

    }
}
