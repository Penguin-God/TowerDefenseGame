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
                { // ���� ��� 3 = ���� �ü�
                    RedArcher,
                    new CombineCondition(RedArcher, new Dictionary<UnitFlags, int>(){ { RedSowrdman, 3 }})
                },
                { // ���� ��� 2 + ���� �ü� 3 = ���� â��
                    RedSpearman,
                    new CombineCondition(RedSpearman, new Dictionary<UnitFlags, int>(){ {RedSowrdman, 2 }, { RedArcher , 3} })
                },
                { // ���� �ü� 2 + ���� â�� 3 = ���� ����
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
        public void ���տ�_����_���ֿ�_�°�_������_�׿���_��(int classNum, int expected)
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
        public void ������_���ÿ�_����_�������_���Ǿ��_��(int score, int stack, int expected)
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
