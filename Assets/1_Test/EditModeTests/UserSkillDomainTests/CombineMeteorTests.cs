using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UserSkillDomainTests
{
    public class CombineMeteorTests
    {
        Dictionary<UnitFlags, CombineCondition> _combineConditionByUnitFlag;

        UnitFlags RedSowrdman = new UnitFlags(0, 0);
        UnitFlags RedArcher = new UnitFlags(0, 1);
        UnitFlags RedSpearman = new UnitFlags(0, 2);
        UnitFlags RedMage = new UnitFlags(0, 3);


        CombineMeteorStackManager CreateCombineMeteorStatkManager()
        {
            _combineConditionByUnitFlag = new Dictionary<UnitFlags, CombineCondition>()
            {
                { // »¡°£ ±â»ç 3 = »¡°£ ±Ã¼ö
                    RedArcher,
                    new CombineCondition(RedArcher, new Dictionary<UnitFlags, int>(){ { RedSowrdman, 3 }})
                },
                { // »¡°£ ±â»ç 2 + »¡°£ ±Ã¼ö 3 = »¡°£ Ã¢º´
                    RedSpearman,
                    new CombineCondition(RedSpearman, new Dictionary<UnitFlags, int>(){ {RedSowrdman, 2 }, { RedArcher , 3} })
                },
                { // »¡°£ ±Ã¼ö 2 + »¡°£ Ã¢º´ 3 = »¡°£ ¹ý»ç
                    RedMage,
                    new CombineCondition(RedMage, new Dictionary<UnitFlags, int>(){ { RedArcher, 2 }, { RedSpearman, 3} })
                },
            };

            var meteorScoreData = new MeteorStackData(1, 4, 20);
            var result = new CombineMeteorStackManager(_combineConditionByUnitFlag, meteorScoreData, 10);
            return result;
        }

        [Test]
        [TestCase(1, 3)]
        [TestCase(2, 14)]
        [TestCase(3, 68)]
        public void Á¶ÇÕ¿¡_»ç¿ëµÈ_À¯´Ö¿¡_¸Â°Ô_½ºÅÃÀÌ_½×¿©¾ß_ÇÔ(int classNum, int expected)
        {
            // Arrange
            var sut = CreateCombineMeteorStatkManager();
            var combineTargetFlag = new UnitFlags(0, classNum);

            // Act
            sut.AddCombineStack(combineTargetFlag);

            // Assert
            Assert.AreEqual(expected, sut.CurrentStack);
        }

        [Test]
        public void ½×ÀÎ_½ºÅÃ¿¡_µû¶ó_¼ÒÈ¯_Ä«¿îÆ®°¡_½×¿©¾ß_ÇÔ()
        {
            // Arrange
            var sut = CreateCombineMeteorStatkManager();

            // Act & Assert
            sut.AddCombineStack(RedArcher);
            Assert.AreEqual(0, sut.SummonUnitCount);

            // Act & Assert
            sut.AddCombineStack(RedSpearman);
            Assert.AreEqual(1, sut.SummonUnitCount);

            // Act & Assert
            sut.SummonUnit();
            Assert.AreEqual(0, sut.SummonUnitCount);

            // Act & Assert
            sut.AddCombineStack(RedMage);
            Assert.AreEqual(7, sut.SummonUnitCount);
        }
    }
}
