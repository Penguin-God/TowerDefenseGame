using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UnitStatChangeTests
    {
        [UnityTest]
        public IEnumerator UnitStatChangeTestsWithEnumeratorPasses()
        {
            Multi_SpawnManagers.NormalUnit.Spawn(new UnitFlags(0, 0));
            yield return null;

            yield return null;
            Assert.AreEqual(1, 1);
        }
    }
}
