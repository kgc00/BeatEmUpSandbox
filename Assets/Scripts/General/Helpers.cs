using System;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

namespace General {
    public static class Helpers {
        public static float GenerateTimeStamp() => UnityEngine.Time.time;

        public static string GetUniqueStateName(string fullyQualifiedStateName) {
            try {
                var spl = fullyQualifiedStateName.Split('.');
                return spl[spl.Length - 1];
            }
            catch (Exception ex) {
                Debug.Log("Caught some error: " + ex);
                return "";
            }
        }
        
        public static void DampenXVelocity(Rigidbody2D rig) {
            var vel = rig.velocity;
            vel.x /= 4;

            rig.velocity = vel;
        }
        
        public static void RemoveXVelocity(Rigidbody2D rig) {
            /* remove horizontal velocity for case of
             *  exiting dash state.
            */

            var vel = rig.velocity;
            vel.x = 0;

            rig.velocity = vel;
        }
        
        public static void RemovePositiveYVelocity(Rigidbody2D rig) {
            var vel = rig.velocity;
            vel.y = vel.y > 0 ? 0 : vel.y;

            rig.velocity = vel;
        }
        
        public static void AddForceY(Rigidbody2D rig, float force) {
            rig.AddForce(new Vector2(0,force));
        }
        
        public static void RemoveYVelocity(Rigidbody2D rig) {
            /* remove downward velocity for case of
             *  doing a double jump while falling at a great speed.
             *  if we didn't do this, the jump would not raise the player up
             * (all the negative y velocity would eat the movement)
            */

            var vel = rig.velocity;
            vel.y = 0;

            rig.velocity = vel;
        }

        public static void AddForceX(Rigidbody2D rig, int force) {            
            rig.AddForce(new Vector2(force,0));
        }

        [CanBeNull]
        public static GameObject GameObjectFromId(int photonViewId) {
            var other = PhotonView.Find(photonViewId);

            if (other == null) {
                Debug.LogError("Unable to find object with id " + photonViewId);
                return null;
            }

            return other.gameObject;
        }
        
        public static void LogInfo(Animator animator) {
            Debug.Log(animator.GetNextAnimatorClipInfo(0));
            Debug.Log(animator.GetNextAnimatorClipInfo(0)[0].clip);
            Debug.Log(animator.GetNextAnimatorClipInfo(0)[0].clip.name);

            Debug.Log("CURRENT STATE INFO --- ");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
            Debug.Log("Logging Attack1");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack1"));
            Debug.Log("Logging Attack2");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack2"));
            Debug.Log("Logging Idle");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
            Debug.Log("Logging Jump");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"));
            Debug.Log("Logging Run");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Run"));

            Debug.Log("NEXT STATE INFO --- ");
            Debug.Log(animator.GetNextAnimatorStateInfo(0));
            Debug.Log("Logging Attack1");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Attack1"));
            Debug.Log("Logging Attack2");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Attack2"));
            Debug.Log("Logging Idle");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Idle"));
            Debug.Log("Logging Jump");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Jump"));
            Debug.Log("Logging Run");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Run"));
        }
    }
}