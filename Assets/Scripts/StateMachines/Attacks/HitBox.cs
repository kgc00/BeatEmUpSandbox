using System;
using UnityEngine;

namespace StateMachines.Attacks {
    public class HitBox : MonoBehaviour {
        [SerializeField] private Attack parent;

        private void OnTriggerEnter2D(Collider2D other) {
            print($"COLLIDED: {other}");
        }
    }
}