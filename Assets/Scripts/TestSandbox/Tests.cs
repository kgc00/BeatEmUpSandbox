using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Tests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestsSimplePasses() {
            int jumps = 2;
            Assert.AreEqual(jumps,2);
            Mathf.Clamp(jumps--, 0, 2);
            Assert.AreEqual(jumps,1);
            Mathf.Clamp(jumps--, 0, 2);
            Assert.AreEqual(jumps,0);
        }
    }
}
