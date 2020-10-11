using System;
using StateMachines.Attacks.Models;
using StateMachines.Interfaces;
using StateMachines.Logger;
using StateMachines.Network;
using StateMachines.State;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public abstract class AttackFS : FSMState<AttackFS>, IAcceptAttackInput, IHandleAttackAnimationEnter,
        IHandleAttackAnimationExit, IHandleComboChaining, IEnableAttackBuffer, IToggleHitboxes,
        IAcceptJumpInput, IAcceptRunInput, IHandleExitAnimationEvents {
        protected readonly GameObject behaviour;
        protected readonly AttackFSM stateMachine;
        protected Animator animator;
        protected AttackKit kit;
        protected UnitMovementData MovementDataValues;
        protected Rigidbody2D rig;
        protected InputLogger logger;
        protected GameObject hitbox;

        protected AttackFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit, UnitMovementData movementDataValues) {
            animator = behaviour.GetComponent<Animator>();
            rig = behaviour.GetComponent<Rigidbody2D>();
            logger = behaviour.GetComponent<InputLogger>();
            this.behaviour = behaviour;
            this.stateMachine = stateMachine;
            this.kit = kit;
            this.MovementDataValues = movementDataValues;
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

        protected virtual void _EnableAttackBuffer() {
            
        }
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

        protected void IdentifyAndTransitionToGroundedAttackState(AttackStates nextComboState = AttackStates.Idle) {
            if (logger.IsForwardAttack())
                HandleStateChange(AttackStates.GroundedForwardAttack);
            else if (logger.IsUpAttack()) HandleStateChange(AttackStates.GroundedUpAttack);
            else {
                if (nextComboState == AttackStates.Idle) return;
                if (!logger.IsRecentInput()) return;
                HandleStateChange(nextComboState);
            }
        }
    }
}