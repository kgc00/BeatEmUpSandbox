using System;
using General;
using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class AerialForwardAttack : AttackFS {
        private readonly int aerialForwardAttack = Animator.StringToHash("air-forward-attack");

        public AerialForwardAttack(GameObject behaviour, AttackFSM stateMachine, AttackKit kit,
            UnitMovementData movementDataValues) :
            base(behaviour, stateMachine, kit, movementDataValues) {
            hitbox = HitboxFromKit(GetType());
            isAerialState = true;
            EnterAerialAttackState();
        }

        public override void Enter() {
            animator.Play(aerialForwardAttack);
            EnterAerialAttackState();
            rig.gravityScale /= 4;
        }


        public override void Update() {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("air-forward-attack"))
                animator.Play(aerialForwardAttack);
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

            var dir = Helpers.GetDir(other, behaviour);
            
            var enemyRig = other.transform.root.GetComponentInChildren<Rigidbody2D>();
            if (enemyRig) Helpers.AddForceX(enemyRig,  dir * 250);
        }
    }
}