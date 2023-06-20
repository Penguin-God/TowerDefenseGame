using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SpawnPositionCalculatorTests
{
    [Test]
    public void 월드_위치는_지정한_범위_이내에_위치를_반한해야_함()
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
    public void 적군의_성_위치는_OffSet이_적용된_상태에서_지정한_범위_내에_위치를_반환해야_함()
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
