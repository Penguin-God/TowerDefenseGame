using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spearman : MeeleUnit
{
    private Animator animator;
    public GameObject trail;
    public GameObject spear;
    public GameObject skileSpaer; // 발사할 때 생성하는 창
    public Transform spearCreatePosition;
    public GameObject dontMoveGameObject;

    private void Awake()
    {
        dontMoveGameObject = GameObject.Find("World");
        animator = GetComponent<Animator>();
    }

    public override void SetPassive()
    {
        switch (unitColor)
        {
            case UnitColor.red:
                break;
            case UnitColor.blue:
                break;
            case UnitColor.yellow:
                break;
            case UnitColor.green:
                damage *= 3;
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                break;
        }
    }

    public override void NormalAttack()
    {
        StartCoroutine("SpaerAttack");
    }

    IEnumerator SpaerAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.55f);
        trail.SetActive(true);
        audioSource.Play();
        yield return new WaitForSeconds(0.3f);
        HitMeeleAttack();
        yield return new WaitForSeconds(0.3f);
        trail.SetActive(false);

        isAttack = false;
        base.NormalAttack();
    }

    public override void SpecialAttack()
    {
        //base.SpecialAttack(); // 나중에 스킬 쿨타임을 따로 만들수도 있음
        StartCoroutine("Spearman_SpecialAttack");
    }

    IEnumerator Spearman_SpecialAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;
        animator.SetTrigger("isSpecialAttack");
        yield return new WaitForSeconds(1f);

        spear.SetActive(false);
        nav.isStopped = true;
        //Vector3 createRotation = new Vector3(-90f, transform.parent.rotation.y, spearCreatePosition.rotation.z);
        //Debug.Log(createRotation);
        GameObject instantSpear = Instantiate(skileSpaer, spearCreatePosition);
        instantSpear.transform.SetParent(dontMoveGameObject.transform);
        instantSpear.GetComponent<AttackWeapon>().attackUnit = this.gameObject;
        instantSpear.GetComponent<Rigidbody>().velocity = (-1 * transform.forward) * 50;

        yield return new WaitForSeconds(0.5f);
        nav.isStopped = false;
        spear.SetActive(true);
        isAttack = false;
        base.NormalAttack();
    }

    

    public override void MeeleUnit_PassiveAttack(Enemy enemy)
    {
        switch (unitColor)
        {
            case UnitColor.red:
                break;
            case UnitColor.blue:
                enemy.EnemySlow(50, 3);
                break;
            case UnitColor.yellow:
                Add_PassiveGold(5, 2);
                break;
            case UnitColor.green:
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                enemy.EnemyStern(30, 2);
                break;
        }
    }
}
