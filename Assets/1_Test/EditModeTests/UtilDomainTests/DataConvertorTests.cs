using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Windows;

namespace UtilDomainTests
{
    public class DataConvertorTests
    {
        DataConvertUtili CreateDataConvertor() => new DataConvertUtili();

        [Test]
        public void ������_����_�÷��׸�_�����ؾ�_��()
        {
            var sut = CreateDataConvertor();
            float[] input = { 0, 1 };

            UnitFlags result = sut.ToUnitFlag(input);

            Assert.AreEqual(new UnitFlags(0, 1), result);
        }

        [Test]
        public void ������_����_�����͸�_�����ؾ�_��()
        {
            var sut = CreateDataConvertor();
            float[] input = { 1, 2, 50 };

            UnitUpgradeData result = sut.ToUnitUpgradeData(input);

            Assert.AreEqual(UnitUpgradeType.Scale, result.UpgradeType);
            Assert.AreEqual(UnitColor.Yellow, result.TargetColor);
            Assert.AreEqual(50, result.Value);
        }

        [Test]
        public void �迭��_���̰�_��������_������_���ܸ�_������_��()
        {
            var sut = CreateDataConvertor();
            float[] input = { 0, 0, 0, 0 };

            Assert.Throws<ArgumentException>(() => sut.ToUnitFlag(input));
            Assert.Throws<ArgumentException>(() => sut.ToUnitUpgradeData(input));
        }
    }
}
