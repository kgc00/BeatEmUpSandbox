using System;
using StateMachines.Jumping.Interfaces;
using StateMachines.Movement.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement {
    public class Run : IAcceptRunInput, IProvideForce{
        private GameObject behaviour;
        private Animator animator;
        private Transform transform;
        private readonly int running = Animator.StringToHash("Running");
        private readonly int idle = Animator.StringToHash("Idle");
        private RunConfig config;
        private Rigidbody2D rig;
        private float moveDir;

        public Run(GameObject behaviour, RunConfig runConfig) {
            this.behaviour = behaviour;
            animator = behaviour.GetComponent<Animator>();
            transform = behaviour.GetComponent<Transform>();
            rig = behaviour.GetComponent<Rigidbody2D>();
            config = runConfig;
        }
        
        public void AcceptMoveInput(InputAction.CallbackContext context) {
            moveDir = context.ReadValue<Single>();

            var moving = Math.Abs(moveDir) > .01f;
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
        
        public float Force() =>Mathf.Abs(rig.velocity.x) >= config.maxVelocity ? 0 : moveDir * config.runVelocity;
    }
}