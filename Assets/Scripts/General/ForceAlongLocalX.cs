using System;
using System.Collections;
using UnityEngine;

namespace General {
    [Serializable, CreateAssetMenu(fileName = "ForceAlongLocalX", menuName = "Actions/ForceAlongLocalX", order = 0)]
    public class ForceAlongLocalX : ForceStrategy {
        [SerializeField, Range(-5, 0)] private float force;
        public override IEnumerator Execute(Collider2D other, Rigidbody2D rigidBody,
            Transform forceComponentTransform) {
            var trnsWithOffset = forceComponentTransform.position;
            trnsWithOffset.x += 0.5f * forceComponentTransform.root.localScale.x;

            Vector3 left = forceComponentTransform.TransformDirection(Vector3.left);
            Vector3 heading = other.transform.position - trnsWithOffset;
            heading.z = 0f;

            // dot scales the value of force to make it stronger as the unit
            // is further away. The unit will always end near center of bounds
            var dot = Vector3.Dot(left, heading.normalized);

            // Force is required to be a negative value because we are pulling
            var scaledForce = force * dot;
            var appliedForce = left * scaledForce;

            // Apply force over several frames for a smoother acceleration
            var frames = 10;
            for (int j = 0; j < frames; j++) {
                if (rigidBody == null) yield break;

                rigidBody.AddForce(appliedForce);
                yield return null;
            }
        }
    }
}