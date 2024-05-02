using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class FreeClimbAnimHookMC : MonoBehaviour
    {
        Animator anim;
        public Transform MCClimbOrigin;

        [SerializeField] List<Transform> _avatarIk = new List<Transform>();
        IKSnapshot ikBase;
        IKSnapshot current = new IKSnapshot();
        IKSnapshot next = new IKSnapshot();
        IKGoals goals = new IKGoals();

        public float w_rh;
        public float w_lh;
        public float w_lf;
        public float w_rf;

        Vector3 rh, lh, rf, lf;
        Transform h;
        bool isMirror;
        bool isLeft;
        Vector3 prevMovDir;

        float delta;
        public float LerpSpeed = 1;
        public void Init(FreeClimbMC c, Transform helper)
        {
            anim = c.anim;
            ikBase = c.baseIKsnapshot;
            h = helper;

            IKSnapshot ik = CreateSnapshot(h.position);
            CopySnapshot(ref current, ik);

            SetIKPosition(true, true, current.lf, AvatarIKMC.LeftFoot);
            SetIKPosition(true, true, current.rf, AvatarIKMC.RightFoot);
            SetIKPosition(true, true, current.lh, AvatarIKMC.LeftHand);
            SetIKPosition(true, true, current.rh, AvatarIKMC.RightHand);

            SetIKPos(AvatarIKMC.LeftHand, lh, w_lh, 1000);
            SetIKPos(AvatarIKMC.RightHand, rh, w_rh, 1000);
            SetIKPos(AvatarIKMC.LeftFoot, lf, w_lf, 1000);
            SetIKPos(AvatarIKMC.RightFoot, rf, w_rf, 1000);
            Debug.Log("INIT");

        }

        public void CreatePositions(Vector3 origin, Vector3 moveDir, bool isMid)
        {
            delta = Time.deltaTime;
            HandleAnim(moveDir, isMid);

            if (!isMid)
            {
                UpdateGoals(moveDir);
                prevMovDir = moveDir;
            }
            else
            {
                UpdateGoals(prevMovDir);
            }

            IKSnapshot ik = CreateSnapshot(origin);
            CopySnapshot(ref current, ik);
            Debug.Log(current.lf + " " + current.rf + " " + current.lh + " " + current.rh);

            SetIKPosition(isMid, goals.lf, current.lf, AvatarIKMC.LeftFoot);
            SetIKPosition(isMid, goals.rf, current.rf, AvatarIKMC.RightFoot);
            SetIKPosition(isMid, goals.lh, current.lh, AvatarIKMC.LeftHand);
            SetIKPosition(isMid, goals.rh, current.rh, AvatarIKMC.RightHand);

            UpdateIkWeight(AvatarIKMC.LeftFoot, 1);
            UpdateIkWeight(AvatarIKMC.RightFoot, 1);
            UpdateIkWeight(AvatarIKMC.LeftHand, 1);
            UpdateIkWeight(AvatarIKMC.RightHand, 1);
        }

        void UpdateGoals(Vector3 moveDir)
        {
            isLeft = moveDir.x <= 0;
            // isMirror = false;

            if (moveDir.x != 0)
            {
                goals.lh = isLeft;
                goals.rh = !isLeft;
                goals.lf = !isLeft;
                goals.rf = isLeft;
            }
            else
            {
                bool isEnabled = isMirror;
                if (moveDir.y < 0)
                {
                    isEnabled = !isEnabled;
                }

                goals.lh = isEnabled;
                goals.rh = !isEnabled;
                goals.lf = !isEnabled;
                goals.rf = isEnabled;
            }
        }

        void HandleAnim(Vector3 moveDir, bool isMid)
        {
            if (isMid)
            {
                if (moveDir.y != 0)
                {
                    if (moveDir.x == 0)
                    {
                        isMirror = !isMirror;
                        // anim.SetBool("mirror", isMirror);
                    }
                    else
                    {
                        if (moveDir.y < 0)
                        {
                            isMirror = (moveDir.x > 0);
                            // anim.SetBool("mirror", isMirror);
                        }
                        else
                        {
                            isMirror = (moveDir.x < 0);
                            // anim.SetBool("mirror", isMirror);
                        }
                    }
                    if (isMirror)
                    {
                        anim.CrossFade("Climb Left", 0.2f);

                    }
                    else
                    {
                        anim.CrossFade("Climb Right", 0.2f);
                    }
                    // anim.CrossFade("climb_up", 0.2f);
                }
            }
            else
            {
                anim.CrossFade("Climb Idle", 0.2f);
            }
        }

        public IKSnapshot CreateSnapshot(Vector3 o)
        {
            IKSnapshot r = new IKSnapshot();
            Vector3 _lh = LocalToWorld(ikBase.lh);
            r.lh = GetPosActual(_lh, AvatarIKMC.LeftHand);
            Vector3 _rh = LocalToWorld(ikBase.rh);
            r.rh = GetPosActual(_rh, AvatarIKMC.RightHand);
            Vector3 _lf = LocalToWorld(ikBase.lf);
            r.lf = GetPosActual(_lf, AvatarIKMC.LeftFoot);
            Vector3 _rf = LocalToWorld(ikBase.rf);
            r.rf = GetPosActual(_rf, AvatarIKMC.RightFoot);
            return r;
        }

        public float wallOffset = 0;

        Vector3 GetPosActual(Vector3 o, AvatarIKMC goal)
        {
            Vector3 r = o;
            Vector3 origin = o;
            Vector3 dir = h.forward;
            origin += -(dir * 0.2f);
            RaycastHit hit;

            bool isHit = false;
            if (Physics.Raycast(origin, dir, out hit, 1.5f))
            {
                Vector3 _r = hit.point + (hit.normal * wallOffset);
                r = _r;
                isHit = true;

                if (goal == AvatarIKMC.LeftFoot || goal == AvatarIKMC.RightFoot)
                {
                    if (hit.point.y > transform.position.y - 0.1f)
                    {
                        isHit = false;
                    }
                }
            }

            if (!isHit)
            {
                switch (goal)
                {
                    case AvatarIKMC.LeftFoot:
                        r = LocalToWorld(ikBase.lf);
                        break;
                    case AvatarIKMC.RightFoot:
                        r = LocalToWorld(ikBase.rf);
                        break;
                    case AvatarIKMC.LeftHand:
                        r = LocalToWorld(ikBase.lh);
                        break;
                    case AvatarIKMC.RightHand:
                        r = LocalToWorld(ikBase.rh);
                        break;
                    default:
                        break;
                }
            }

            return r;
        }

        Vector3 LocalToWorld(Vector3 p)
        {
            Vector3 r = h.position;
            r += h.right * p.x;
            r += h.forward * p.z;
            r += h.up * p.y;
            return r;
        }

        public void CopySnapshot(ref IKSnapshot to, IKSnapshot from)
        {
            to.rh = from.rh;
            to.lh = from.lh;
            to.lf = from.lf;
            to.rf = from.rf;
        }

        void SetIKPosition(bool isMid, bool isTrue, Vector3 pos, AvatarIKMC goal)
        {
            if (isMid)
            {
                Vector3 p = GetPosActual(pos, goal);

                if (isTrue)
                {
                    UpdateIKPosition(goal, p);
                }
                else
                {
                    if (goal == AvatarIKMC.LeftFoot || goal == AvatarIKMC.RightFoot)
                    {
                        if (p.y > transform.position.y - 0.25f)
                        {
                            //       Debug.Log("higher");
                            //       UpdateIKPosition(goal, p);
                        }
                    }
                }
            }
            else
            {
                if (!isTrue)
                {
                    Vector3 p = GetPosActual(pos, goal);
                    UpdateIKPosition(goal, p);
                }
            }
        }

        public void UpdateIKPosition(AvatarIKMC goal, Vector3 pos)
        {
            switch (goal)
            {
                case AvatarIKMC.LeftFoot:
                    lf = pos;
                    break;
                case AvatarIKMC.RightFoot:
                    rf = pos;
                    break;
                case AvatarIKMC.LeftHand:
                    lh = pos;
                    break;
                case AvatarIKMC.RightHand:
                    rh = pos;
                    break;
                default:
                    break;
            }
        }

        public void UpdateIkWeight(AvatarIKMC goal, float w)
        {
            switch (goal)
            {
                case AvatarIKMC.LeftFoot:
                    w_lf = w;
                    break;
                case AvatarIKMC.RightFoot:
                    w_rf = w;
                    break;
                case AvatarIKMC.LeftHand:
                    w_lh = w;
                    break;
                case AvatarIKMC.RightHand:
                    w_rh = w;
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            OnAnimatorIK();

        }

        void OnAnimatorIK()
        {
            delta = Time.deltaTime;
            // Debug.Log("ANIMATOR IK");

            SetIKPos(AvatarIKMC.LeftHand, lh, w_lh);
            SetIKPos(AvatarIKMC.RightHand, rh, w_rh);
            SetIKPos(AvatarIKMC.LeftFoot, lf, w_lf);
            SetIKPos(AvatarIKMC.RightFoot, rf, w_rf);
        }

        void SetIKPos(AvatarIKMC goal, Vector3 tp, float w, float lerpSpeed = -1)
        {
            if (lerpSpeed == -1) lerpSpeed = LerpSpeed;
            IKStates ikState = GetIKStates(goal);
            if (ikState == null)
            {
                ikState = new IKStates();
                ikState.goal = goal;
                ikStates.Add(ikState);
            }

            if (w == 0)
            {
                ikState.isSet = false;
            }

            if (!ikState.isSet)
            {
                ikState.position = GoalToBodyBones(goal).position;
                ikState.isSet = true;
            }

            ikState.positionWeight = w;
            ikState.position = Vector3.Lerp(ikState.position, tp, delta * lerpSpeed);

            _avatarIk[(int)goal].position = ikState.position;
            // anim.SetIKPositionWeight(goal, ikState.positionWeight);
            // anim.SetIKPosition(goal, ikState.position);
        }

        Transform GoalToBodyBones(AvatarIKMC goal)
        {
            return _avatarIk[(int)goal].transform;

            // switch (goal)
            // {
            //     case AvatarIKMC.LeftFoot:
            //         return anim.GetBoneTransform(HumanBodyBones.LeftFoot);
            //     case AvatarIKMC.RightFoot:
            //         return anim.GetBoneTransform(HumanBodyBones.RightFoot);
            //     case AvatarIKMC.LeftHand:
            //         return anim.GetBoneTransform(HumanBodyBones.LeftHand);
            //     default:
            //     case AvatarIKMC.RightHand:
            //         return anim.GetBoneTransform(HumanBodyBones.RightHand);
            // }
        }

        IKStates GetIKStates(AvatarIKMC goal)
        {
            IKStates r = null;
            foreach (IKStates i in ikStates)
            {
                if (i.goal == goal)
                {
                    r = i;
                    break;
                }
            }

            return r;
        }

        List<IKStates> ikStates = new List<IKStates>();

        class IKStates
        {
            public AvatarIKMC goal;
            public Vector3 position;
            public float positionWeight;
            public bool isSet = false;
        }

        public enum AvatarIKMC
        {
            LeftHand,
            RightHand,
            LeftFoot,
            RightFoot,

        }
    }
}
