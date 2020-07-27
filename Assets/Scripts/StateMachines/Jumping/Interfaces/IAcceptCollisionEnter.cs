using UnityEngine;

namespace StateMachines.Jumping.Interfaces {
    public interface IAcceptCollisionEnter {
        void OnCollisionEnter2D(Collision2D other);
    }
}