using System;
using System.Collections;
using System.Globalization;
using Common.Extensions;
using StateMachines.Jumping;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines {
    public class UnitFSM : MonoBehaviour {
        private JumpFSM jump;
        private Vector2 moveDir;
        [SerializeField]private JumpConfig jumpConfig;

        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody2D rig;
        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private bool moving;


        [SerializeField] private float accel;
        [SerializeField] private float maxSpeed;

        private Vector2 relativeForce;

        private void Awake() {
            jump = new JumpFSM(gameObject, jumpConfig);
        }

        public void AcceptMoveInput(InputAction.CallbackContext context) {
            moveDir.x = context.ReadValue<Single>();

            moving = Math.Abs(moveDir.x) > .01f;
            if (moving) {
                animator.ResetTrigger(Idle);
                animator.SetTrigger(Running);
                transform.localScale = moveDir.x > 0 ? Vector3.one : new Vector3(-1, 1, 1);
            }
            else {
                animator.ResetTrigger(Running);
                animator.SetTrigger(Idle);
            }
        }

        public void AcceptJumpInput(InputAction.CallbackContext context) {
            jump.AcceptJumpInput(context);
        }

        private void FixedUpdate() {
            jump.Update();

            relativeForce.x = Mathf.Abs(rig.velocity.x) >= maxSpeed ? 0 : moveDir.x * accel;
            relativeForce.y = jump.Force();

            rig.AddRelativeForce(relativeForce);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            jump.OnCollisionEnter2D(other);
        }
    }
}