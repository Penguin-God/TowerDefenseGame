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
            int result = sut.CalculateRedScore(combineTargetFlag);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
