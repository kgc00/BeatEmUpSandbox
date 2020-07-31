using System;
using UnityEngine;

namespace StateMachines.Attacks {
    public class UnlockInput : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Observer.UnlockInput();
            animator.ResetTrigger("Idle");
        }
    }
}