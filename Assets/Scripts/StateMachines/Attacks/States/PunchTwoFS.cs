﻿using System.Collections.Generic;
using Photon.Pun;
using StateMachines.Attacks.Models;
using StateMachines.Network;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class PunchTwoFS : AttackFS {
        private GameObject punch2;
        private readonly int attack2 = Animator.StringToHash("Attack2");
        private bool chainingEnabled = false;
        private bool bufferEnabled = false;
        private Queue<InputAction.CallbackContext> bufferedActions = new Queue<InputAction.CallbackContext>();
        private readonly GameObject hitbox;

        public PunchTwoFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit) : base(behaviour, stateMachine,
            kit) {
            hitbox = HitboxFromKit(GetType());
        }

        public override void Enter() {
            animator.SetTrigger(attack2);
        }

        public override void Exit() {
            animator.ResetTrigger(attack2);
        }

        protected override void _EnableHitbox() => hitbox.SetActive(true);
        protected override void _DisableHitbox() => hitbox.SetActive(false);

        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (bufferedActions.Count > 0) {
                // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Actions.html#responding-to-actions

                // Note: The contents of the structure are only valid for the duration of the callback.
                // In particular, it isn't safe to store the received context and later access its properties from outside the callback.

                // var context = bufferedActions.Dequeue();
                // _AcceptAttackInput(context);

                HandleStateChange(AttackStates.PunchThree);
            }
        }

        protected override void _EnableAttackBuffer() => bufferEnabled = true;

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (chainingEnabled) {
                HandleStateChange(AttackStates.PunchThree);
                return;
            }

            if (bufferEnabled) bufferedActions.Enqueue(context);
        }

        protected override void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        protected override void _HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) {
            if (IsExitingAttackState()) {
                HandleStateChange(AttackStates.Idle);
            }
        }
    }
}