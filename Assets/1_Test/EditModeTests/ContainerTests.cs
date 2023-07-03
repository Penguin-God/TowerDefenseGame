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
        public void 컨테이너에_넣은건_찾을_수_있어야하고_없는건_예외_던져야_함()
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
        public void 매개변수_없는_생성자는_자동_생성_되야_함()
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
