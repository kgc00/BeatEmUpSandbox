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
        public MovementValues Values { get; private set; }
        public GameObject Behaviour { get; private set; }
        public int ViewId { get; private set; }

        public JumpFSM(GameObject behaviour, JumpConfig jumpConfig, MovementValues values) {
            Config = jumpConfig;
            Values = values;
            Behaviour = behaviour;
            ViewId = behaviour.GetPhotonView().ViewID;
            
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

            if (eventCode == NetworkedEventCodes.ChangeJumpStateEventCode) {
                var data = (object[]) photonEvent.CustomData;
                
                if ((int) data[1] != ViewId) return;
                
                var newState = (JumpStates) data[0];

                ChangeState(newState);
            }
            
            
            if (eventCode == NetworkedEventCodes.SetMovementDirEventCode) {
                var data = (object[]) photonEvent.CustomData;
                
                if ((int) data[1] != ViewId) return;

                var dir = (float) data[0];
                SetMoveDir(dir);
            }
        }
        public void RaiseSetMoveDirEvent(float moveDir, int viewId) {
            SetMoveDir(moveDir);
            SetMovementDirEvent.SendSetMovementDirEvent(moveDir, viewId);
        }
        
        public void SetMoveDir(float moveDir) => Values.moveDir = moveDir;

        public Vector2 Force() => State.Force();

        public void RaiseChangeStateEvent(JumpStates newState) {
            ChangeState(newState);
            ChangeJumpStateEvent.SendChangeJumpStateEvent(newState, ViewId);
        }

        public void ChangeState(JumpStates newState) {
            State.Exit();
            State = StateFactory.JumpFSFromEnum(newState, this);
            State.Enter();
        }

        public void Update() => State.Update();
        public void FixedUpdate() => State.FixedUpdate();
        public void OnCollisionEnter2D_RPC() => State.OnCollisionEnter2D_RPC();
        public void AcceptJumpInput(InputAction.CallbackContext context) => State.AcceptJumpInput(context);

        public void AcceptLockMovementInput(object sender) {
            if (!ReferenceEquals(sender, Behaviour)) return;

            State.AcceptLockJumpInput(sender);
        }

        public void AcceptUnlockMovementInput(object sender) {
            if (!ReferenceEquals(sender, Behaviour)) return;

            State.AcceptUnlockJumpInput(sender);
        }

        public void AcceptLockJumpInput(object sender) {
            if (!ReferenceEquals(sender, Behaviour)) return;

            State.AcceptLockJumpInput(sender);
        }

        public void AcceptUnlockJumpInput(object sender) {
            if (!ReferenceEquals(sender, Behaviour)) return;

            State.AcceptUnlockJumpInput(sender);
        }
    }
}