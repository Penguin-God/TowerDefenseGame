using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit_Archer : RangeUnit, IEvent
{
    [Header("아처 변수")]
    public GameObject arrow;
    public Transform arrowTransform;
    private GameObject trail;

    public override void OnAwake()
    {
        if(!enterStoryWorld) trail = GetComponentInChildren<TrailRenderer>().gameObject;
        skillDamage = (int)(damage * 1.2f);
        SettingWeaponPool(arrow, 15);
    }

    public override void NormalAttack()
    {
        StartCoroutine("ArrowAttack");
    }

    IEnumerator ArrowAttack()
    {
        base.StartAttack();

        nav.isStopped = true;
        trail.SetActive(false);
        if (target != null && enemyDistance < chaseRange)
        {
            UsedWeapon(arrowTransform, Get_ShootDirection(2f, target), 50);

            //CollisionWeapon UseWeapon = GetWeapon_FromPool();
            //UseWeapon.transform.position = arrowTransform.position;
            //UseWeapon.Shoot(Get_ShootDirection(2f, target), 50, (Enemy enemy) => delegate_OnHit(enemy));
            //GameObject instantEnergyBall = CreateBullte(energyBall, energyBallTransform, delegate_OnHit);
            //ShotBullet(instantEnergyBall, 2f, 50f, target);
        }
        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.isStopped = false;

        base.NormalAttack();
    }

    public override void SpecialAttack()
    {
        if (target.gameObject.CompareTag("Tower") || target.gameObject.CompareTag("Boss"))
        {
            NormalAttack();
            PlayNormalAttackClip();
            return;
        }
        StartCoroutine(Special_ArcherAttack());
    }

    IEnumerator Special_ArcherAttack()
    {
        base.StartAttack();
        isAttackDelayTime = true;
        nav.angularSpeed = 1;
        trail.SetActive(false);

        int enemyCount = 3;
        Transform[] targetArray = Set_AttackTarget(target, enemySpawn.currentEnemyList, enemyCount);
        for (int i = 0; i < targetArray.Length; i++)
        {
            UsedWeapon(arrowTransform, Get_ShootDirection(2f, targetArray[i]), 50);

            //CollisionWeapon UseWeapon = GetWeapon_FromPool();
            //UseWeapon.transform.position = arrowTransform.position;
            //UseWeapon.Shoot(Get_ShootDirection(2f, targetArray[i]), 50, (Enemy enemy) => delegate_OnHit(enemy));

            //GameObject instantArrow = CreateBullte(arrow, arrowTransform, delegate_OnSkile);
            //instantArrow.GetComponent<SphereCollider>().radius = 5f; // 적이 잘 안맞아서 반지름 늘림
            //ShotBullet(instantArrow, 3f, 50f, targetArray[i]);
        }
        if (enterStoryWorld == GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(normalAttackClip);

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.angularSpeed = 1000;

        base.NormalAttack();
    }

    // 첫번째에 targetTransform을 넣고 currentEnemyList에서 targetTransform을 가장 가까운 transform을 count 크기만큼 가지는 array를 return하는 함수
    Transform[] Set_AttackTarget(Transform p_Target, List<GameObject> currentEnemyList, int count)
    {
        if (currentEnemyList.Count == 0) return null;

        List<Transform> tf_EnemyList = new List<Transform>(); // 새로운 리스트 생성
        for(int i = 0; i < currentEnemyList.Count; i++)
        {
            tf_EnemyList.Add(currentEnemyList[i].transform);
        }
        Transform[] targetArray = new Transform[count];
        targetArray[0] = p_Target;
        tf_EnemyList.Remove(p_Target);

        float shortDistance = 150f;
        Transform targetTransform= null;

        for (int i = 1; i < count; i++) // 위에서 array에 targetTransform을 넣었으니 i가 1부타 시작
        {
            if(tf_EnemyList.Count != 0 && !target.gameObject.CompareTag("Tower") &&  !target.gameObject.CompareTag("Boss"))
            {
                foreach (Transform enemyTransform in tf_EnemyList)
                {
                    if (enemyTransform != null)
                    {
                        float distanceToEnemy = Vector3.Distance(p_Target.position, enemyTransform .position);
                        if (distanceToEnemy < shortDistance)
                        {
                            shortDistance = distanceToEnemy;
                            targetTransform = enemyTransform ; 
                        }
                    }
                }
                shortDistance = 150f;
                if (targetTransform!= null)
                {
                    targetArray[i] = targetTransform;
                    tf_EnemyList.Remove(targetTransform);
                }
            }
            else targetArray[i] = p_Target; // 적이 부족하거나 보스이면 1명한테 올인
        }
        return targetArray;
    }


    // 스킬 빈도 증가 이벤트
    public void SkillPercentUp()
    {
        specialAttackPercent += 20;
    }
}
