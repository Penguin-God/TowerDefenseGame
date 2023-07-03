using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ContainerTests
    {
        BattleDIContainer CreateContainer() => new BattleDIContainer(null);

        [Test]
        public void �����̳ʿ�_������_ã��_��_�־���ϰ�_���°�_����_������_��()
        {
            // Arrange
            var test1Instance = new Test1("TestName");
            var sut = CreateContainer();

            // Act
            sut.AddService(test1Instance);
            var result = sut.GetService<Test1>();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("TestName", result.Name);
            Assert.Throws<InvalidOperationException>(() => sut.GetService<Test2>());
        }

        [Test]
        public void �Ű�����_����_�����ڴ�_�ڵ�_����_�Ǿ�_��()
        {
            // Arrange
            var sut = CreateContainer();

            // Act
            sut.AddService<Test2>();
            var result = sut.GetService<Test2>();
            result.Number = 123;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Number);
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
    }
}
