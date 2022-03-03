using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_GreenMage : Multi_Unit_Mage
{
    
    System.Action skillAct = null;
    public override void SetMageAwake()
    {
        skillPoolManager.SettingSkilePool(mageSkillObject, 3);
        attackRange *= 2; // 패시브
        skillAct += () => ShootSkill(energyBallTransform.position);
        StartCoroutine(Co_SkileReinforce());
    }

    void ShootSkill(Vector3 _pos)
    {
        GameObject _skill = skillPoolManager.UsedSkill(_pos);
        _skill.GetComponent<Multi_Projectile>().Shot(_pos, Get_ShootDirection(2f, target), 50, (Multi_Enemy enemy) => delegate_OnHit(enemy));
    }

    public override void MageSkile()
    {
        StartCoroutine(Co_GreenMageSkile());
        StartCoroutine(Co_FixMana());
    }

    IEnumerator Co_GreenMageSkile()
    {
        nav.isStopped = true;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        //magicLight.SetActive(true);
        if(skillAct != null) skillAct();

        yield return new WaitForSeconds(0.5f);
        //magicLight.SetActive(false);
        nav.isStopped = false;
    }

    // isSkillAttack 조절이 안되서 지금은 의미없는 코드임
    IEnumerator Co_FixMana()
    {
        // 공 튕기는 동안에는 마나 충전 못하게 하기
        int savePlusMana = PlusMana;
        PlusMana = 0;
        yield return new WaitUntil(() => !isSkillAttack);
        PlusMana = savePlusMana;
    }

    IEnumerator Co_SkileReinforce()
    {
        yield return new WaitUntil(() => isUltimate);
        skillPoolManager.SettingSkilePool(mageSkillObject, 7);
        skillAct += Ultimate;
    }

    [SerializeField] Transform UltimateTransform = null;
    void Ultimate()
    {
        for (int i = 0; i < UltimateTransform.childCount; i++)
        {
            GameObject _skill = skillPoolManager.UsedSkill(UltimateTransform.GetChild(i).position);
            _skill.GetComponent<MyPunRPC>().RPC_Rotation(UltimateTransform.GetChild(i).rotation);
        }
    }
}
