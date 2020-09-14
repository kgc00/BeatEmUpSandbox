using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Interfaces;
using StateMachines.Movement.Horizontal.Run;
using StateMachines.Movement.Models;
using StateMachines.Network;
using StateMachines.Observer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpFSM : IProvideForce, IAcceptCollisionEnter, IAcceptJumpInput,
        IAcceptDashInput, IAcceptRunInput,
        IHandleLockedMovementInput, IHandleLockedJumpInput, IOnEventCallback {
        public JumpFS State { get; private set; }
        public JumpConfig Config { get; private set; }
        public GameObject Behaviour { get; private set; }

        public JumpFSM(GameObject behaviour, JumpConfig jumpConfig) {
            Config = jumpConfig;
            Behaviour = behaviour;

            InputLockObserver.LockMovementInput += AcceptLockMovementInput;
            InputLockObserver.UnlockMovementInput += AcceptUnlockMovementInput;

            InputLockObserver.LockJumpInput += AcceptLockJumpInput;
            InputLockObserver.UnlockJumpInput += AcceptUnlockJumpInput;

            PhotonNetwork.AddCallbackTarget(this);

            State = new JumpGroundedFS(behaviour, this, jumpConfig);
            State.Enter();
        }

        ~JumpFSM() {
            InputLockObserver.LockMovementInput -= AcceptLockMovementInput;
            InputLockObserver.UnlockMovementInput -= AcceptUnlockMovementInput;

            InputLockObserver.LockJumpInput -= AcceptLockJumpInput;
            InputLockObserver.UnlockJumpInput -= AcceptUnlockJumpInput;

            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void AcceptMoveInput(InputAction.CallbackContext context) => State.AcceptMoveInput(context);

        public void AcceptDashInput(InputAction.CallbackContext context) => State.AcceptDashInput(context);

        public void OnEvent(EventData photonEvent) {
            byte eventCode = photonEvent.Code;

            if (eventCode == ChangeJumpStateEvent.ChangeJumpStateEventCode) {
                if (Behaviour.GetPhotonView().IsMine) return;

                var data = (object[]) photonEvent.CustomData;
                var newState = (JumpStates) data[0];
                var moveDir = (float) data[1];
                var timeLapsed = (float) data[2];

                ChangeState(newState, moveDir, timeLapsed);
            }
        }

        public Vector2 Force() => State.Force();

        public void RaiseChangeStateEvent(JumpStates newState, float moveDir, float timeLapsed = 0f) {
            ChangeState(newState, moveDir, timeLapsed);
            ChangeJumpStateEvent.SendChangeJumpStateEvent(newState, moveDir, timeLapsed);
        }

        public void ChangeState(JumpStates newState, float moveDir, float timeLapsed = 0f) {
            if(!Behaviour.GetPhotonView().IsMine)
                Debug.Log($"ChangeState to {newState}");
            State.Exit();
            State = StateFactory.JumpFSFromEnum(newState, this, moveDir, timeLapsed);
            State.Enter();
        }

        public void Update() => State.Update();
        public void FixedUpdate() => State.FixedUpdate();
        public void OnCollisionEnter2D_RPC() => State.OnCollisionEnter2D_RPC();
        public void AcceptJumpInput(InputAction.CallbackContext context) => State.AcceptJumpInput(context);
        public void AcceptLockMovementInput() => State.AcceptLockJumpInput();
        public void AcceptUnlockMovementInput() => State.AcceptUnlockJumpInput();
        public void AcceptLockJumpInput() => State.AcceptLockJumpInput();
        public void AcceptUnlockJumpInput() => State.AcceptUnlockJumpInput();
    }
}