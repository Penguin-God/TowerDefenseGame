using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_GreenMage : Multi_Unit_Mage
{
    System.Action skillAct = null;
    public override void SetMageAwake()
    {
        SetSkillPool(mageSkillObject, 3);
        attackRange *= 2; // 패시브
        skillAct += () => ShootSkill(energyBallTransform.position);
        StartCoroutine(Co_SkileReinforce());
    }

    void ShootSkill(Vector3 _pos)
    {
        GameObject _skill = UsedSkill(_pos);
        _skill.GetComponent<Multi_Projectile>().Shot(_pos, Get_ShootDirection(2f, target), 100, OnSkileHit);
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
        if(skillAct != null) skillAct();

        yield return new WaitForSeconds(0.5f);
        nav.isStopped = false;
    }

    // isSkillAttack 조절이 안되서 지금은 의미없는 코드임
    IEnumerator Co_FixMana()
    {
        // 공 튕기는 동안에는 마나 충전 못하게 하기
        int savePlusMana = PlusMana;
        PlusMana = 0;
        yield return new WaitForSeconds(skillCoolDownTime); // skillCoolDownTime을 마나 제한 시간으로 사용
        PlusMana = savePlusMana;
    }

    IEnumerator Co_SkileReinforce()
    {
        yield return new WaitUntil(() => isUltimate);
        SetSkillPool(mageSkillObject, 7);
        skillAct += Ultimate;
    }

    [SerializeField] Transform UltimateTransform = null;
    void Ultimate()
    {
        for (int i = 0; i < UltimateTransform.childCount; i++)
        {
            GameObject _skill = UsedSkill(UltimateTransform.GetChild(i).position);
            RPC_Utility.Instance.RPC_Rotation(photonView.ViewID, UltimateTransform.GetChild(i).rotation);
            //_skill.GetComponent<MyPunRPC>().RPC_Rotation(UltimateTransform.GetChild(i).rotation);
        }
    }
}
