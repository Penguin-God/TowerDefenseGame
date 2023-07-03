using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnitDomainTests
{
    public class ChaseTests
    {
        const float Range = 20f;
        UnitChaseUseCase CreateSut() => new UnitChaseUseCase(Range);

        [Test]
        [TestCase(19, ChaseState.Far)]
        [TestCase(14, ChaseState.Close)]
        [TestCase(3, ChaseState.Contact)]
        public void 거리에_따라_적절한_상태를_반환해야_함(float z, ChaseState expected)
        {
            var sut = CreateSut();

            var result = sut.CalculateChaseState(Vector3.zero, new Vector3(0, 0, z), Vector3.up, Vector3.up);

            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(18, -1f, ChaseState.FaceToFace)]
        [TestCase(25, -1f, ChaseState.Far)]
        [TestCase(14, 1f, ChaseState.Close)]
        public void 거리와_방향에_따라_적절한_상태를_반환해야_함(float z, float dirZ, ChaseState expected)
        {
            var sut = CreateSut();

            var result = sut.CalculateChaseState(Vector3.zero, new Vector3(0, 0, z), Vector3.forward, new Vector3(0, 0, dirZ));

            Assert.AreEqual(expected, result);
        }
    }
}
