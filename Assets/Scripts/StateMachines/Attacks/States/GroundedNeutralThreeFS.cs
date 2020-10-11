﻿using Photon.Pun;
using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class GroundedNeutralThreeFS : AttackFS {
        private GameObject punch3;
        private readonly int attack3 = Animator.StringToHash("GroundedNeutral3");
        private readonly GameObject hitbox;
        private bool chainingEnabled;

        public GroundedNeutralThreeFS(GameObject behaviour, AttackFSM stateMachine, AttackKit attackKit,
            UnitMovementData movementDataValues) : base(behaviour,
            stateMachine, attackKit, movementDataValues) {
            hitbox = HitboxFromKit(GetType());
        }

        public override void Enter() {
            animator.Play(attack3);
        }

        public override void Exit() { }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (chainingEnabled) IdentifyAndTransitionToGroundedAttackState();
        }

        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (chainingEnabled) IdentifyAndTransitionToGroundedAttackState();
        }

        protected override void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        protected override void _HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) {
            if (IsExitingAttackState()) HandleStateChange(AttackStates.Idle);
        }
    }
}