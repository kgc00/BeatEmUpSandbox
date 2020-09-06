using Photon.Pun;
using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.Observer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class IdleFS : AttackFS {
        private GameObject punch1;
        public IdleFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit) : base(behaviour, stateMachine, kit) { }

        public override void Enter() => InputLockObserver.UnlockMovementInput();
        public override void Exit() { }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed || IsJumpState()) return;
            InputLockObserver.LockMovementInput();

            HandleStateChange(AttackStates.PunchOne);
        }

        protected override void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) {
        }

        protected override void _HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) {
        }
    }
}