using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Attacks.Legacy;
using StateMachines.Interfaces;
using StateMachines.Movement.Models;
using StateMachines.Network;
using StateMachines.Observer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    public class RunFSM : IAcceptRunInput, IProvideForce, IAcceptCollisionEnter,
        IHandleLockedMovementInput, IHandleLockedRunInput, IOnEventCallback, IAcceptDashInput {
        private readonly Animator animator;
        private readonly Transform transform;
        private readonly Rigidbody2D rig;

        public RunFS State { get; private set; }
        public GameObject Behaviour { get; }
        public RunConfig Config { get; }
        public MovementValues Values { get; private set; }
        public int ViewId { get; private set; }
        public RunFSM(GameObject behaviour, RunConfig runConfig, MovementValues movementValues) {
            Behaviour = behaviour;
            ViewId = Behaviour.GetPhotonView().ViewID;
            Values = movementValues;
            Config = runConfig;
            animator = behaviour.GetComponent<Animator>();
            transform = behaviour.GetComponent<Transform>();
            rig = behaviour.GetComponent<Rigidbody2D>();

            InputLockObserver.LockRunInput += AcceptLockRunInput;
            InputLockObserver.UnlockRunInput += AcceptUnlockRunInput;

            InputLockObserver.LockMovementInput += AcceptLockRunInput;
            InputLockObserver.UnlockMovementInput += AcceptUnlockRunInput;

            PhotonNetwork.AddCallbackTarget(this);

            State = new IdleFS(behaviour, runConfig, this);
            State.Enter();
        }

        ~RunFSM() {
            InputLockObserver.LockRunInput -= AcceptLockRunInput;
            InputLockObserver.UnlockRunInput -= AcceptUnlockRunInput;

            InputLockObserver.LockMovementInput -= AcceptLockRunInput;
            InputLockObserver.UnlockMovementInput -= AcceptUnlockRunInput;

            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void SetMoveDir(float moveDir) => Values.moveDir = moveDir;


        public void OnEvent(EventData photonEvent) {
            byte eventCode = photonEvent.Code;

            if (eventCode == NetworkedEventCodes.SetMovementDirEventCode) {
                var data = (object[]) photonEvent.CustomData;
                
                if ((int) data[1] != ViewId) return;

                var dir = (float) data[0];
                SetMoveDir(dir);
            }

            else if (eventCode == NetworkedEventCodes.ChangeRunStateEventCode) {
                var data = (object[]) photonEvent.CustomData;
                
                if ((int) data[1] != ViewId) return;

                var newState = (RunStates) data[0];
                ChangeState(newState);
            }
        }

        public void ChangeState(RunStates newState) {
            State.Exit();
            State = StateFactory.RunFSFromEnum(newState, this);
            State.Enter();
        }

        public void AcceptMoveInput(InputAction.CallbackContext context) => State.AcceptMoveInput(context);
        public Vector2 Force() => State.Force();
        public void AcceptDashInput(InputAction.CallbackContext context) => State.AcceptDashInput(context);
        public void OnCollisionEnter2D_RPC() => State.OnCollisionEnter2D_RPC();

        public void AcceptLockRunInput(object sender) {
            if (!ReferenceEquals(sender, Behaviour)) return;

            State.AcceptLockRunInput(sender);
        }

        public void AcceptUnlockRunInput(object sender) {
            if (!ReferenceEquals(sender, Behaviour)) return;

            State.AcceptUnlockRunInput(sender);
        }

        public void AcceptLockMovementInput(object sender) {
            if (!ReferenceEquals(sender, Behaviour)) return;

            State.AcceptLockRunInput(sender);
        }

        public void AcceptUnlockMovementInput(object sender) {
            if (!ReferenceEquals(sender, Behaviour)) return;

            State.AcceptUnlockRunInput(sender);
        }

        public void Update() => State.Update();

        public void FixedUpdate() => State.FixedUpdate();

        public void RaiseSetMoveDirEvent(float moveDir, int viewId) {
            SetMoveDir(moveDir);
            SetMovementDirEvent.SendSetMovementDirEvent(moveDir, viewId);
        }

        public void RaiseChangeRunStateEvent(RunStates newState, int viewId) {
            ChangeState(newState);
            ChangeRunStateEvent.SendChangeRunStateEvent(newState, viewId);
        }
    }
}