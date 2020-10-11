﻿using System.Collections.Generic;
using Photon.Pun;
using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class GroundedNeutralOneFS : AttackFS {
        private readonly int attack1 = Animator.StringToHash("GroundedNeutral1");

        public GroundedNeutralOneFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit, UnitMovementData movementDataValues)
            : base(behaviour, stateMachine,
                kit, movementDataValues) {
            hitbox = HitboxFromKit(GetType());
        }

        public override void Enter() {
            animator.Play(attack1);
        }

        public override void Update() {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("GroundedNeutral1"))
                animator.Play(attack1);
        }


        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (chainingEnabled) IdentifyAndTransitionToGroundedAttackState(AttackStates.GroundedNeutralTwo, true);
        }


        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (chainingEnabled) IdentifyAndTransitionToGroundedAttackState(AttackStates.GroundedNeutralTwo);
        }
    }
}