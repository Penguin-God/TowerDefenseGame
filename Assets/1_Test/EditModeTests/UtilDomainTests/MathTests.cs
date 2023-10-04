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
        public void 적절한_퍼센트_소수_값을_반환해야_함(float total, float percent, float expected)
        {
            Assert.That(expected, Is.EqualTo(MathUtil.CalculatePercentage(total, percent)).Within(0.01f));
        }

        [Test]
        [TestCase(100, 30, 30)]
        [TestCase(100, 4.8f, 5)]
        [TestCase(100, 0, 0)]
        [TestCase(0, 5, 0)]
        public void 적절한_퍼센트_정수_값을_반환해야_함(int total, float percent, int expected)
        {
            Assert.AreEqual(expected, MathUtil.CalculatePercentage(total, percent));
        }

        [Test]
        public void 방향_8개_계산_시_원을_기준으로_동일한_간격만큼_벌어져야_함()
        {
            // Arrange
            int numberOfDirections = 8;
            float delta = 0.01f; // 허용 가능한 오차

            Vector2[] expectedDirections =
            {
            new Vector2(1f, 0f), // 0도
            new Vector2(0.7071f, 0.7071f), // 45도
            new Vector2(0f, 1f), // 90도
            new Vector2(-0.7071f, 0.7071f), // 135도
            new Vector2(-1f, 0f), // 180도
            new Vector2(-0.7071f, -0.7071f), // 225도
            new Vector2(0f, -1f), // 270도
            new Vector2(0.7071f, -0.7071f) // 315도
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
        public void 위치_8개_계산_시_중심에서_반지름만큼_떨어지고_위치끼리_동일한_간격만큼_벌어져야_함()
        {
            // Arrange
            int numberOfPositions = 8;
            float distanceFromCenter = 1f; // 단위 원을 기준으로 함
            float delta = 0.01f; // 허용 가능한 오차

            Vector2[] expectedPositions =
            {
            new Vector2(1f, 0f), // 0도
            new Vector2(0.7071f, 0.7071f), // 45도
            new Vector2(0f, 1f), // 90도
            new Vector2(-0.7071f, 0.7071f), // 135도
            new Vector2(-1f, 0f), // 180도
            new Vector2(-0.7071f, -0.7071f), // 225도
            new Vector2(0f, -1f), // 270도
            new Vector2(0.7071f, -0.7071f) // 315도
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
