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

        public RunFSM(GameObject behaviour, RunConfig runConfig, MovementValues movementValues) {
            Behaviour = behaviour;
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


        public void RaiseSetMovementDirEvent(float moveDir) {
            SetMoveDir(moveDir);
            SetMovementDirEvent.SendSetMovementDirEvent(moveDir);
        }

        private void SetMoveDir(float moveDir) => Values.moveDir = moveDir;


        public void OnEvent(EventData photonEvent) {
            byte eventCode = photonEvent.Code;

            if (eventCode == NetworkedEventCodes.SetMovementDirEventCode) {
                if (Behaviour.GetPhotonView().IsMine) return;

                var dir = (float) photonEvent.CustomData;
                SetMoveDir(dir);
            }

            else if (eventCode == NetworkedEventCodes.ChangeRunStateEventCode) {
                if (Behaviour.GetPhotonView().IsMine) return;

                var data = (object[]) photonEvent.CustomData;
                var newState = (RunStates) data[0];

                ChangeState(newState);
            }
        }

        public void RaiseChangeStateEvent(RunStates newState) {
            ChangeState(newState);
            ChangeRunStateEvent.SendChangeRunStateEvent(newState);
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
    }
}