using System.Collections;
using General;
using StateMachines.Attacks.Models;
using StateMachines.Movement;
using StateMachines.Movement.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class GroundedUpAttackFS : AttackFS {
        public GroundedUpAttackFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit,
            UnitMovementData movementDataValues) : base(behaviour, stateMachine, kit, movementDataValues) {
            hitbox = HitboxFromKit(GetType());
        }

        public override void Enter() {
            animator.Play("ground-up-attack");
        }

        public override void Update() {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("ground-up-attack"))
                animator.Play("ground-up-attack");
        }

        public override void AttackConnected(int id) {
            base.AttackConnected(id);

            var other = Helpers.GameObjectFromId(id);
            if (other == null) return;

            var otherRig = other.transform.root.GetComponentInChildren<Rigidbody2D>();
            if (otherRig == null) return;
            
            stateMachine.DoCoroutine(LaunchOther(otherRig));
            
            // var jump = other.gameObject.transform.root.GetComponentInChildren<MovementFSM>()?.Jump;
            // jump?.RaiseChangeStateEvent(JumpStates.Falling);
        }

        private IEnumerator LaunchOther(Rigidbody2D otherRig) {
            for (int i = 0; i < 6; i++) {
                Helpers.AddForceY(otherRig, 30);
            }
            yield break;
        }

        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (chainingEnabled) IdentifyAndTransitionToGroundedMovementOrAttackState(true);
        }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) { }

        public override void HandleExitAnimation() {
            HandleStateChange(AttackStates.Idle);
        }
    }
}