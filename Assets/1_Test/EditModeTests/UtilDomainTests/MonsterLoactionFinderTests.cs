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
        public void ���͵���_�ٴϴ�_���_������_��ġ��_ã�ƾ�_��()
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
                // ��ȯ�� ��ġ�� ��� ��������Ʈ ���̿� �ִ��� Ȯ���մϴ�.
                Assert.That(result.x, Is.InRange(0, 10));
                Assert.That(result.z, Is.InRange(0, 10));
            }
        }
    }
}
