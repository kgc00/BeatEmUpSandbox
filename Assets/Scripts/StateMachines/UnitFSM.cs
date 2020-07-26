using System;
using System.Collections;
using System.Globalization;
using Common.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines {
    public class UnitFSM : MonoBehaviour {
        private FSMState state;
        private Vector2 moveDir;

        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody2D rig;
        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private bool moving;

        [SerializeField] private float jumpDuration;
        [SerializeField] private float jumpVelocity;
        [SerializeField] private float fallMultiplier;
        [SerializeField] private float lowJumpMultiplier;

        [SerializeField] private float accel;
        [SerializeField] private float maxSpeed;
        
        public enum JumpStatus {
            Grounded,
            Launching,
            Launched,
            Falling
        }

        private JumpStatus js;

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
            if (context.phase == InputActionPhase.Performed) {
                if (js != JumpStatus.Grounded) return;
                js = JumpStatus.Launching;
                moveDir.y = context.ReadValue<Single>();
                animator.SetTrigger(Jumping);
                StartCoroutine(LaunchingStatusLoop());
            }
            else if (context.phase == InputActionPhase.Canceled) {
                if (js != JumpStatus.Launching) return;
                js = JumpStatus.Launched;
                moveDir.y = context.ReadValue<Single>();
            }
        }
        
        private void FixedUpdate() {
            var relativeX = moveDir.x * accel;
            var relativeY = 0f;

            if (js != JumpStatus.Grounded) {
                relativeY = moveDir.y * jumpVelocity;
            }

            if (js == JumpStatus.Launching) {
                rig.gravityScale = 1f;
            }
            else if (js == JumpStatus.Launched) {
                rig.gravityScale = lowJumpMultiplier;
                if (rig.velocity.y < 0) js = JumpStatus.Falling;
            }
            else if (js == JumpStatus.Falling) {
                rig.gravityScale = fallMultiplier;
            }

            var relativeForce = new Vector2(relativeX, relativeY);
            if (Mathf.Abs(rig.velocity.x) >= maxSpeed) relativeForce.x = 0;
            if (Mathf.Abs(rig.velocity.y) >= maxSpeed) relativeForce.y = 0;
            rig.AddRelativeForce(relativeForce);
        }

        private void OnGUI() {
            GUILayout.Box(js.ToString());
            GUILayout.Box(rig.gravityScale.ToString());
        }

        private bool AnimatorStateJumping() => animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump");
        private bool AnimatorStateFalling() => animator.GetCurrentAnimatorStateInfo(0).IsName("player_fall");
        private void OnCollisionEnter2D(Collision2D other) {
            js = JumpStatus.Grounded;
            
            if (AnimatorStateJumping() || AnimatorStateFalling()) animator.SetTrigger(Grounded);
        }
        
        public IEnumerator LaunchingStatusLoop() {
            var timeLapsed = 0f;
            while (timeLapsed < jumpDuration) {
                if (js != JumpStatus.Launching) {
                    yield break;
                }

                timeLapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            if(js == JumpStatus.Launching) {
                js = JumpStatus.Launched;
            }
            
            moveDir.y = 0f;
        }
    }
}