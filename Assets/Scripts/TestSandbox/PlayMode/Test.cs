using System.Collections;
using NUnit.Framework;
using StateMachines.Movement;
using StateMachines.Movement.Horizontal.Run;
using StateMachines.Movement.Models;
using StateMachines.Observer;
using StateMachines.State;
using UnityEngine;
using UnityEngine.TestTools;

namespace TestSandbox.PlayMode {
    public class Test {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestInputLock() {
            var go1 = new GameObject("obj 1");
                go1.AddComponent<Animator>();
            var go2 = new GameObject("obj 2");
                go2.AddComponent<Animator>();
                
            var run1 = new RunFSM(go1, ScriptableObject.CreateInstance<RunConfig>(), new UnitMovementData());
            var run2 = new RunFSM(go2, ScriptableObject.CreateInstance<RunConfig>(), new UnitMovementData());

            yield return null;
            Assert.IsNotNull(go1);
            Assert.AreEqual(run1.State.ToString(), "StateMachines.Movement.Horizontal.Run.IdleFS");
            
            InputLockObserver.LockMovementInput(go1);
            yield return null;
            Assert.AreEqual(run1.State.ToString(), "StateMachines.Movement.Horizontal.Run.LockedFS");
            Assert.AreEqual(run2.State.ToString(), "StateMachines.Movement.Horizontal.Run.IdleFS");
        }

        [UnityTest]
        public IEnumerator TestEquality() {
            var go1 = new GameObject("obj 1");
            var go2 = new GameObject("obj 2");
            yield return null;
            Assert.IsFalse(ReferenceEquals(go1, go2));
        }
    }
}