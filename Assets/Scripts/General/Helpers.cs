using System;
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