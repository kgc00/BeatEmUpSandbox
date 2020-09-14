using System;
using StateMachines.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
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
        protected readonly int Grounded = Animator.StringToHash("Grounded");
        protected readonly int Jumping = Animator.StringToHash("Jumping");
        protected readonly int DoubleJumping = Animator.StringToHash("DoubleJumping");
        public float MoveDir { get; protected set; }

        protected JumpFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig, float moveDir) {
            Jump = jump;
            Behaviour = behaviour;
            Config = jumpConfig;
            Animator = behaviour.GetComponent<Animator>();
            Rig = behaviour.GetComponent<Rigidbody2D>();
            MoveDir = moveDir;
        }

        public bool AnimatorStateJumping() => Animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump");
        public bool AnimatorStateDoubleJumping() => Animator.GetCurrentAnimatorStateInfo(0).IsTag("DoubleJump");
        public bool AnimatorStateFalling() => Animator.GetCurrentAnimatorStateInfo(0).IsTag("Fall");

        public virtual Vector2 Force() => Vector2.zero;

        public virtual void OnCollisionEnter2D_RPC() {
            if (AnimatorStateJumping() || AnimatorStateFalling()) Animator.SetTrigger(Grounded);
        }

        public abstract void AcceptJumpInput(InputAction.CallbackContext context);
        public virtual void AcceptDashInput(InputAction.CallbackContext context) { }
        public virtual void AcceptLockJumpInput() { }
        public virtual void AcceptUnlockJumpInput() { }
        protected bool OutOfJumps() => Config.jumpsLeft <= 0;
        protected bool OutOfDashes() => Config.dashesLeft <= 0;


        protected void RemoveXVelocity() {
            /* remove horizontal velocity for case of
             *  exiting dash state.
            */

            var vel = Rig.velocity;
            vel.x = 0;

            Rig.velocity = vel;
        }
        
        protected void RemoveYVelocity() {
            /* remove downward velocity for case of
             *  doing a double jump while falling at a great speed.
             *  if we didn't do this, the jump would not raise the player up
             * (all the negative y velocity would eat the movement)
            */

            var vel = Rig.velocity;
            vel.y = 0;

            Rig.velocity = vel;
        }


        public virtual void AcceptMoveInput(InputAction.CallbackContext context) {
            MoveDir = context.ReadValue<Single>();

            if (context.phase == InputActionPhase.Performed)
                Behaviour.transform.localScale = new Vector3((int)MoveDir,1,1);
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