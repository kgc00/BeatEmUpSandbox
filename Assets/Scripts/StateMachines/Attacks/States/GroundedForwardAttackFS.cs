using General;
using StateMachines.Attacks.Models;
using StateMachines.Movement;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class GroundedForwardAttackFS : AttackFS {
        public GroundedForwardAttackFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit,
            UnitMovementData movementDataValues) : base(behaviour, stateMachine, kit, movementDataValues) {
            hitbox = HitboxFromKit(GetType()); 
        }

        public override void Enter() {
            animator.Play("ground-forward-attack");
        }

        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (chainingEnabled) IdentifyAndTransitionToGroundedMovementOrAttackState( true);
        }
        
        protected override void _AcceptAttackInput(InputAction.CallbackContext context) { }


        public override void Update() {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("ground-forward-attack"))
                animator.Play("ground-forward-attack");
        }

        public override void AttackConnected(int id) {
            base.AttackConnected(id);
            
            var other = Helpers.GameObjectFromId(id);
            if (other == null) return;
            
            var dir = Helpers.GetDir(other, behaviour);
            
            var enemyRig = other.transform.root.GetComponentInChildren<Rigidbody2D>();
            if (enemyRig) Helpers.AddForceX(enemyRig, dir * 250);
        }
    }
}