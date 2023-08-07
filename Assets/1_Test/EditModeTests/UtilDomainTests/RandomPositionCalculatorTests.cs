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
        public void ������_��ġ��_������_����_�̳�����_����_�����ؾ�_��()
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
