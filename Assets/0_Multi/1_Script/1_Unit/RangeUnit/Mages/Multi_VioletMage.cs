using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_VioletMage : Multi_Unit_Mage
{
    public override void SetMageAwake()
    {
        SetSkillPool(mageSkillObject, 3, SetSkill);
        StartCoroutine(Co_SkilleReinForce());
    }

    void SetSkill(GameObject _skill) => _skill.GetComponent<Multi_HitSkill>().OnHitSkile += 
        (Multi_Enemy enemy) => enemy.photonView.RPC("OnPoison", RpcTarget.MasterClient, 25, 8, 0.3f, 120000);

    public override void MageSkile()
    {
        UsedSkill(target.position);
    }

    IEnumerator Co_SkilleReinForce()
    {
        yield return new WaitUntil(() => isUltimate);
        SetSkillPool(mageSkillObject, 3, SetSkill);
        OnUltimateSkile += () => UsedSkill(EnemySpawn.instance.GetRandom_CurrentEnemy().transform.position);
    }
}
