using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UtilDomainTests
{

    public class PoolTests
    {
        [Test]
        public void Ǯ_����_�׽�Ʈ()
        {
            var sut = new ObjectPool("A/B/C", 0, null);
            Assert.AreEqual("C", sut.ObjectName);
        }
    }
}
