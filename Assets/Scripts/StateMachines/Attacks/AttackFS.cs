using System;
using StateMachines.Attacks.Models;
using StateMachines.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks {
    public abstract class AttackFS : FSMState, IAcceptAttackInput, IHandleAttackAnimationEnter,
        IHandleAttackAnimationExit, IHandleComboChaining, IEnableAttackBuffer, IToggleHitboxes, IUpdateAttackPhase {
        protected readonly GameObject behaviour;
        protected readonly UnitFSM stateMachine;
        protected Animator animator;
        protected AttackKit kit;

        protected AttackFS(GameObject behaviour, UnitFSM stateMachine, AttackKit kit) {
            animator = behaviour.GetComponent<Animator>();
            this.behaviour = behaviour;
            this.stateMachine = stateMachine;
            this.kit = kit;
        }

        public AttackPhase Phase { get; protected set; } = AttackPhase.Startup;
        public virtual void EnterStartup() => Phase = AttackPhase.Startup;
        public virtual void EnterActive()  => Phase = AttackPhase.Active;
        public virtual void EnterRecovery()  => Phase = AttackPhase.Recovery;

        protected bool IsJumpState() =>
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Fall");

        protected bool IsExitingAttackState() =>
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Run") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump");

        public void AcceptAttackInput(InputAction.CallbackContext context) => _AcceptAttackInput(context);
        protected abstract void _AcceptAttackInput(InputAction.CallbackContext context);

        public void HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            _HandleAttackAnimationEnter(animator, stateInfo, layerIndex);
        }

        protected abstract void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex);

        public void HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            _HandleAttackAnimationExit(animator, stateInfo, layerIndex);
        }

        protected abstract void _HandleAttackAnimationExit(Animator animator1, AnimatorStateInfo stateInfo,
            int layerIndex);

        public void EnableChaining() => _EnableChaining();
        protected virtual void _EnableChaining() { }
        public void EnableAttackBuffer() => _EnableAttackBuffer();
        protected virtual void _EnableAttackBuffer() { }
        public void EnableHitbox() => _EnableHitbox();
        protected virtual void _EnableHitbox() { }
        public void DisableHitbox() => _DisableHitbox();
        protected virtual void _DisableHitbox() { }

        protected GameObject HitboxFromKit(Type fsType) => kit.attacks.Find(x => x.AttckFS == fsType).HitboxObject;
    }
}