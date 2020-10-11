﻿using General;
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
            Debug.Log("Entering Grounded Forward ------");
            Debug.Log("Run State: " + Helpers.GetUniqueStateName(behaviour.GetComponent<MovementFSM>().Run.State.ToString()));
            Debug.Log("Jump State: " + Helpers.GetUniqueStateName(behaviour.GetComponent<MovementFSM>().Jump.State.ToString()));
            animator.StopPlayback();
            animator.Play("ground-forward-attack");
        }

        public override void Update() {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("ground-forward-attack"))
                animator.Play("ground-forward-attack");
        }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) { }

        protected override void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        protected override void _HandleAttackAnimationExit(Animator animator1, AnimatorStateInfo stateInfo,
            int layerIndex) { }
    }
}