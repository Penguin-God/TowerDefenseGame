using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UnitManagerTests
    {
        //[Test]
        //public void 유닛이_매니저에_추가되면_찾을_수_있어야함()
        //{
        //    var sut = new UnitManager();

        //    sut.RegisterUnit(new Unit(UnitFlags.RedSowrdman));

        //    Assert.AreEqual(1, sut.UnitCount);
        //    Assert.NotNull(sut.GetUnit(UnitFlags.RedSowrdman));
        //    Assert.IsNull(sut.GetUnit(UnitFlags.BlueSowrdman));
        //}

        //[Test]
        //public void 유닛이_죽으면_매니저에서_삭제되어야함()
        //{
        //    var sut = new UnitManager();
        //    var unit = new Unit(UnitFlags.RedSowrdman);
        //    sut.RegisterUnit(unit);

        //    unit.Dead();

        //    Assert.AreEqual(0, sut.UnitCount);
        //    Assert.IsNull(sut.GetUnit(UnitFlags.RedSowrdman));
        //}

        [Test]
        public void CombineResultTrue()
        {
            var sut = new UnitCombineSystem(CreateCombineCondition());
            var flags = new UnitFlags[] { UnitFlags.RedSowrdman, UnitFlags.RedSowrdman, UnitFlags.RedSowrdman };

            var result = sut.CheckCombineable(new UnitFlags(0, 1), flags);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void CombineResultFalse()
        {
            var sut = new UnitCombineSystem(CreateCombineCondition());
            var flags = new UnitFlags[] { UnitFlags.RedSowrdman, UnitFlags.RedSowrdman };

            var result = sut.CheckCombineable(new UnitFlags(0, 1), flags);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void 조합할_수_있는_유닛은_2개()
        {
            var sut = new UnitCombineSystem(CreateCombineCondition());
            var flags = new UnitFlags[] { UnitFlags.RedSowrdman, UnitFlags.RedSowrdman, UnitFlags.RedSowrdman, UnitFlags.BlueSowrdman };

            var result = sut.GetCombinableUnitFalgs(flags);

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void 보라_기사를_만들기_위해서는_빨간_기사와_파란_기사가_필요함()
        {
            var sut = new UnitCombineSystem(CreateCombineCondition());

            var result = sut.GetNeedFlags(new UnitFlags(5, 0));

            Assert.AreEqual(2, result.Count());
            Assert.Contains(UnitFlags.RedSowrdman, result.ToArray());
            Assert.Contains(UnitFlags.BlueSowrdman, result.ToArray());
        }

        Dictionary<UnitFlags, CombineCondition> CreateCombineCondition()
            => new Dictionary<UnitFlags, CombineCondition>()
            {
                { // 빨간 기사 3 = 빨간 궁수
                    new UnitFlags(0, 1),
                    new CombineCondition(new UnitFlags(0, 1),
                    new Dictionary<UnitFlags, int>(){ {UnitFlags.RedSowrdman, 3 }})
                },
                { // 빨간 기사 1 + 파란 기사 1 = 보라 기사
                    new UnitFlags(5, 0),
                    new CombineCondition(new UnitFlags(5, 0),
                    new Dictionary<UnitFlags, int>(){ {UnitFlags.RedSowrdman, 1 }, { UnitFlags.BlueSowrdman, 1 } })
                }
            };
    }
}
