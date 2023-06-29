using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ContainerTests
{
    BattleDIContainer CreateContainer() => new BattleDIContainer(null);

    [Test]
    public void AddAndGetService_Test1_IsSuccessful()
    {
        // Arrange
        var test1Instance = new Test1("TestName");
        var container = CreateContainer();

        // Act
        container.AddService(test1Instance);
        var retrievedInstance = container.GetService<Test1>();

        // Assert
        Assert.Throws<InvalidOperationException>(() => container.GetService<Test2>());
        Assert.IsNotNull(retrievedInstance);
        Assert.AreEqual("TestName", retrievedInstance.Name);
    }

    [Test]
    public void AddAndGetService_Test2_IsSuccessful()
    {
        // Arrange
        var test2Instance = new Test2(123);
        var container = CreateContainer();

        // Act
        container.AddService(test2Instance);
        var retrievedInstance = container.GetService<Test2>();

        // Assert
        Assert.Throws<InvalidOperationException>(() => container.GetService<Test1>());
        Assert.IsNotNull(retrievedInstance);
        Assert.AreEqual(123, retrievedInstance.Number);
    }
}

public class Test1
{
    public string Name { get; set; }
    public Test1(string name) => Name = name;
}

public class Test2
{
    public int Number { get; set; }
    public Test2(int number) => Number = number;
}