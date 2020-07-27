using System;
using System.Collections;
using System.Globalization;
using Common.Extensions;
using StateMachines.Jumping;
using StateMachines.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines {
    public class UnitFSM : MonoBehaviour {
        [SerializeField]private JumpConfig jumpConfig;
        [SerializeField]private RunConfig runConfig;
        [SerializeField] private Rigidbody2D rig;
        
        // TODO refactor to use the interfaces InputProvider / CollisionEnter
        // TODO Potentially refactor to a combined "movment" object
        private JumpFSM jump;
        private Run run;
        private Vector2 relativeForce;

        private void Awake() {
            jump = new JumpFSM(gameObject, jumpConfig);
            // ReSharper disable once Unity.InefficientPropertyAccess
            run = new Run(gameObject, runConfig);
        }

        public void AcceptMoveInput(InputAction.CallbackContext context) => run.AcceptMoveInput(context);

        public void AcceptJumpInput(InputAction.CallbackContext context) => jump.AcceptJumpInput(context);

        private void FixedUpdate() {
            jump.Update();

            relativeForce.x = run.Force();
            relativeForce.y = jump.Force();
            
            rig.AddRelativeForce(relativeForce);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            jump.OnCollisionEnter2D(other);
            run.OnCollisionEnter2D(other);
        }
    }
}