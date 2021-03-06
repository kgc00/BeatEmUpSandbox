﻿using System.Collections.Generic;
using Photon.Pun;
using StateMachines.Attacks.Models;
using StateMachines.Network;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class PunchOneFS : AttackFS {
        private GameObject punch1;
        private readonly int attack1 = Animator.StringToHash("Attack1");
        private bool chainingEnabled;
        private bool bufferEnabled;
        private Queue<InputAction.CallbackContext> bufferedActions = new Queue<InputAction.CallbackContext>();

        private readonly GameObject hitbox;

        public PunchOneFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit) : base(behaviour, stateMachine,
            kit) {
            hitbox = HitboxFromKit(GetType());
        }

        public override void Enter() {
            animator.SetTrigger(attack1);
        }

        public override void Exit() {
            animator.ResetTrigger(attack1);
        }

        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (bufferedActions.Count <= 0) return;

            // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Actions.html#responding-to-actions

            // Note: The contents of the structure are only valid for the duration of the callback.
            // In particular, it isn't safe to store the received context and later access its properties from outside the callback.

            // var context = bufferedActions.Dequeue();
            // _AcceptAttackInput(context);

            HandleStateChange(AttackStates.PunchTwo);
        }

        protected override void _EnableAttackBuffer() => bufferEnabled = true;

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (chainingEnabled) HandleStateChange(AttackStates.PunchTwo);
            else if (bufferEnabled) bufferedActions.Enqueue(context);
        }

        protected override void _HandleAttackAnimationEnter(
            Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        protected override void _HandleAttackAnimationExit(
            Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) {
            if (IsExitingAttackState()) HandleStateChange(AttackStates.Idle);
        }

        protected override void _EnableHitbox() => hitbox.SetActive(true);
        protected override void _DisableHitbox() => hitbox.SetActive(false);
    }
}