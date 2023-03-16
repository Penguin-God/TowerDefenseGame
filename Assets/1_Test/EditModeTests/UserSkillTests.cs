using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UserSkillTests
    {
        [Test]
        public void 빨간_파란_유닛만_있으면_태극_켜짐()
        {
            var sut = new TaegeukConditionChecker();
            var dict = CreateCounter(new UnitFlags[] { UnitFlags.RedSowrdman, UnitFlags.BlueSowrdman});

            bool result = sut.CheckTaegeuk(UnitClass.Swordman, dict);

            Assert.IsTrue(result);
        }

        [Test]
        public void 빨간_파란_이외의_유닛이_있으면_태극_꺼짐()
        {
            var sut = new TaegeukConditionChecker();
            var dict = CreateCounter(new UnitFlags[] { UnitFlags.RedSowrdman, UnitFlags.BlueSowrdman, new UnitFlags(0, 2) });

            bool result = sut.CheckTaegeuk(UnitClass.Swordman, dict);

            Assert.IsFalse(result);
        }

        [Test]
        public void 다른_클래스의_영향은_받지_않음()
        {
            var sut = new TaegeukConditionChecker();
            var dict = CreateCounter(new UnitFlags[] { UnitFlags.RedSowrdman, UnitFlags.BlueSowrdman, new UnitFlags(1, 2) });

            bool result = sut.CheckTaegeuk(UnitClass.Swordman, dict);

            Assert.IsTrue(result);
        }

        [Test]
        public void 검정_하얀_유닛의_영향은_받지_않음()
        {
            var sut = new TaegeukConditionChecker();
            var dict = CreateCounter(new UnitFlags[] { UnitFlags.RedSowrdman, UnitFlags.BlueSowrdman, new UnitFlags(0,6), new UnitFlags(0, 7) });

            bool result = sut.CheckTaegeuk(UnitClass.Swordman, dict);

            Assert.IsTrue(result);
        }

        IReadOnlyDictionary<UnitFlags, int> CreateCounter(IEnumerable<UnitFlags> unitFlags)
        {
            var dict = new Dictionary<UnitFlags, int>();
            foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
            {
                foreach (UnitClass unitClass in Enum.GetValues(typeof(UnitClass)))
                    dict.Add(new UnitFlags(color, unitClass), unitFlags.Contains(new UnitFlags(color, unitClass)) ? 1 : 0);
            }
            return dict;
        }
    }
}
