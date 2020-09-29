using System;
using Photon.Pun;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    internal class LockedFS : RunFS {
        public LockedFS(GameObject behaviour, RunConfig config, RunFSM runFsm) : base(behaviour, config,
            runFsm) { }

        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            // TODO - write MoveDir in RPC
            StateMachine.RaiseSetMovementDirEvent(context.ReadValue<Single>());
        }

        protected override void _OnCollisionEnter2D_RPC() { }

        protected override Vector2 _Force() => Vector2.zero;

        protected override void _AcceptLockInput() { }

        protected override void _AcceptUnlockInput() {
            var moving = Math.Abs(StateMachine.Values.moveDir) > .01f;

            if (!Behaviour.GetPhotonView().IsMine) {
                Debug.Log("Remote Player");
                Debug.Log("Unlocking input: moving = " + moving);
            }
            else {
                Debug.Log("Local Player");
                Debug.Log("Unlocking input: moving = " + moving);
            }
            
            StateMachine.RaiseChangeStateEvent(moving ? RunStates.Moving : RunStates.Idle);
        }
    }
}