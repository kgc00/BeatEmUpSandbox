using System;
using UnityEngine;

namespace StateMachines.Attacks {
    public class DeactivateHitbox : StateMachineBehaviour {
        private GameObject hitbox;
        private Action cb;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            (hitbox, cb) = animator.gameObject.GetComponent<Attack>().AssignAttack(stateInfo);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            hitbox.SetActive(false);
            // Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Idle"));
            // Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Run"));
            // Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Attack1"));
            // cb();
        }
    }
}