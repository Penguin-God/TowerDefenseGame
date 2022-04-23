using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMage : Unit_Mage
{
    BluePassive bluePassive = null;

    public override void SetMageAwake()
    {
        SettingSkilePool(mageSkillObject, 3, SetEnemyFreeze);
        bluePassive = GetComponent<BluePassive>();
        GetComponent<SphereCollider>().radius = bluePassive.Get_ColliderRange;
        bluePassive.OnBeefup += () => GetComponent<SphereCollider>().radius = bluePassive.Get_ColliderRange;

        StartCoroutine(Co_SkilleReinForce());  
    }

    void SetEnemyFreeze(GameObject _skill) => _skill.GetComponent<HitSkile>().OnHitSkile += (Enemy enemy) => enemy.EnemyFreeze(5f);

    IEnumerator Co_SkilleReinForce()
    {
        yield return new WaitUntil(() => isUltimate);
        UpdatePool(SkilleReinForce);
    }

    void SkilleReinForce(GameObject _skill)
    {
        _skill.GetComponent<HitSkile>().OnHitSkile += (Enemy enemy) => enemy.OnDamage(20000);
    }

    public override void MageSkile()
    {
        base.MageSkile();
        UsedSkill(transform.position + (Vector3.up * 2));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<NomalEnemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.EnemySlow(bluePassive.Get_SlowPercent, -1f); // 나가기 전까진 무한 슬로우
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<NomalEnemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.ExitSlow();
        }
    }
}
