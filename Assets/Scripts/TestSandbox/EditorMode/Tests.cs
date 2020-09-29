using NUnit.Framework;
using UnityEngine;

namespace TestSandbox.EditorMode
{
    public class Tests
    {
        
        [Test]
        public void DoesClamp0() {
            int jumps = 2;
            Assert.AreEqual(jumps,2);
            Mathf.Clamp(jumps--, 0, 2);
            Assert.AreEqual(jumps,1);
            Mathf.Clamp(jumps--, 0, 2);
            Assert.AreEqual(jumps,0);
            
            jumps = Mathf.Clamp(jumps - 1, 0, 2);
            Assert.AreEqual(jumps,0);
        }
        
        [Test]
        public void DipsBelow0() {
            int jumps = 0;
            Mathf.Clamp(jumps--, 0, 2);
            Assert.AreEqual(jumps,-1);
        }

    }
}
