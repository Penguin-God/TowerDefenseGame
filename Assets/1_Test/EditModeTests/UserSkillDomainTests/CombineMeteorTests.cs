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

        CombineMeteor CreateCombineMeteor()
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

            var meteorScoreData = new MeteorScoreData(1, 4, 20);
            var result = new CombineMeteor(meteorScoreData, _combineConditionByUnitFlag);
            return result;
        }

        [Test]
        [TestCase(1, 3)]
        [TestCase(2, 14)]
        [TestCase(3, 68)]
        public void Á¶ÇÕ¿¡_»ç¿ëµÈ_À¯´Ö¿¡_¸Â°Ô_Á¡¼ö°¡_½×¿©¾ß_ÇÔ(int classNum, int expected)
        {
            // Arrange
            var sut = CreateCombineMeteor();
            var combineTargetFlag = new UnitFlags(0, classNum);

            // Act
            int result = sut.CalculateMeteorScore(combineTargetFlag);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(3, 0, 1500)]
        [TestCase(3, 10, 2500)]
        [TestCase(20, 100, 20000)]
        public void Á¡¼ö¿Í_½ºÅÃ¿¡_µû¶ó_´ë¹ÌÁö°¡_°è»êµÇ¾î¾ß_ÇÔ(int score, int stack, int expected)
        {
            // Arrange
            const int DamagePerScore = 500;
            const int DamagePerStack = 100;
            var sut = CreateCombineMeteor();

            // Act
            int result = sut.CalculateMeteorDamage(score, DamagePerScore, stack, DamagePerStack);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
