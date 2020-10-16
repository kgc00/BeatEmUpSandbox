using General;
using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class AerialDownAttack : AttackFS {
        private readonly int aerialDownAttack = Animator.StringToHash("air-down-attack");

        public AerialDownAttack(GameObject behaviour, AttackFSM stateMachine, AttackKit kit,
            UnitMovementData movementDataValues) :
            base(behaviour, stateMachine, kit, movementDataValues) {
            hitbox = HitboxFromKit(GetType());
            isAerialState = true;
        }

        public override void Enter() {
            animator.Play(aerialDownAttack);
            EnterAerialAttackState();
        }

        public override void Update() {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("air-down-attack"))
                animator.Play(aerialDownAttack);
        }

        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (chainingEnabled) IdentifyAndTransitionToAerialMovementOrAttackState(true);
        }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) { }

        public override void AttackConnected(int id) {
            base.AttackConnected(id);

            var other = Helpers.GameObjectFromId(id);
            if (other == null) return;
            
            var enemyRig = other.transform.root.GetComponentInChildren<Rigidbody2D>();
            if (enemyRig) Helpers.AddForceY(enemyRig, -250);
        }
    }
}