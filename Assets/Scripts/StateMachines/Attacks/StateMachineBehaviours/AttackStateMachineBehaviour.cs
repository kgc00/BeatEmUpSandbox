using UnityEngine;

namespace StateMachines.Attacks.StateMachineBehaviours {
    public class AttackStateMachineBehaviour : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.gameObject.GetComponent<UnitFSM>().HandleAttackAnimationEnter(animator,
                stateInfo,
                layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.gameObject.GetComponent<UnitFSM>().HandleAttackAnimationExit(animator,
                stateInfo,
                layerIndex);
        }
    }
}