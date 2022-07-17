using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_VioletMage : Multi_Unit_Mage
{
    //void SetSkill(GameObject _skill) => _skill.GetComponent<Multi_HitSkill>().OnHitSkile += 
    //    (Multi_Enemy enemy) => enemy.OnPoison(RpcTarget.MasterClient, 25, 8, 0.3f, 120000);

    //public override void MageSkile()
    //{
    //    UsedSkill(target.position);
    //}

    // TODO : 리터럴 죽이기
    protected override void _MageSkile()
    {
        SkillSpawn(target.position).GetComponent<Multi_HitSkill>().OnHitSkile += (enemy) => enemy.OnPoison_RPC(25, 8, 0.3f, 120000);
    }

    //// TODO : Event로 옮기기
    //IEnumerator Co_SkilleReinForce()
    //{
    //    yield return new WaitUntil(() => isUltimate);
    //    SetSkillPool(mageSkillObject, 3, SetSkill);
    //    OnUltimateSkile += () => UsedSkill(EnemySpawn.instance.GetRandom_CurrentEnemy().transform.position);
    //}
}
