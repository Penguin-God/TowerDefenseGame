using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Photon.Pun;

namespace Tests
{
    public class UnitStatChangeTests
    {
        [UnityTest]
        public IEnumerator AddUnitDamage()
        {
            const int ADD_AMOUNT = 100;
            var unit = Multi_SpawnManagers.NormalUnit.Spawn(UnitFlags.RedSowrdman);
            yield return null;

            Multi_UnitManager.Instance.Stat.AddUnitDamage(ADD_AMOUNT);

            int result = Multi_GameManager.Instance.UnitDamageManager.GetUnitDamage(UnitFlags.RedSowrdman) + ADD_AMOUNT;
            Assert.AreEqual(result, unit.Stat.Damage);
            Assert.AreEqual(result, Multi_GameManager.Instance.UnitDamageManager.GetUnitDamage(UnitFlags.RedSowrdman));
        }
    }
}
