using System;
using Photon.Pun;
using StateMachines.Movement;
using StateMachines.Movement.Models;
using UnityEngine;

namespace StateMachines.Triggers {
    public class TriggerGround : MonoBehaviour {
        [SerializeField] MovementFSM fsm;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Board")) return;

            fsm.RaiseTouchingGroundEvent(true);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Board")) return;

            fsm.RaiseTouchingGroundEvent(false);
        }
    }
}