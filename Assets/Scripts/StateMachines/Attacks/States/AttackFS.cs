﻿using System;
using Photon.Pun;
using StateMachines.Attacks.Models;
using StateMachines.Interfaces;
using StateMachines.Logger;
using StateMachines.Movement;
using StateMachines.Movement.Horizontal.Run;
using StateMachines.Movement.Models;
using StateMachines.Movement.Vertical.Jumping;
using StateMachines.Network;
using StateMachines.State;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public abstract class AttackFS : FSMState<AttackFS>, IAcceptAttackInput, IHandleExitAnimationEvents,
        IHandleComboChaining, IToggleHitboxes, IAcceptJumpInput, IAcceptRunInput {
        protected readonly GameObject behaviour;
        protected readonly AttackFSM stateMachine;
        protected readonly JumpFSM jumpStateMachine;
        protected readonly RunFSM runStateMachine;
        protected readonly int viewId;
        protected Animator animator;
        protected AttackKit kit;
        protected UnitMovementData MovementDataValues;
        protected Rigidbody2D rig;
        protected InputLogger logger;
        protected GameObject hitbox;
        protected bool chainingEnabled;

        protected AttackFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit,
            UnitMovementData movementDataValues) {
            animator = behaviour.GetComponent<Animator>();
            rig = behaviour.GetComponent<Rigidbody2D>();
            logger = behaviour.GetComponent<InputLogger>();
            runStateMachine = behaviour.GetComponent<MovementFSM>().Run;
            jumpStateMachine = behaviour.GetComponent<MovementFSM>().Jump;
            viewId = behaviour.GetComponent<PhotonView>().ViewID;
            this.behaviour = behaviour;
            this.stateMachine = stateMachine;
            this.kit = kit;
            MovementDataValues = movementDataValues;
        }

        protected bool IsDashState() =>
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Dash");

        protected bool IsJumpState() =>
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("DoubleJump") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("AirDash") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Fall");

        protected bool IsExitingAttackState() =>
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Run") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Dash") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump");

        public void AcceptAttackInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed) return;

            _AcceptAttackInput(context);
        }

        protected abstract void _AcceptAttackInput(InputAction.CallbackContext context);

        public void EnableChaining() => _EnableChaining();
        protected virtual void _EnableChaining() { }
        public void EnableAttackBuffer() => _EnableAttackBuffer();

        protected virtual void _EnableAttackBuffer() { }
        public void EnableHitbox() => _EnableHitbox();

        protected virtual void _EnableHitbox() {
            if (hitbox != null) hitbox.SetActive(true);
        }

        protected virtual void _DisableHitbox() {
            if (hitbox != null) hitbox.SetActive(false);
        }

        public void DisableHitbox() => _DisableHitbox();
        protected GameObject HitboxFromKit(Type fsType) => kit.attacks.Find(x => x?.AttckFS == fsType)?.HitboxObject;

        protected void HandleStateChange(AttackStates newState) => stateMachine.RaiseChangeStateEvent(newState);

        public virtual void AttackConnected(HitBox hitBox, Collider2D other) {
            // other.transform.root.GetComponentInChildren<HealthComponent>()?.Damage(1);
        }

        public virtual void AcceptJumpInput(InputAction.CallbackContext context) { }

        public virtual void AcceptMoveInput(InputAction.CallbackContext context) { }

        public virtual void HandleExitAnimation() {
            // animation clip reached end without interruption from player input,
            // return to idle
            HandleStateChange(AttackStates.Idle);
        }

        protected void IdentifyAndTransitionToGroundedMovementOrAttackState(bool inputWasBuffered) {
            if (!inputWasBuffered) {
                Debug.LogWarning("IdentifyAndTransitionToGroundedMovementOrAttackState without buffering is not implemented");
                return;
            }

            if (logger.DidBufferDashInput()) runStateMachine.RaiseChangeRunStateEvent(RunStates.Dash, viewId);
            if (logger.DidBufferMoveInput()) runStateMachine.RaiseChangeRunStateEvent(RunStates.Moving, viewId);
            if (logger.DidBufferJumpInput()) jumpStateMachine.RaiseChangeStateEvent(JumpStates.Launching);
            if (logger.DidBufferAttackInput()) HandleStateChange(AttackStates.GroundedNeutralOne);
        }

        protected void IdentifyAndTransitionToGroundedAttackState(AttackStates? nextComboState) {
            if (logger.IsForwardAttack())
                HandleStateChange(AttackStates.GroundedForwardAttack);
            else if (logger.IsUpAttack()) HandleStateChange(AttackStates.GroundedUpAttack);
            else {
                if (nextComboState == null) return;
                HandleStateChange((AttackStates) nextComboState);
            }
        }

        protected void IdentifyAndTransitionToGroundedAttackState(AttackStates? nextComboState, bool inputWasBuffered) {
            if (logger.IsForwardAttack())
                HandleStateChange(AttackStates.GroundedForwardAttack);
            else if (logger.IsUpAttack()) HandleStateChange(AttackStates.GroundedUpAttack);
            else {
                if (nextComboState == null) return;
                if (!logger.DidBufferAttackInput(0.25f)) return;
                HandleStateChange((AttackStates) nextComboState);
            }
        }
    }
}