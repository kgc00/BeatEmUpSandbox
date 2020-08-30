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
    public abstract class JumpFS : FSMState<JumpFS>, IAcceptLockedInput {
        protected readonly Animator Animator;
        protected readonly Rigidbody2D Rig;
        protected readonly GameObject Behaviour;
        protected readonly JumpFSM Jump;
        protected readonly JumpConfig Config;
        protected readonly int Grounded = Animator.StringToHash("Grounded");
        protected readonly int Jumping = Animator.StringToHash("Jumping");
        protected JumpFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) {
            Jump = jump;
            Behaviour = behaviour;
            Config = jumpConfig;
            Animator = behaviour.GetComponent<Animator>();
            Rig = behaviour.GetComponent<Rigidbody2D>();
        }
        
        public bool AnimatorStateJumping() => Animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump");
        public bool AnimatorStateFalling() => Animator.GetCurrentAnimatorStateInfo(0).IsName("player_fall");
        public virtual float Force() => 0f;
        public virtual void OnCollisionEnter2D(Collision2D other) {
            if (!other.gameObject.CompareTag("Board")) return;
            
            if (AnimatorStateJumping() || AnimatorStateFalling()) Animator.SetTrigger(Grounded);
        }
        public abstract void AcceptJumpInput(InputAction.CallbackContext context);
        public virtual void AcceptLockInput() { }
        public virtual void AcceptUnlockInput() { }
    }
}