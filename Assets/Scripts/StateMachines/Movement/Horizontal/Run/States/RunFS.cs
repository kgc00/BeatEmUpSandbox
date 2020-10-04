using Photon.Pun;
using StateMachines.Interfaces;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run.States {
    public abstract class RunFS : FSMState<RunFS>, IProvideForce, IAcceptRunInput, IAcceptCollisionEnter, IHandleLockedRunInput {
        protected readonly Animator Animator;
        protected readonly Transform Transform;
        protected readonly RunConfig Config;
        protected readonly Rigidbody2D Rig;

        protected readonly int Running = Animator.StringToHash("Running");
        protected readonly int Idle = Animator.StringToHash("Idle");

        protected readonly RunFSM StateMachine;
        protected readonly GameObject Behaviour;
        
        protected readonly int ViewId;
        protected RunFS(GameObject behaviour, RunConfig runConfig, RunFSM runFsm) {
            StateMachine = runFsm;
            Animator = behaviour.GetComponent<Animator>();
            Transform = behaviour.GetComponent<Transform>();
            Rig = behaviour.GetComponent<Rigidbody2D>();
            Behaviour = behaviour;
            ViewId = behaviour.GetPhotonView().ViewID;
            Config = runConfig;
        }


        protected bool IsJumpState() =>
            Animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump") ||
            Animator.GetCurrentAnimatorStateInfo(0).IsTag("DoubleJump") ||
            Animator.GetCurrentAnimatorStateInfo(0).IsTag("Fall");
        
        public void AcceptMoveInput(InputAction.CallbackContext context) => _AcceptMoveInput(context);

        protected abstract void _AcceptMoveInput(InputAction.CallbackContext context);
        public void OnCollisionEnter2D_RPC() => _OnCollisionEnter2D_RPC();

        protected abstract void _OnCollisionEnter2D_RPC();

        public Vector2 Force() => _Force();

        protected abstract Vector2 _Force();
        public void AcceptLockRunInput(object sender) => _AcceptLockInput();

        protected virtual void _AcceptLockInput() => StateMachine.RaiseChangeRunStateEvent(RunStates.Locked, ViewId);

        public void AcceptUnlockRunInput(object sender) => _AcceptUnlockInput();

        protected abstract void _AcceptUnlockInput();
        protected virtual void UpdateAnimations() {}
        
        public virtual void AcceptDashInput(InputAction.CallbackContext context) { }
    }
}