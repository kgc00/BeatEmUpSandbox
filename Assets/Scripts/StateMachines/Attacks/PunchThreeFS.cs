using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks {
    public class PunchThreeFS : AttackFS {
        private GameObject punch3;
        private readonly int attack3 = Animator.StringToHash("Attack3");
        
        public PunchThreeFS(GameObject behaviour, AttackFSM stateMachine) : base(behaviour, stateMachine) {}

        public override void Enter() {
            animator.SetTrigger(attack3);
        }

        public override void Exit() {
            animator.ResetTrigger(attack3);
        }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) { }
        protected override void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        }

        protected override void _HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            stateMachine.ChangeState(new IdleFS(behaviour, stateMachine));
        }
    }
}