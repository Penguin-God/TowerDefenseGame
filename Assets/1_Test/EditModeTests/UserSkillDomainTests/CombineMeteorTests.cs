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
                { // �Ķ� ��� 3 = �Ķ� �ü�
                    new UnitFlags(1, 1),
                    new CombineCondition(new UnitFlags(1, 1), new Dictionary<UnitFlags, int>(){ { new UnitFlags(1, 0), 3 }})
                },
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

            var meteorScoreData = new MeteorStackData(1, 4, 20);
            var result = new CombineMeteorStackManager(_combineConditionByUnitFlag, meteorScoreData);
            return result;
        }

        [Test]
        [TestCase(1, 1, 0)]
        [TestCase(0, 1, 3)]
        [TestCase(0, 2, 14)]
        [TestCase(0, 3, 68)]
        public void ���տ�_����_���ֿ�_�°�_������_�׿���_��(int colorNum, int classNum, int expected)
        {
            // Arrange
            var sut = CreateCombineMeteorStatkManager();
            var combineTargetFlag = new UnitFlags(colorNum, classNum);

            // Act
            sut.AddCombineStack(combineTargetFlag);

            // Assert
            Assert.AreEqual(expected, sut.CurrentStack);
        }
    }
}
