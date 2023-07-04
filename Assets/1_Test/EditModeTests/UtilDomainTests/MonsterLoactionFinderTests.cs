using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UtilDomainTests
{
    public class MonsterLoactionFinderTests
    {
        [Test]
        public void 몬스터들이_다니는_경로_내에서_위치를_찾아야_함()
        {
            // Arrange
            Vector3[] points = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(10, 0, 0),
                new Vector3(10, 0, 10),
                new Vector3(0, 0, 10)
            };
            var sut = new MonsterPathLocationFinder(points);

            for (int i = 0; i < 10; i++)
            {
                // Act
                Vector3 result = sut.CalculateMonsterPathLocation();

                // Assert
                // 반환된 위치가 모든 웨이포인트 사이에 있는지 확인합니다.
                Assert.That(result.x, Is.InRange(0, 10));
                Assert.That(result.z, Is.InRange(0, 10));
            }
        }
    }
}
