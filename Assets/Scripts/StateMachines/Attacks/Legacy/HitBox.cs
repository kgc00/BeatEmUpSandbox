using UnityEngine;

namespace StateMachines.Attacks.Legacy {
    public class HitBox : MonoBehaviour {
        [SerializeField] private Attack parent;

        private void OnTriggerEnter2D(Collider2D other) {
            print($"COLLIDED: {other}");
        }
    }
}