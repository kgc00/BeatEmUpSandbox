using System;
using StateMachines.Attacks.Legacy;
using UnityEngine;

namespace StateMachines.Attacks {
    public class HitBox : MonoBehaviour {
        [SerializeField] private AttackFSM parent;

        private void OnTriggerEnter2D(Collider2D other) {
            parent.AttackConnected(other);
        }
    }
}