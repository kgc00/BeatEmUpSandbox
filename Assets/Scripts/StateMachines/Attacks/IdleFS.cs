using StateMachines.Movement.Vertical.Jumping;
using StateMachines.Observer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks {
    public class IdleFS : AttackFS {
        private GameObject punch1;
        public IdleFS(GameObject behaviour, AttackFSM stateMachine) : base(behaviour, stateMachine) { }

        public override void Enter() => InputLockObserver.UnlockInput();
        public override void Exit() => InputLockObserver.LockInput();

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed || IsJumpState()) return;
            stateMachine.ChangeState(new PunchOneFS(behaviour, stateMachine));
        }

        protected override void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) {
        }

        protected override void _HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) {
        }
    }
}