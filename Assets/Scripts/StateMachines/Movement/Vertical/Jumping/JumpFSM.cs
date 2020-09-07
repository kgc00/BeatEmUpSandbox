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

        public void OnEvent(EventData photonEvent) {
            byte eventCode = photonEvent.Code;

            if (eventCode == ChangeJumpStateEvent.ChangeJumpStateEventCode) {
                if (Behaviour.GetPhotonView().IsMine) return;

                var data = (object[]) photonEvent.CustomData;
                var newState = (JumpStates) data[0];
                var timeLapsed = (float) data[1];

                ChangeState(newState, timeLapsed);
            }
        }

        public float Force() => State.Force();

        public void RaiseChangeStateEvent(JumpStates newState, float timeLapsed = 0f) {
            ChangeState(newState, timeLapsed);
            ChangeJumpStateEvent.SendChangeJumpStateEvent(newState, timeLapsed);
        }

        public void ChangeState(JumpStates newState, float timeLapsed = 0f) {
            State.Exit();
            State = StateFactory.JumpFSFromEnum(newState, this, timeLapsed);
            State.Enter();
        }

        public void Update() => State.Update();

        public void OnCollisionEnter2D_RPC() => State.OnCollisionEnter2D_RPC();
        public void AcceptJumpInput(InputAction.CallbackContext context) => State.AcceptJumpInput(context);


        public void AcceptLockMovementInput() => State.AcceptLockJumpInput();
        public void AcceptUnlockMovementInput() => State.AcceptUnlockJumpInput();
        public void AcceptLockJumpInput() => State.AcceptLockJumpInput();

        public void AcceptUnlockJumpInput() => State.AcceptUnlockJumpInput();
    }
}