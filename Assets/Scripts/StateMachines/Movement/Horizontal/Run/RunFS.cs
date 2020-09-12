using StateMachines.Interfaces;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    public abstract class RunFS : FSMState<RunFS>, IProvideForce, IAcceptRunInput, IAcceptCollisionEnter, IHandleLockedRunInput {
        protected readonly Animator Animator;
        protected readonly Transform Transform;
        protected readonly RunConfig Config;
        protected readonly Rigidbody2D Rig;

        protected readonly int Running = Animator.StringToHash("Running");
        protected readonly int Idle = Animator.StringToHash("Idle");

        protected float MoveDir;
        protected readonly RunFSM StateMachine;
        protected readonly GameObject Behaviour;
        protected RunFS(GameObject behaviour, RunConfig runConfig, RunFSM runFsm, float dir) {
            StateMachine = runFsm;
            Animator = behaviour.GetComponent<Animator>();
            Transform = behaviour.GetComponent<Transform>();
            Rig = behaviour.GetComponent<Rigidbody2D>();
            Behaviour = behaviour;
            Config = runConfig;
            MoveDir = dir;
        }


        protected bool IsJumpState() =>
            Animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump") ||
            Animator.GetCurrentAnimatorStateInfo(0).IsTag("Fall");
        
        public void AcceptMoveInput(InputAction.CallbackContext context) => _AcceptMoveInput(context);

        protected abstract void _AcceptMoveInput(InputAction.CallbackContext context);
        public void OnCollisionEnter2D_RPC() => _OnCollisionEnter2D_RPC();

        protected abstract void _OnCollisionEnter2D_RPC();

        public float Force() => _Force();

        protected abstract float _Force();
        public void AcceptLockRunInput() => _AcceptLockInput();

        protected virtual void _AcceptLockInput() => StateMachine.RaiseChangeStateEvent(RunStates.Locked, MoveDir);

        public void AcceptUnlockRunInput() => _AcceptUnlockInput();

        protected abstract void _AcceptUnlockInput();
        protected virtual void UpdateAnimations() {}
        
        public virtual void AcceptDashInput(InputAction.CallbackContext context) { }
    }
}