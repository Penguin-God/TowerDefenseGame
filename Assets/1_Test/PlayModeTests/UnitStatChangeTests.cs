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
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            new GameObject("MultiHelper").AddComponent<MultiDevelopHelper>().EditorConnect();
        }

        [UnityTest]
        public IEnumerator AddUnitDamage()
        {
            Multi_SpawnManagers.NormalUnit.gameObject.AddComponent<PhotonView>();
            Multi_UnitManager.Instance.gameObject.AddComponent<PhotonView>();
            Multi_SpawnManagers.NormalUnit.Spawn(UnitFlags.RedSowrdman);

            const int ADD_AMOUNT = 100;
            int result = Multi_GameManager.Instance.UnitDamageManager.GetUnitDamage(UnitFlags.RedSowrdman) + ADD_AMOUNT;
            Multi_GameManager.Instance.UnitStatChanger.AddUnitDamage(ADD_AMOUNT);


            yield return null;
            Assert.AreEqual(result, Multi_UnitManager.Instance.FindUnit(0, UnitFlags.RedSowrdman).Stat.Damage);
            Assert.AreEqual(result, Multi_GameManager.Instance.UnitDamageManager.GetUnitDamage(UnitFlags.RedSowrdman));
        }
    }
}
