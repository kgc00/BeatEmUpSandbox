using StateMachines.Attacks.Legacy;
using StateMachines.Interfaces;
using StateMachines.Observer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    

    public class RunFSM : IAcceptRunInput, IProvideForce, IAcceptCollisionEnter, IChangeState<RunFS>, IHandleLockedMovementInput {
        private readonly Animator animator;
        private readonly Transform transform;
        private readonly RunConfig config;
        private readonly Rigidbody2D rig;

        private readonly int running = Animator.StringToHash("Running");
        private readonly int idle = Animator.StringToHash("Idle");

        private float moveDir;
        private bool moving;

        public RunFS State { get; private set; }

        public RunFSM(GameObject behaviour, RunConfig runConfig) {
            animator = behaviour.GetComponent<Animator>();
            transform = behaviour.GetComponent<Transform>();
            rig = behaviour.GetComponent<Rigidbody2D>();
            config = runConfig;
            State = new IdleFS(behaviour, runConfig, this);
            InputLockObserver.LockMovementInput += AcceptLockMovementInput;
            InputLockObserver.UnlockMovementInput += AcceptUnlockMovementInput;
        }

        ~RunFSM() {
            InputLockObserver.LockMovementInput -= AcceptLockMovementInput;
            InputLockObserver.UnlockMovementInput -= AcceptUnlockMovementInput;
        }
        
        public void ChangeState(RunFS newState) {
            State.Exit();
            State = newState;
            State.Enter();
        }
        public void AcceptMoveInput(InputAction.CallbackContext context) => State.AcceptMoveInput(context);
        public float Force() => State.Force();
        public void OnCollisionEnter2D(Collision2D other) => State.OnCollisionEnter2D(other);
        public void AcceptLockMovementInput() => State.AcceptLockMovementInput();
        public void AcceptUnlockMovementInput() => State.AcceptUnlockMovementInput();
    }
}