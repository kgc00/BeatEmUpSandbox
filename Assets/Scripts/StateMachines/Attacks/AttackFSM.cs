using StateMachines.Attacks.Legacy;
using StateMachines.Interfaces;
using StateMachines.Movement.Horizontal.Run;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks {
    public class AttackFSM : IAcceptAttackInput, IChangeState<AttackFS>, IHandleAttackAnimationEnter, IHandleAttackAnimationExit {
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
    }
}