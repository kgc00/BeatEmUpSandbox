using UnityEngine;

namespace StateMachines.Interfaces {
    public interface IHandleAttackAnimationEnter {
        void HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
    }
}