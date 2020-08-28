using UnityEngine;

namespace StateMachines.Interfaces {
    public interface IAcceptCollisionEnter {
        void OnCollisionEnter2D(Collision2D other);
    }
}