using StateMachines.Movement.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    public abstract class RunFS : FSMState<RunFS>, IProvideForce, IAcceptRunInput, IAcceptCollisionEnter {
        protected readonly Animator Animator;
        protected readonly Transform Transform;
        protected readonly RunConfig Config;
        protected readonly Rigidbody2D Rig;

        protected readonly int Running = Animator.StringToHash("Running");
        protected readonly int Idle = Animator.StringToHash("Idle");

        protected float MoveDir;
        protected readonly RunFSM StateMachine;
        protected readonly GameObject Behaviour;

        protected bool inputLocked;
        protected RunFS(GameObject behaviour, RunConfig runConfig, RunFSM runFsm, float dir) {
            StateMachine = runFsm;
            Animator = behaviour.GetComponent<Animator>();
            Transform = behaviour.GetComponent<Transform>();
            Rig = behaviour.GetComponent<Rigidbody2D>();
            Behaviour = behaviour;
            Config = runConfig;
            MoveDir = dir;
        }


        public void AcceptMoveInput(InputAction.CallbackContext context) {
            _AcceptMoveInput(context);
        }

        protected abstract void _AcceptMoveInput(InputAction.CallbackContext context);
        public void OnCollisionEnter2D(Collision2D other) {
            _OnCollisionEnter2D(other);
        }

        protected abstract void _OnCollisionEnter2D(Collision2D other);

        public float Force() {
            return _Force();
        }

        protected abstract float _Force();
    }
}