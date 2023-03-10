using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Spawners;

namespace Tests
{
    public class UnitStatChangeTests
    {
        [UnityTest]
        public IEnumerator AddUnitDamage()
        {
            const int ADD_AMOUNT = 100;
            var unitManager = new UnitManager();
            // dataManager
            var unit = new UnitSpanwer(null, null).Spawn(UnitFlags.RedSowrdman);
            yield return null;

            // 퍼사드로 스탯 증가

            int result = Multi_GameManager.Instance.UnitDamageManager.GetUnitDamage(UnitFlags.RedSowrdman) + ADD_AMOUNT;
            Assert.AreEqual(result, unit.Stat.Damage);
            Assert.AreEqual(result, Multi_GameManager.Instance.UnitDamageManager.GetUnitDamage(UnitFlags.RedSowrdman));
        }
    }
}
