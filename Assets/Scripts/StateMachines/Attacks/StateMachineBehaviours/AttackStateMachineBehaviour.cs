using StateMachines.Attacks.Legacy;
using UnityEngine;

namespace StateMachines.Attacks.StateMachineBehaviours {
    public class AttackStateMachineBehaviour : StateMachineBehaviour {
        // TODO : Use events or cache reference
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.gameObject.GetComponent<AttackFSM>().HandleAttackAnimationEnter(animator,
                stateInfo,
                layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.gameObject.GetComponent<AttackFSM>().HandleAttackAnimationExit(animator,
                stateInfo,
                layerIndex);
        }
    }
}