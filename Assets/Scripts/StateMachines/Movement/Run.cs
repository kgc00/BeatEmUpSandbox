using System;
using StateMachines.Jumping.Interfaces;
using StateMachines.Movement.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement {
    public class Run : IAcceptRunInput, IProvideForce, IAcceptCollisionEnter{
        private readonly Animator animator;
        private readonly Transform transform;
        private readonly RunConfig config;
        private readonly Rigidbody2D rig;
        
        private readonly int running = Animator.StringToHash("Running");
        private readonly int idle = Animator.StringToHash("Idle");

        private float moveDir;
        private bool moving;

        public Run(GameObject behaviour, RunConfig runConfig) {
            animator = behaviour.GetComponent<Animator>();
            transform = behaviour.GetComponent<Transform>();
            rig = behaviour.GetComponent<Rigidbody2D>();
            config = runConfig;
        }
        
        public void AcceptMoveInput(InputAction.CallbackContext context) {
            moveDir = context.ReadValue<Single>();
            moving = Math.Abs(moveDir) > .01f;
            UpdateAnimations();
        }

        private void UpdateAnimations() {
            if (moving) {
                animator.ResetTrigger(idle);
                animator.SetTrigger(running);
                transform.localScale = moveDir > 0 ? Vector3.one : new Vector3(-1, 1, 1);
            }
            else {
                animator.ResetTrigger(running);
                animator.SetTrigger(idle);
            }
        }

        public float Force() {
            var rigX = rig.velocity.x;
            var capMoveSpeed = Mathf.Abs(rigX) >= config.maxVelocity;
            var positive = moveDir > 0 && rigX > 0;
            var negative = moveDir < 0 && rigX < 0;
            var isSameSign = positive || negative; 
            return capMoveSpeed && isSameSign ? 0 : moveDir * config.runVelocity;    
        }

        public void OnCollisionEnter2D(Collision2D other) => UpdateAnimations();
    }
}