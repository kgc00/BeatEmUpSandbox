using System;
using System.Collections.Generic;
using StateMachines.KeyLogger;
using StateMachines.Messages;
using StateMachines.Movement;
using StateMachines.Movement.Horizontal.Run;
using StateMachines.Movement.Vertical.Jumping;
using StateMachines.Observer;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines {
    public class UnitFSM : MonoBehaviour {
        [SerializeField] private JumpConfig jumpConfig;
        [SerializeField] private RunConfig runConfig;
        [SerializeField] private Rigidbody2D rig;

        // TODO refactor to use the interfaces InputProvider / CollisionEnter
        // TODO Potentially refactor to a combined "movment" object
        private JumpFSM jump;
        private RunFSM run;
        private Vector2 relativeForce;
        public InputEventObserver inputEventObserver;

        private void Awake() {
            jump = new JumpFSM(gameObject, jumpConfig);
            // ReSharper disable once Unity.InefficientPropertyAccess
            run = new RunFSM(gameObject, runConfig);
            inputEventObserver = new InputEventObserver();
        }

        private void Start() {
            inputEventObserver.OnInputEvent += SomeTest;
        }

        private void SomeTest(List<InputEvent> obj) {
            print(obj);
            print(obj.Count);
        }

        public void AcceptMoveInput(InputAction.CallbackContext context) => run.AcceptMoveInput(context);

        public void AcceptJumpInput(InputAction.CallbackContext context) => jump.AcceptJumpInput(context);

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
            GUILayout.Box(run.State.GetType().ToString());
        }
    }
}