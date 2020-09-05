using StateMachines.Attacks.Models;
using StateMachines.Observer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks {
    public class PunchThreeFS : AttackFS {
        private GameObject punch3;
        private readonly int attack3 = Animator.StringToHash("Attack3");
        private readonly GameObject hitbox;

        public PunchThreeFS(GameObject behaviour, AttackFSM stateMachine, AttackKit attackKit) : base(behaviour,
            stateMachine, attackKit) {
            hitbox = HitboxFromKit(GetType());
        }

        public override void Enter() {
            animator.SetTrigger(attack3);
        }

        public override void Exit() {
            animator.ResetTrigger(attack3);
        }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) { }

        protected override void _EnableChaining() => InputLockObserver.UnlockJumpInput();

        protected override void _EnableHitbox() => hitbox.SetActive(true);
        protected override void _DisableHitbox() => hitbox.SetActive(false);

        protected override void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        protected override void _HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) {
            if (IsExitingAttackState())
                stateMachine.ChangeState(new IdleFS(behaviour, stateMachine, kit));
        }
    }
}