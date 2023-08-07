using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UtilDomainTests
{
    public class RandomPositionCalculatorTests
    {
        [Test]
        public void 랜덤한_위치는_지정한_범위_이내에서_값을_반한해야_함()
        {
            // Arrange
            var worldPos = new Vector3(0, 0, 0);
            var spawnRange = 10f;
            var calculator = new RandomPositionCalculator();

            // Act
            var result = calculator.CalculateRandomPosInRange(worldPos, spawnRange);

            // Assert
            Assert.That(result.x, Is.InRange(-10f, 10f));
            Assert.That(result.z, Is.InRange(-10f, 10f));
        }
    }
}
