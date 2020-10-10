using System;
using System.Collections.Generic;
using StateMachines.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.Legacy {
    public class Attack : MonoBehaviour, IAcceptAttackInput {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject punch1;
        [SerializeField] private GameObject punch2;
        [SerializeField] private GameObject punch3;
        private readonly int attack1 = Animator.StringToHash("Attack1");
        private readonly int attack2 = Animator.StringToHash("Attack2");
        private readonly int attack3 = Animator.StringToHash("Attack3");
        public List<GameObject> attacks;

        private void Start() {
            attacks = new List<GameObject> {
                punch1,
                punch2,
                punch3
            };
        }

        public void AcceptAttackInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed) return;

            HandleAttack();
        }

        private void HandleAttack() {
            if (NotAttacking()) animator.SetTrigger(attack1);
            else if (IsAttack1()) {
                // animator.ResetTrigger(attack1);
                // print(animator.GetAnimatorTransitionInfo(0).);
                animator.SetTrigger(attack2);
            }
            else if (IsAttack2()) {
                // animator.ResetTrigger(attack2);
                animator.SetTrigger(attack3);
            }
        }

        private bool NotAttacking() => !IsAttack1() && !IsAttack2() && !IsAttack3();
        private bool IsAttack1() => animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack1");
        private bool IsAttack2() => animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack2");
        private bool IsAttack3() => animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack3");

        public void ToggleAttack1Hitbox(int newStatus) {
            attacks[0].SetActive(newStatus == 1);
            // if (newStatus == 1) Observer.LockInput();
        }

        public void ToggleAttack2Hitbox(int newStatus) {
            attacks[1].SetActive(newStatus == 1);
        }
        
        public void ToggleAttack3Hitbox(int newStatus) {
            attacks[2].SetActive(newStatus == 1);
        }

        public (GameObject, Action) AssignAttack(AnimatorStateInfo stateInfo) {
            if (stateInfo.IsTag("Attack1")) return (attacks[0], () => { });
            else if (stateInfo.IsTag("Attack2")) return (attacks[1], () => { });
            else if (stateInfo.IsTag("Attack3")) return (attacks[2], () => {});
            // else if (stateInfo.IsTag("Attack3")) return (attacks[2], () => Observer.UnlockInput());

            print("Unable to assign hitbox to " + stateInfo);
            return (null, () => {});
        }
    }
}