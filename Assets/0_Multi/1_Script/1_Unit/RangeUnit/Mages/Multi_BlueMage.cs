using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_BlueMage : Multi_Unit_Mage
{
    Multi_BluePassive bluePassive = null;
    SphereCollider blueCollider = null;

    public override void SetMageAwake()
    {
        SetSkillPool(mageSkillObject, 3, SetEnemyFreeze);

        bluePassive = GetComponent<Multi_BluePassive>();
        blueCollider = GetComponentInChildren<SphereCollider>();
        blueCollider.radius = bluePassive.Get_ColliderRange;
        bluePassive.OnBeefup += () => blueCollider.radius = bluePassive.Get_ColliderRange;

        StartCoroutine(Co_SkilleReinForce());
    }

    void SetEnemyFreeze(GameObject _skill) =>
        _skill.GetComponent<Multi_HitSkill>().OnHitSkile += (Multi_Enemy enemy) => enemy.OnFreeze(RpcTarget.MasterClient, 5f);

    public override void MageSkile() => UsedSkill(transform.position + (Vector3.up * 2));


    IEnumerator Co_SkilleReinForce()
    {
        yield return new WaitUntil(() => isUltimate);
        UpdateSkill(SkilleReinForce);
    }

    void SkilleReinForce(GameObject _skill)
    {
        _skill.GetComponent<Multi_HitSkill>().OnHitSkile += (Multi_Enemy enemy) => enemy.OnDamage(RpcTarget.MasterClient, 20000);
    }


    private void OnTriggerStay(Collider other)
    {
        if (!pv.IsMine) return;

        if (other.GetComponentInParent<Multi_NormalEnemy>() != null) // 나가기 전까진 무한 슬로우
            other.GetComponentInParent<Multi_NormalEnemy>().OnSlow(RpcTarget.MasterClient, bluePassive.Get_SlowPercent, -1);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!pv.IsMine) return;

        if (other.GetComponentInParent<Multi_NormalEnemy>() != null)
            other.GetComponentInParent<Multi_NormalEnemy>().ExitSlow(RpcTarget.All);
    }
}
