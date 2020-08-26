using UnityEngine;

namespace StateMachines.Movement.Interfaces {
    public interface IAcceptCollisionEnter {
        void OnCollisionEnter2D(Collision2D other);
    }
}