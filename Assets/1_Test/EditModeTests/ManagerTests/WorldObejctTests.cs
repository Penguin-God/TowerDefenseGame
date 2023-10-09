using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ManagerTests
{
    public class WorldObejctTests
    {
        WorldObjectManager<int> CreateWorldObjectManager() => new WorldObjectManager<int>();

        [Test]
        public void 객체_추가_시_지정된_Id에_해야_함()
        {
            var sut = CreateWorldObjectManager();
            sut.AddObject(1, 0);
            sut.AddObject(2, 0);
            sut.AddObject(3, 1);

            IReadOnlyList<int> world0Objects = sut.GetList(0);
            IReadOnlyList<int> world1Objects = sut.GetList(1);

            Assert.AreEqual(2, world0Objects.Count);
            CollectionAssert.Contains(world0Objects, 1);
            CollectionAssert.Contains(world0Objects, 2);
            CollectionAssert.DoesNotContain(world0Objects, 3);

            Assert.AreEqual(1, world1Objects.Count);
            CollectionAssert.Contains(world1Objects, 3);
            CollectionAssert.DoesNotContain(world1Objects, 1);
        }

        [Test]
        public void 객체_삭제_시_지정된_Id에_해야_함()
        {
            var sut = CreateWorldObjectManager();
            sut.AddObject(1, 0);
            sut.AddObject(2, 0);
            sut.AddObject(3, 1);

            sut.RemoveObject(1, 0);
            sut.RemoveObject(3, 1);

            IReadOnlyList<int> world0Objects = sut.GetList(0);
            IReadOnlyList<int> world1Objects = sut.GetList(1);

            Assert.AreEqual(1, world0Objects.Count);
            CollectionAssert.Contains(world0Objects, 2);
            CollectionAssert.DoesNotContain(world0Objects, 1);

            Assert.AreEqual(0, world1Objects.Count);
            CollectionAssert.DoesNotContain(world1Objects, 3);
        }
    }

}
