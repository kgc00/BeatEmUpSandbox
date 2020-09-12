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
    public abstract class JumpFS : FSMState<JumpFS>, IHandleLockedJumpInput, IAcceptDashInput {
        protected readonly Animator Animator;
        protected readonly Rigidbody2D Rig;
        protected readonly GameObject Behaviour;
        protected readonly JumpFSM Jump;
        protected readonly JumpConfig Config;
        protected readonly int Grounded = Animator.StringToHash("Grounded");
        protected readonly int Jumping = Animator.StringToHash("Jumping");
        protected readonly int DoubleJumping = Animator.StringToHash("DoubleJumping");
        protected JumpFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) {
            Jump = jump;
            Behaviour = behaviour;
            Config = jumpConfig;
            Animator = behaviour.GetComponent<Animator>();
            Rig = behaviour.GetComponent<Rigidbody2D>();
        }
        
        public bool AnimatorStateJumping() => Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump");
        public bool AnimatorStateFalling() => Animator.GetCurrentAnimatorStateInfo(0).IsTag("Fall");
        
        public virtual float Force() => 0f;
        public virtual void OnCollisionEnter2D_RPC() {
            if (AnimatorStateJumping() || AnimatorStateFalling()) Animator.SetTrigger(Grounded);
        }
        public abstract void AcceptJumpInput(InputAction.CallbackContext context);
        public virtual void AcceptDashInput(InputAction.CallbackContext context) { }
        public virtual void AcceptLockJumpInput() { }
        public virtual void AcceptUnlockJumpInput() { }
        protected bool OutOfJumps()  => Config.jumpsLeft <= 0;
        protected bool OutOfDashes()  => Config.dashesLeft <= 0;
    }
}