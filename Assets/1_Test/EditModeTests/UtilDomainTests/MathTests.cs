using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UtilDomainTests
{
    public class MathTests
    {
        [Test]
        [TestCase(100, 30, 30)]
        [TestCase(100, 4.5f, 4.5f)]
        [TestCase(100, 0, 0)]
        [TestCase(0, 5, 0)]
        public void ������_�ۼ�Ʈ_�Ҽ�_����_��ȯ�ؾ�_��(float total, float percent, float expected)
        {
            Assert.That(expected, Is.EqualTo(MathUtil.CalculatePercentage(total, percent)).Within(0.01f));
        }

        [Test]
        [TestCase(100, 30, 30)]
        [TestCase(100, 4.8f, 5)]
        [TestCase(100, 0, 0)]
        [TestCase(0, 5, 0)]
        public void ������_�ۼ�Ʈ_����_����_��ȯ�ؾ�_��(int total, float percent, int expected)
        {
            Assert.AreEqual(expected, MathUtil.CalculatePercentage(total, percent));
        }

        [Test]
        public void ����_8��_���_��_����_��������_������_���ݸ�ŭ_��������_��()
        {
            // Arrange
            int numberOfDirections = 8;
            float delta = 0.01f; // ��� ������ ����

            Vector2[] expectedDirections =
            {
            new Vector2(1f, 0f), // 0��
            new Vector2(0.7071f, 0.7071f), // 45��
            new Vector2(0f, 1f), // 90��
            new Vector2(-0.7071f, 0.7071f), // 135��
            new Vector2(-1f, 0f), // 180��
            new Vector2(-0.7071f, -0.7071f), // 225��
            new Vector2(0f, -1f), // 270��
            new Vector2(0.7071f, -0.7071f) // 315��
        };

            // Act
            var actualDirections = MathUtil.CalculateDirections(numberOfDirections);

            // Assert
            for (int i = 0; i < numberOfDirections; i++)
            {
                Assert.That(expectedDirections[i].x, Is.EqualTo(actualDirections[i].x).Within(delta));
                Assert.That(expectedDirections[i].y, Is.EqualTo(actualDirections[i].y).Within(delta));
            }
        }

        [Test]
        public void ��ġ_8��_���_��_�߽ɿ���_��������ŭ_��������_��ġ����_������_���ݸ�ŭ_��������_��()
        {
            // Arrange
            int numberOfPositions = 8;
            float distanceFromCenter = 1f; // ���� ���� �������� ��
            float delta = 0.01f; // ��� ������ ����

            Vector2[] expectedPositions =
            {
            new Vector2(1f, 0f), // 0��
            new Vector2(0.7071f, 0.7071f), // 45��
            new Vector2(0f, 1f), // 90��
            new Vector2(-0.7071f, 0.7071f), // 135��
            new Vector2(-1f, 0f), // 180��
            new Vector2(-0.7071f, -0.7071f), // 225��
            new Vector2(0f, -1f), // 270��
            new Vector2(0.7071f, -0.7071f) // 315��
        };

            // Act
            var actualPositions = MathUtil.CalculateCirclePositions(numberOfPositions, distanceFromCenter);

            // Assert
            for (int i = 0; i < numberOfPositions; i++)
            {
                Assert.That(expectedPositions[i].x, Is.EqualTo(actualPositions[i].x).Within(delta));
                Assert.That(expectedPositions[i].y, Is.EqualTo(actualPositions[i].y).Within(delta));
            }
        }
    }
}
