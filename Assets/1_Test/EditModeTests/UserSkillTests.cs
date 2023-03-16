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
            var hashSet = CreateCounter(UnitFlags.RedSowrdman, UnitFlags.BlueSowrdman);

            bool result = sut.CheckTaegeuk(UnitClass.Swordman, hashSet);

            Assert.IsTrue(result);
        }

        [Test]
        public void 빨간_파란_이외의_유닛이_있으면_태극_꺼짐()
        {
            var sut = new TaegeukConditionChecker();
            var hashSet = CreateCounter( UnitFlags.RedSowrdman, UnitFlags.BlueSowrdman, new UnitFlags(2, 0));

            bool result = sut.CheckTaegeuk(UnitClass.Swordman, hashSet);

            Assert.IsFalse(result);
        }

        [Test]
        public void 다른_클래스의_영향은_받지_않음()
        {
            var sut = new TaegeukConditionChecker();
            var hashSet = CreateCounter( UnitFlags.RedSowrdman, UnitFlags.BlueSowrdman, new UnitFlags(0, 1));

            bool result = sut.CheckTaegeuk(UnitClass.Swordman, hashSet);

            Assert.IsTrue(result);
        }

        [Test]
        public void 검정_하얀_유닛의_영향은_받지_않음()
        {
            var sut = new TaegeukConditionChecker();
            var hashSet = CreateCounter( UnitFlags.RedSowrdman, UnitFlags.BlueSowrdman, new UnitFlags(0,6), new UnitFlags(0, 7));

            bool result = sut.CheckTaegeuk(UnitClass.Swordman, hashSet);

            Assert.IsTrue(result);
        }

        HashSet<UnitFlags> CreateCounter(params UnitFlags[] flags)
            => new HashSet<UnitFlags>(flags);
    }
}
