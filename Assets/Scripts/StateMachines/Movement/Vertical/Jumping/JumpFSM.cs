using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Interfaces;
using StateMachines.Movement.Horizontal.Run;
using StateMachines.Observer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpFSM : IProvideForce, IAcceptCollisionEnter, IAcceptJumpInput, IChangeState<JumpFS>,
        IHandleLockedMovementInput, IHandleLockedJumpInput, IOnEventCallback {
        public JumpFS State { get; private set; }

        public JumpFSM(GameObject behaviour, JumpConfig jumpConfig) {
            State = new JumpGroundedFS(behaviour, this, jumpConfig);
            InputLockObserver.LockMovementInput += AcceptLockMovementInput;
            InputLockObserver.UnlockMovementInput += AcceptUnlockMovementInput;
            
            InputLockObserver.LockJumpInput += AcceptLockJumpInput;
            InputLockObserver.UnlockJumpInput += AcceptUnlockJumpInput;
            PhotonNetwork.AddCallbackTarget(this);
        }

        ~JumpFSM() {
            InputLockObserver.LockMovementInput -= AcceptLockMovementInput;
            InputLockObserver.UnlockMovementInput -= AcceptUnlockMovementInput;
            
            InputLockObserver.LockJumpInput -= AcceptLockJumpInput;
            InputLockObserver.UnlockJumpInput -= AcceptUnlockJumpInput;
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent) {
            throw new System.NotImplementedException();
        }

        public float Force() => State.Force();

        public void ChangeState(JumpFS newState) {
            State.Exit();
            State = newState;
            State.Enter();
        }

        public void Update() => State.Update();

        public void OnCollisionEnter2D(Collision2D other) => State.OnCollisionEnter2D(other);
        public void AcceptJumpInput(InputAction.CallbackContext context) => State.AcceptJumpInput(context);


        public void AcceptLockMovementInput() => State.AcceptLockJumpInput();
        public void AcceptUnlockMovementInput() => State.AcceptUnlockJumpInput();
        public void AcceptLockJumpInput() => State.AcceptLockJumpInput();

        public void AcceptUnlockJumpInput() => State.AcceptUnlockJumpInput();
    }
}