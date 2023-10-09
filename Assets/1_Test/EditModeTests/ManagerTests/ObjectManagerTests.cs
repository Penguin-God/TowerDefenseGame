using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ManagerTests
{
    public class ObjectManagerTests
    {
        ObjectManager<int> CreateManager() => new ObjectManager<int>();

        [Test]
        public void AddObject_AddsElementToList()
        {
            // Arrange
            var sut = CreateManager();

            // Act
            sut.AddObject(5);

            // Assert
            Assert.AreEqual(1, sut.Count);
            Assert.AreEqual(5, sut.List[0]);

            // Act
            sut.AddObject(2);

            // Assert
            Assert.AreEqual(2, sut.List.Count);
            Assert.AreEqual(2, sut.List[1]);
        }

        [Test]
        public void RemoveObject_RemovesElementFromList()
        {
            // Arrange
            int elementToAdd = 5;
            var sut = CreateManager();
            sut.AddObject(elementToAdd);

            // Act
            sut.RemoveObject(elementToAdd);

            // Assert
            Assert.AreEqual(0, sut.Count);
        }
    }
}
