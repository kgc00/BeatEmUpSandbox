using UnityEngine;

namespace StateMachines.Attacks.Legacy {
    public class UnlockInput : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            // Observer.UnlockInput();
            animator.ResetTrigger("Idle");
        }
    }
}