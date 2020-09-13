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

        public RunFSM(GameObject behaviour, RunConfig runConfig) {
            Behaviour = behaviour;
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
        
        public void OnEvent(EventData photonEvent) {
            byte eventCode = photonEvent.Code;

            // if (eventCode == RunCollisionEvent.RunCollisionEventCode) {
            //     if (Behaviour.GetPhotonView().IsMine) return;
            //     
            //     var other = (Collision2D) photonEvent.CustomData;
            //     OnCollisionEnter2D(other);
            // }
            
            if (eventCode == ChangeRunStateEvent.ChangeRunStateEventCode) {
                if (Behaviour.GetPhotonView().IsMine) return;

                var data = (object[]) photonEvent.CustomData;
                var newState = (RunStates) data[0];
                var moveDir = (float) data[1];
                
                ChangeState(newState, moveDir);
            }
        }
        
        public void RaiseChangeStateEvent(RunStates newState, float moveDir = 0f) {
            ChangeState(newState, moveDir);
            ChangeRunStateEvent.SendChangeRunStateEvent(newState, moveDir);
        }

        public void ChangeState(RunStates newState, float moveDir = 0f) {
            State.Exit();
            State = StateFactory.RunFSFromEnum(newState, this, moveDir);
            State.Enter();
        }

        public void AcceptMoveInput(InputAction.CallbackContext context) => State.AcceptMoveInput(context);
        public Vector2 Force() => State.Force();
        public void AcceptDashInput(InputAction.CallbackContext context) => State.AcceptDashInput(context);
        public void OnCollisionEnter2D_RPC() => State.OnCollisionEnter2D_RPC();
        public void AcceptLockRunInput() => State.AcceptLockRunInput();
        public void AcceptUnlockRunInput() => State.AcceptUnlockRunInput();
        public void AcceptLockMovementInput() => State.AcceptLockRunInput();
        public void AcceptUnlockMovementInput() => State.AcceptUnlockRunInput();

        public void Update() => State.Update();

        public void FixedUpdate() => State.FixedUpdate();
    }
}