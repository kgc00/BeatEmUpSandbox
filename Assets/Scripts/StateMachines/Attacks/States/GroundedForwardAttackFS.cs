using StateMachines.Attacks.Models;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class GroundedForwardAttackFS : AttackFS {
        public GroundedForwardAttackFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit,
            UnitState stateValues) : base(behaviour, stateMachine, kit, stateValues) { }
        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            throw new System.NotImplementedException();
        }

        protected override void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            throw new System.NotImplementedException();
        }

        protected override void _HandleAttackAnimationExit(Animator animator1, AnimatorStateInfo stateInfo, int layerIndex) {
            throw new System.NotImplementedException();
        }
    }
}