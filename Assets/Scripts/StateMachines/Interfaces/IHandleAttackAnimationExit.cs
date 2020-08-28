using UnityEngine;

namespace StateMachines.Interfaces {
    public interface IHandleAttackAnimationExit {
        void HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
    }
}