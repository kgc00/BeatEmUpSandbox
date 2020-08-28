using StateMachines.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks {
    public abstract class AttackFS : FSMState<AttackFS>, IAcceptAttackInput,IHandleAttackAnimationEnter, IHandleAttackAnimationExit {
        protected readonly GameObject behaviour;
        protected readonly AttackFSM stateMachine;
        protected Animator animator;

        protected AttackFS(GameObject behaviour, AttackFSM stateMachine) {
            animator = behaviour.GetComponent<Animator>();
            this.behaviour = behaviour;
            this.stateMachine = stateMachine;
        }
        
        public void AcceptAttackInput(InputAction.CallbackContext context) => _AcceptAttackInput(context);
        protected abstract void _AcceptAttackInput(InputAction.CallbackContext context);
        public void HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            _HandleAttackAnimationEnter(animator, stateInfo, layerIndex);
        }
        protected abstract void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
        public void HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            _HandleAttackAnimationExit(animator, stateInfo, layerIndex);
        }
        protected abstract void _HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
    }
}