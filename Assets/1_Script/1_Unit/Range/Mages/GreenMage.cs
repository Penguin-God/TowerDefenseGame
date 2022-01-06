using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMage : Unit_Mage
{
    [SerializeField] Transform UltimateTransform = null;
    public override void SetMageAwake()
    {
        SettingSkilePool(mageSkillObject, 3);
        attackRange *= 2;
        attackDelegate += () => ShootSkill(energyBallTransform.position);
        StartCoroutine(Co_SkileReinforce());
    }

    public override void MageSkile()
    {
        base.MageSkile();
        StartCoroutine(Co_GreenMageSkile());
        StartCoroutine(Co_FixMana());
    }


    // 평타강화 함수
    delegate void AttackDelegate();
    AttackDelegate attackDelegate = null;
    IEnumerator Co_SkileReinforce()
    {
        yield return new WaitUntil(() => isUltimate);
        SettingSkilePool(mageSkillObject, 7);
        attackDelegate += Ultimate;
    }

    void Ultimate()
    {
        for (int i = 0; i < UltimateTransform.childCount; i++)
        {
            GameObject _skill = UsedSkill(UltimateTransform.GetChild(i).position);
            _skill.transform.rotation = UltimateTransform.GetChild(i).rotation;
            _skill.transform.GetComponent<Rigidbody>().velocity = _skill.transform.forward * 50;
        }
    }

    IEnumerator Co_GreenMageSkile()
    {
        nav.isStopped = true;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        magicLight.SetActive(true);
        attackDelegate();

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.isStopped = false;
    }

    void ShootSkill(Vector3 _pos)
    {
        GameObject _skill = UsedSkill(_pos);
        _skill.transform.position = new Vector3(_pos.x, 1f, _pos.z);
        _skill.GetComponent<CollisionWeapon>().Shoot(Get_ShootDirection(2f, target), 50, (Enemy enemy) => delegate_OnHit(enemy));
    }

    IEnumerator Co_FixMana()
    {
        // 공 튕기는 동안에는 마나 충전 못하게 하기
        int savePlusMana = plusMana;
        plusMana = 0;
        yield return new WaitUntil(() => !isSkillAttack);
        plusMana = savePlusMana;
    }
}
