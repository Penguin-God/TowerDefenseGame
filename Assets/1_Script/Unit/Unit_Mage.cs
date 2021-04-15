using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Mage : TeamSoldier
{
    private Enemy enemy;
    private Animator animator;
    public GameObject magicLight;

    public GameObject energyBall;
    public Transform energyBallTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
        StartCoroutine(MageAttack());
    }

    IEnumerator MageAttack()
    {
        LookEnemy();
        yield return new WaitForSeconds(0.2f);
        animator.SetTrigger("isAttack");
        nav.isStopped = true;
        yield return new WaitForSeconds(0.6f);
        magicLight.SetActive(true);

        if (target != null && Vector3.Distance(target.position, transform.position) < 150f)
        {
            GameObject instantEnergyBall = CreateBullte(energyBall, energyBallTransform);
            ShotBullet(instantEnergyBall, 2f, 35f);
        }

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        yield return new WaitForSeconds(1f);
        nav.isStopped = false;
    }
}
