using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Mage : RangeUnit, IUnitMana
{
    private Animator animator;
    public GameObject magicLight;

    public GameObject energyBall;
    public Transform energyBallTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (unitColor == UnitColor.black || unitColor == UnitColor.white) return;
        canvasRectTransform = transform.parent.GetComponentInChildren<RectTransform>();
        manaSlider = transform.parent.GetComponentInChildren<Slider>();
        manaSlider.maxValue = maxMana;
        manaSlider.value = currentMana;
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
                attackRange *= 2;
                break;
            case UnitColor.orange:
                bossDamage *= 6;
                break;
            case UnitColor.violet:
                break;
        }
    }

    public override void NormalAttack()
    {
        if (currentMana < 100) StartCoroutine("MageAttack");
        else MageSpecialAttack();
    }

    IEnumerator MageAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

        nav.angularSpeed = 1;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        AddMana(30);
        magicLight.SetActive(true);

        if (target != null && Vector3.Distance(target.position, transform.position) < 150f)
        {
            GameObject instantEnergyBall = CreateBullte(energyBall, energyBallTransform);
            ShotBullet(instantEnergyBall, 2f, 50f, target);
        }

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.angularSpeed = 1000;
        
        isAttack = false;
        base.NormalAttack();
    }

    void MageSpecialAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

        Debug.Log("특별하다!!!!!!");
        MageColorSpecialAttack();
        ClearMana();

        isAttack = false;
        base.NormalAttack();
    }

    void MageColorSpecialAttack()
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
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                break;
        }
    }

    public RectTransform canvasRectTransform;
    public Slider manaSlider;
    public int maxMana;
    public int currentMana;

    public void SetCanvas()
    {
        if (unitColor == UnitColor.black || unitColor == UnitColor.white) return;
        canvasRectTransform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }

    public void AddMana(int addMana)
    {
        if (unitColor == UnitColor.black || unitColor == UnitColor.white) return;
        currentMana += addMana;
        manaSlider.value = currentMana;
    }

    public void ClearMana()
    {
        currentMana = 0;
        manaSlider.value = 0;
    }

    public override void RangeUnit_PassiveAttack(Enemy enemy)
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
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                enemy.EnemyStern(60, 3);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            switch (unitColor)
            {
                case UnitColor.red:
                    break;
                case UnitColor.blue:
                    enemy.EnemySlow(50);
                    break;
                case UnitColor.yellow:
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    break;
            }
        }

        if(other.gameObject.layer == 9)
        {
            TeamSoldier otherTeamSoldier = other.gameObject.GetComponent<TeamSoldier>();
            switch (unitColor)
            {
                case UnitColor.red:
                    otherTeamSoldier.damage *= 2;
                    break;
                case UnitColor.blue:
                    break;
                case UnitColor.yellow:
                    otherTeamSoldier.attackDelayTime *= 0.5f;
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    break;
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            switch (unitColor)
            {
                case UnitColor.red:
                    break;
                case UnitColor.blue:
                    enemy.ExitSlow();
                    break;
                case UnitColor.yellow:
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    break;
            }
        }

        if (other.gameObject.layer == 9)
        {
            TeamSoldier otherTeamSoldier = other.gameObject.GetComponent<TeamSoldier>();
            switch (unitColor)
            {
                case UnitColor.red:
                    otherTeamSoldier.damage /= 2;
                    break;
                case UnitColor.blue:
                    break;
                case UnitColor.yellow:
                    otherTeamSoldier.attackDelayTime *= 2f;
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    break;
            }
        }
    }

}