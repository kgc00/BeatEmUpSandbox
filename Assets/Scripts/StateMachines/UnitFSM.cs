using System;
using System.Collections.Generic;
using StateMachines.Attacks;
using StateMachines.Interfaces;
using StateMachines.Messages;
using StateMachines.Movement;
using StateMachines.Movement.Horizontal.Run;
using StateMachines.Movement.Vertical.Jumping;
using StateMachines.Observer;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines {
    public class UnitFSM : MonoBehaviour, IAcceptAttackInput, IAcceptRunInput, IAcceptJumpInput,
        IHandleAttackAnimationEnter, IHandleAttackAnimationExit {
        [SerializeField] private JumpConfig jumpConfig;
        [SerializeField] private RunConfig runConfig;
        [SerializeField] private Rigidbody2D rig;

        private AttackFSM attack;
        private JumpFSM jump;
        private RunFSM run;
        private Vector2 relativeForce;

        private void Awake() {
            // ReSharper disable twice Unity.InefficientPropertyAccess
            jump = new JumpFSM(gameObject, jumpConfig);
            run = new RunFSM(gameObject, runConfig);
            attack = new AttackFSM(gameObject);
        }

        public void AcceptMoveInput(InputAction.CallbackContext context) => run.AcceptMoveInput(context);
        public void AcceptJumpInput(InputAction.CallbackContext context) => jump.AcceptJumpInput(context);
        public void AcceptAttackInput(InputAction.CallbackContext context) => attack.AcceptAttackInput(context);
        public void ToggleAttack1Hitbox(int newState) { }
        public void ToggleAttack2Hitbox(int newState) { }
        public void ToggleAttack3Hitbox(int newState) { }

        private void FixedUpdate() {
            jump.Update();

            relativeForce.x = run.Force();
            relativeForce.y = jump.Force();

            rig.AddForce(relativeForce, ForceMode2D.Force);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            jump.OnCollisionEnter2D(other);
            run.OnCollisionEnter2D(other);
        }

        private void OnGUI() {
            GUILayout.Box(rig.velocity.ToString());
            GUILayout.Box(attack.State.GetType().ToString());
        }

        public void HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            attack.HandleAttackAnimationEnter(animator,
                stateInfo,
                layerIndex);

        public void HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            attack.HandleAttackAnimationExit(animator,
                stateInfo,
                layerIndex);
    }
}