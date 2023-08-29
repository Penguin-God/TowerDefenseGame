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
        public void 적절한_유닛_플래그를_생성해야_함()
        {
            var sut = CreateDataConvertor();
            float[] input = { 0, 1 };

            UnitFlags result = sut.ToUnitFlag(input);

            Assert.AreEqual(new UnitFlags(0, 1), result);
        }

        [Test]
        public void 적절한_유닛_데이터를_생성해야_함()
        {
            var sut = CreateDataConvertor();
            float[] input = { 1, 2, 50 };

            UnitUpgradeData result = sut.ToUnitUpgradeData(input);

            Assert.AreEqual(UnitUpgradeType.Scale, result.UpgradeType);
            Assert.AreEqual(UnitColor.Yellow, result.TargetColor);
            Assert.AreEqual(50, result.Value);
        }

        [Test]
        public void 배열의_길이가_적절하지_않으면_예외를_던져야_함()
        {
            var sut = CreateDataConvertor();
            float[] input = { 0, 0, 0, 0 };

            Assert.Throws<ArgumentException>(() => sut.ToUnitFlag(input));
            Assert.Throws<ArgumentException>(() => sut.ToUnitUpgradeData(input));
        }
    }
}
