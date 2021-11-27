using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMage : Unit_Mage
{
    [SerializeField] Transform attackDirsParent = null;
    [SerializeField] GameObject originEnergyBall = null;
    public override void OnAwake()
    {
        attackDelegate += () => StartCoroutine(Co_GreenMageSkile());
        StartCoroutine(Co_SkileReinforce());
    }

    public override void MageSkile()
    {
        base.MageSkile();
        attackDelegate();
    }


    // 평타강화 함수
    delegate void AttackDelegate();
    AttackDelegate attackDelegate = null;
    IEnumerator Co_SkileReinforce()
    {
        yield return new WaitUntil(() => isUltimate);
        attackDelegate += () => GetComponent<MultiDirectionAttack>().MultiDirectionShot(transform, energyBall);
    }


    IEnumerator Co_GreenMageSkile()
    {
        energyBall = mageEffectObject;

        // 공 튕기는 동안에는 마나 충전 못하게 하기
        int savePlusMana = plusMana;
        plusMana = 0;
        StartCoroutine("MageAttack");
        yield return new WaitUntil(() => !isAttackDelayTime);
        plusMana = savePlusMana;
        energyBall = originEnergyBall;
        Debug.Log(7);
    }
}
