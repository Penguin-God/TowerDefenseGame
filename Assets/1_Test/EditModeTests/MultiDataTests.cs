using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MultiDataTests
    {
        [Test]
        public void 서로_다른_객체를_생성해야_함()
        {
            var sut = new MultiData<TestClass>(() => new TestClass());

            Assert.AreNotSame(sut.GetData(PlayerIdManager.MasterId), sut.GetData(PlayerIdManager.ClientId));
        }

        [Test]
        public void ID에_알맞은_서비스를_제공해야_함()
        {
            var sut = new MultiData<TestClass>(() => new TestClass());

            sut.GetData(PlayerIdManager.ClientId).count++;

            Assert.AreEqual(0, sut.GetData(PlayerIdManager.MasterId).count);
            Assert.AreEqual(1, sut.GetData(PlayerIdManager.ClientId).count);
        }

        class TestClass
        {
            public int count;
        }
    }
}
