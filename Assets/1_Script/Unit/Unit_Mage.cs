using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Mage : TeamSoldier
{
    private Enemy enemy;
    private Animator animator;
    public GameObject energyBall;
    public GameObject light;
    public Transform energyBallTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        speed = 3f;
        attackDelayTime = 3f;
        attackRange = 40f;
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
        light.SetActive(true);

        if (target != null && Vector3.Distance(target.position, transform.position) < 150f)
        {
            GameObject instantEnergyBall = Instantiate(energyBall, energyBallTransform.position, energyBallTransform.rotation);
            AttackWeapon attackWeapon = instantEnergyBall.GetComponent<AttackWeapon>();
            attackWeapon.attackUnit = this.gameObject; // 화살과 적의 충돌감지를 위한 대입
            ShotBullet(instantEnergyBall, 2f, 35f);
        }

        yield return new WaitForSeconds(0.5f);
        light.SetActive(false);
        yield return new WaitForSeconds(1f);
        nav.isStopped = false;
    }
}
