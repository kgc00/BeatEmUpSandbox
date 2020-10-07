using System;
using StateMachines.Attacks.Models;
using StateMachines.Interfaces;
using StateMachines.Network;
using StateMachines.State;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public abstract class AttackFS : FSMState<AttackFS>, IAcceptAttackInput, IHandleAttackAnimationEnter,
        IHandleAttackAnimationExit, IHandleComboChaining, IEnableAttackBuffer, IToggleHitboxes,
        IAcceptJumpInput, IAcceptRunInput {
        protected readonly GameObject behaviour;
        protected readonly AttackFSM stateMachine;
        protected Animator animator;
        protected AttackKit kit;
        protected UnitState stateValues;
        protected Rigidbody2D rig;
        protected AttackFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit, UnitState stateValues) {
            animator = behaviour.GetComponent<Animator>();
            rig = behaviour.GetComponent<Rigidbody2D>();
            this.behaviour = behaviour;
            this.stateMachine = stateMachine;
            this.kit = kit;
            this.stateValues = stateValues;
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
        protected virtual void _EnableAttackBuffer() { }
        public void EnableHitbox() => _EnableHitbox();
        protected virtual void _EnableHitbox() { }
        public void DisableHitbox() => _DisableHitbox();
        protected virtual void _DisableHitbox() { }
        protected GameObject HitboxFromKit(Type fsType) => kit.attacks.Find(x => x.AttckFS == fsType).HitboxObject;

        protected void HandleStateChange(AttackStates newState) => stateMachine.RaiseChangeStateEvent(newState);

        private void LogInfo() {
            Debug.Log(animator.GetNextAnimatorClipInfo(0));
            Debug.Log(animator.GetNextAnimatorClipInfo(0)[0].clip);
            Debug.Log(animator.GetNextAnimatorClipInfo(0)[0].clip.name);

            Debug.Log("CURRENT STATE INFO --- ");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
            Debug.Log("Logging Attack1");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack1"));
            Debug.Log("Logging Attack2");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack2"));
            Debug.Log("Logging Idle");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
            Debug.Log("Logging Jump");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"));
            Debug.Log("Logging Run");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Run"));

            Debug.Log("NEXT STATE INFO --- ");
            Debug.Log(animator.GetNextAnimatorStateInfo(0));
            Debug.Log("Logging Attack1");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Attack1"));
            Debug.Log("Logging Attack2");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Attack2"));
            Debug.Log("Logging Idle");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Idle"));
            Debug.Log("Logging Jump");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Jump"));
            Debug.Log("Logging Run");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Run"));
        }

        public virtual void AttackConnected(HitBox hitBox, Collider2D other) {
            // other.transform.root.GetComponentInChildren<HealthComponent>()?.Damage(1);
        }

        public virtual void AcceptJumpInput(InputAction.CallbackContext context) { }

        public virtual void AcceptMoveInput(InputAction.CallbackContext context) { }
    }
}