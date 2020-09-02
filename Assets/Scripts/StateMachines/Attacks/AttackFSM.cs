using StateMachines.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks {
    public class AttackFSM : IAcceptAttackInput, IChangeState<AttackFS>, IHandleAttackAnimationEnter, IHandleAttackAnimationExit, IHandleComboChaining, IEnableAttackBuffer {
        private GameObject behaviour;
        public AttackFS State { get; private set; }
        public AttackFSM(GameObject behaviour) {
            this.behaviour = behaviour;
            State = new IdleFS(behaviour, this);
        }

        public void AcceptAttackInput(InputAction.CallbackContext context) => State.AcceptAttackInput(context);
        
        public void ChangeState(AttackFS newState) {
            State.Exit();
            State = newState;
            State.Enter();
        }

        public void HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            State.HandleAttackAnimationEnter(animator, stateInfo, layerIndex);
        }

        public void HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            State.HandleAttackAnimationExit(animator, stateInfo, layerIndex);
        }

        public void EnableChaining() => State.EnableChaining();
        public void EnableAttackBuffer() => State.EnableAttackBuffer();
    }
}