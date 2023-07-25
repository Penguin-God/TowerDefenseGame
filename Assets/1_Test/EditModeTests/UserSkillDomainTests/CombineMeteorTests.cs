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

        CombineMeteorCalculator CreateCombineMeteor()
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
            var result = new CombineMeteorCalculator(meteorScoreData, _combineConditionByUnitFlag);
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
            int result = sut.CalculateMeteorStack(combineTargetFlag);

            // Assert
            Assert.AreEqual(expected, result);
        }

        const int DefaultDamage = 1000;
        const int DamagePerStack = 3000;

        [Test]
        [TestCase(0, DefaultDamage)]
        [TestCase(10, 31000)]
        [TestCase(50, 151000)]
        public void ½ºÅÃ¿¡_µû¶ó_´ë¹ÌÁö°¡_°è»êµÇ¾î¾ß_ÇÔ(int stack, int expected)
        {
            // Arrange
            var sut = new CombineMeteorCalculator(null, DefaultDamage, DamagePerStack);

            // Act
            int result = sut.CalculateMeteorDamage(stack);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
