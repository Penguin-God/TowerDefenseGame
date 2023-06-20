using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SpawnPositionCalculatorTests
{
    [Test]
    public void ����_��ġ��_������_����_�̳���_��ġ��_�����ؾ�_��()
    {
        // Arrange
        var worldPos = new Vector3(0, 0, 0);
        var spawnRange = 10f;
        var calculator = new WorldSpawnPositionCalculator(spawnRange, 0, 0, 0);

        // Act
        var result = calculator.CalculateWorldPostion(worldPos);

        // Assert
        Assert.That(result.x, Is.InRange(-10f, 10f));
        Assert.That(result.z, Is.InRange(-10f, 10f));
    }

    [Test]
    public void ������_��_��ġ��_OffSet��_�����_���¿���_������_����_����_��ġ��_��ȯ�ؾ�_��()
    {
        // Arrange
        var towerPos = new Vector3(0, 0, 0);
        var offsetZ = -10f;
        var spawnRangeX = 10f;
        var spawnRangeZ = 10f;
        var calculator = new WorldSpawnPositionCalculator(0, offsetZ, spawnRangeX, spawnRangeZ);

        // Act
        var result = calculator.CalculateEnemyTowerPostion(towerPos);

        // Assert
        Assert.That(result.x, Is.InRange(-10f, 10f));
        Assert.That(result.z, Is.InRange(-20f, 0f));
    }
}
