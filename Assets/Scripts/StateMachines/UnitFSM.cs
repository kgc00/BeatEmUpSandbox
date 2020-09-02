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

        public AttackFSM Attack {get; private set;}
        public JumpFSM Jump {get; private set;}
        public RunFSM Run {get; private set;}
        private Vector2 relativeForce;

        private void Awake() {
            // ReSharper disable twice Unity.InefficientPropertyAccess
            Jump = new JumpFSM(gameObject, jumpConfig);
            Run = new RunFSM(gameObject, runConfig);
            Attack = new AttackFSM(gameObject);
        }

        public void AcceptMoveInput(InputAction.CallbackContext context) => Run.AcceptMoveInput(context);
        public void AcceptJumpInput(InputAction.CallbackContext context) => Jump.AcceptJumpInput(context);
        public void AcceptAttackInput(InputAction.CallbackContext context) => Attack.AcceptAttackInput(context);

        public void EnableComboChaining() => Attack.EnableChaining();
        public void EnableAttackBuffer() => Attack.EnableAttackBuffer();
        
        public void ToggleAttack1Hitbox(int newState) { }
        public void ToggleAttack2Hitbox(int newState) { }
        public void ToggleAttack3Hitbox(int newState) { }

        private void FixedUpdate() {
            Jump.Update();

            relativeForce.x = Run.Force();
            relativeForce.y = Jump.Force();

            rig.AddForce(relativeForce, ForceMode2D.Force);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            Jump.OnCollisionEnter2D(other);
            Run.OnCollisionEnter2D(other);
        }

        private void OnGUI() {
            GUILayout.Box(rig.velocity.ToString());
            GUILayout.Box("attack: " + Attack.State.GetType());
            GUILayout.Box("run: " + Run.State.GetType());
            GUILayout.Box("jump: " + Jump.State.GetType());
        }

        public void HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            Attack.HandleAttackAnimationEnter(animator,
                stateInfo,
                layerIndex);

        public void HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            Attack.HandleAttackAnimationExit(animator,
                stateInfo,
                layerIndex);
    }
}