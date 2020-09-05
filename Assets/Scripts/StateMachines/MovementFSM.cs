using System;
using System.Collections.Generic;
using Photon.Pun;
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
    public class MovementFSM : MonoBehaviourPun, IAcceptRunInput, IAcceptJumpInput {
        [SerializeField] private JumpConfig jumpConfig;
        [SerializeField] private RunConfig runConfig;
        [SerializeField] private Rigidbody2D rig;
        public JumpFSM Jump {get; private set;}
        public RunFSM Run {get; private set;}
        private Vector2 relativeForce;

        private void Awake() {
            // ReSharper disable twice Unity.InefficientPropertyAccess
            Jump = new JumpFSM(gameObject, jumpConfig);
            Run = new RunFSM(gameObject, runConfig);
        }

        public void AcceptMoveInput(InputAction.CallbackContext context) => Run.AcceptMoveInput(context);
        public void AcceptJumpInput(InputAction.CallbackContext context) => Jump.AcceptJumpInput(context);

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
            GUILayout.Box("rig velocity: " + rig.velocity);
            GUILayout.Box("run: " + Run.State.GetType());
            GUILayout.Box("jump: " + Jump.State.GetType());
        }
    }
}