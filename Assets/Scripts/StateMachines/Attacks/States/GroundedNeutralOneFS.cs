using System.Collections.Generic;
using Photon.Pun;
using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class GroundedNeutralOneFS : AttackFS {
        private GameObject punch1;
        private readonly int attack1 = Animator.StringToHash("GroundedNeutral1");
        private bool chainingEnabled;
        private bool bufferEnabled;
        private Queue<InputAction.CallbackContext> bufferedActions = new Queue<InputAction.CallbackContext>();

        public GroundedNeutralOneFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit, UnitMovementData movementDataValues)
            : base(behaviour, stateMachine,
                kit, movementDataValues) {
            hitbox = HitboxFromKit(GetType());
        }

        public override void Enter() {
            animator.Play(attack1);
        }

        public override void Exit() { }

        protected override void _EnableChaining() {
            chainingEnabled = true;
            IdentifyAndTransitionToGroundedAttackState(AttackStates.GroundedNeutralTwo);
        }

        protected override void _EnableAttackBuffer() => bufferEnabled = true;

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (chainingEnabled) IdentifyAndTransitionToGroundedAttackState(AttackStates.GroundedNeutralTwo);
        }

        protected override void _HandleAttackAnimationEnter(
            Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        protected override void _HandleAttackAnimationExit(
            Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) {
            if (IsExitingAttackState()) HandleStateChange(AttackStates.Idle);
        }
    }
}