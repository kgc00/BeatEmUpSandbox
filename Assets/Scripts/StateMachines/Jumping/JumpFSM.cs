using System;
using UnityEngine;

namespace StateMachines.Jumping {
    public class JumpLaunchFSM : MonoBehaviour {
        private FSMState state;

        private void Start() {
            state = new JumpGroundedFS();
        }
    }
}