using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace General {
    public class ForceComponent : MonoBehaviour {
        [SerializeField] private List<ForceStrategy> strategies;
        private bool ShouldActivate(Collider2D other, [CanBeNull] out Rigidbody2D rigidBody) {
            rigidBody = null;
            rigidBody = other.transform.root.GetComponent<Rigidbody2D>();
            if (rigidBody == null) {
                Debug.Log($"Unable to affect {gameObject.name} because they do not posses a rigidbody");
                return false;
            }

            return true;
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!ShouldActivate(other, out var rigidBody)) return;
            foreach (var strategy in strategies) {
                StartCoroutine(strategy.Execute(other, rigidBody, transform));
            }
        }
    }
}