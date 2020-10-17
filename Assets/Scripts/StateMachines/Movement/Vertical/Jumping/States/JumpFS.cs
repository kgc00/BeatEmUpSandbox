using System;
using General;
using Photon.Pun;
using StateMachines.Attacks;
using StateMachines.Interfaces;
using StateMachines.Logger;
using StateMachines.Movement.Models;
using StateMachines.Network;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = System.Diagnostics.Debug;

namespace StateMachines.Movement.Vertical.Jumping.States {
    /// <summary>
    /// States for jump object.  Not inhereting from FSM because
    /// this is a callback based state machine, and FSMState
    /// requires update return an FSMState (which we would ignore
    /// and would be confusing)
    /// </summary>
    public abstract class JumpFS : FSMState<JumpFS>, IHandleLockedJumpInput, IAcceptDashInput, IAcceptRunInput {
        protected readonly Animator Animator;
        protected readonly Rigidbody2D Rig;
        protected readonly GameObject Behaviour;
        protected readonly JumpFSM Jump;
        protected readonly JumpConfig Config;
        protected readonly InputLogger logger;
        protected readonly int Grounded = Animator.StringToHash("Grounded");
        protected readonly int Jumping = Animator.StringToHash("Jumping");
        protected readonly int DoubleJumping = Animator.StringToHash("DoubleJumping");
        protected readonly AttackFSM attackStateMachine;
        public int ViewID { get; private set; }
        public bool PUNIsMine { get; private set; }

        protected JumpFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) {
            Jump = jump;
            Behaviour = behaviour;
            ViewID = behaviour.GetPhotonView().ViewID;
            PUNIsMine = behaviour.GetPhotonView().IsMine;
            Config = jumpConfig;
            Animator = behaviour.GetComponent<Animator>();
            Rig = behaviour.GetComponent<Rigidbody2D>();
            logger = behaviour.GetComponent<InputLogger>();
            attackStateMachine = behaviour.GetComponent<AttackFSM>();
        }


        public bool AnimatorStateJumping() => Animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump");
        public bool AnimatorStateDoubleJumping() => Animator.GetCurrentAnimatorStateInfo(0).IsTag("DoubleJump");
        public bool AnimatorStateFalling() => Animator.GetCurrentAnimatorStateInfo(0).IsTag("Fall");

        public virtual Vector2 Force() => Vector2.zero;

        public virtual void OnCollisionEnter2D_RPC(int? id) {
            if (!id.HasValue) return;
            
            var other = Helpers.GameObjectFromId(id.Value);
            Debug.Assert(other != null, nameof(other) + " != null");
            if (attackStateMachine.State.isAerialState  && other.CompareTag("Board")) attackStateMachine.RaiseChangeStateEvent(AttackStates.Idle);
        }

        public abstract void AcceptJumpInput(InputAction.CallbackContext context);
        public virtual void AcceptDashInput(InputAction.CallbackContext context) { }

        public virtual void AcceptLockJumpInput(object sender) {
            Jump.RaiseChangeStateEvent(JumpStates.Locked);
        }
        public virtual void AcceptUnlockJumpInput(object sender) { }
        protected bool OutOfJumps() => Jump.UnitMovementData.jumpsLeft <= 0;
        protected bool OutOfDashes() => Jump.UnitMovementData.dashesLeft <= 0;



      

        public virtual void AcceptMoveInput(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed)
                Jump.RaiseSetMoveDirEvent(context.ReadValue<Single>(), new Vector3((int) Jump.UnitMovementData.moveDir, 1, 1),
                    ViewID);
            Jump.RaiseSetMoveDirEvent(context.ReadValue<Single>(), Behaviour.transform.localScale, ViewID);
        }

        protected float ProvideCappedHorizontalForce(float velocity, float cap, float dir, float rigX) {
            int CappedMoveVelocity() => 0;
            float NormalMoveVelocity() => dir * velocity;
            bool HitSpeedCap(float xVel) => Mathf.Abs(xVel) >= cap;
            bool IsForwardMovement(float xVel) => Mathf.Sign(dir) == Mathf.Sign(xVel);

            return HitSpeedCap(rigX) && IsForwardMovement(rigX)
                ? CappedMoveVelocity()
                : NormalMoveVelocity();
        }
    }
}