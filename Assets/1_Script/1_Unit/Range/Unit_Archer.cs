using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit_Archer : RangeUnit, IEvent, IHitThrowWeapon
{
    [Header("아처 변수")]
    public GameObject arrow;
    public Transform arrowTransform;
    private GameObject trail;

    private void Awake()
    {
        if(!enterStoryWorld) trail = GetComponentInChildren<TrailRenderer>().gameObject;
        skillDamage = (int)(damage * 1.2f);
    }

    public override void SetPassiveFigure()
    {
        redPassiveFigure = 0.4f;
        bluePassiveFigure = new Vector2(40, 2);
        yellowPassiveFigure = new Vector2(2, 1);
        greenPassiveFigure = 1.3f;
        orangePassiveFigure = 2f;
        violetPassiveFigure = new Vector3(30, 2, 250);
    }

    public override void NormalAttack()
    {
        StartCoroutine("ArrowAttack");
    }

    IEnumerator ArrowAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

        nav.isStopped = true;
        trail.SetActive(false);
        GameObject instantArrow = CreateBullte(arrow, arrowTransform);
        ShotBullet(instantArrow, 1.5f, 50f, target);
        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.isStopped = false;

        isAttack = false;
        base.NormalAttack();
    }

    public override void SpecialAttack()
    {
        if (target.gameObject.CompareTag("Tower") || target.gameObject.CompareTag("Boss"))
        {
            Invoke("NormalAttackAudioPlay", normalAttakc_AudioDelay);
            NormalAttack();
            return;
        }
        StartCoroutine(Special_ArcherAttack());
    }

    IEnumerator Special_ArcherAttack()
    {
        isSkillAttack = true;
        isAttack = true;
        isAttackDelayTime = true;
        nav.angularSpeed = 1;
        trail.SetActive(false);

        int enemyCount = 3;
        Transform[] targetArray = Set_AttackTarget(target, enemySpawn.currentEnemyList, enemyCount);
        for (int i = 0; i < targetArray.Length; i++)
        {
            GameObject instantArrow = CreateBullte(arrow, arrowTransform);
            instantArrow.GetComponent<SphereCollider>().radius = 5f; // 적이 잘 안맞아서 반지름 늘림
            ShotBullet(instantArrow, 3f, 50f, targetArray[i]);
        }
        if (enterStoryWorld == GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(normalAttackClip);

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.angularSpeed = 1000;
        isAttack = false;
        isSkillAttack = false;
        base.NormalAttack();
        if (enemySpawn.currentEnemyList.Count != 0) UpdateTarget();
    }

    // 첫번째에 targetTransform을 넣고 currentEnemyList에서 targetTransform을 가장 가까운 transform을 count 크기만큼 가지는 array를 return하는 함수
    Transform[] Set_AttackTarget(Transform targetTransform, List<GameObject> currentEnemyList, int count)
    {
        if (currentEnemyList.Count == 0) return null;

        List<GameObject> enemyList = new List<GameObject>();
        for(int i = 0; i < currentEnemyList.Count; i++)
        {
            enemyList.Add(currentEnemyList[i]);
        }
        Transform[] targetArray = new Transform[count];
        targetArray[0] = targetTransform;
        enemyList.Remove(targetTransform.gameObject);

        float shortDistance = 150f;
        GameObject targetObject = null;

        for (int i = 1; i < count; i++) // 위에서 array에 targetTransform을 넣었으니 i가 1부타 시작
        {
            if(enemyList.Count != 0 && !target.gameObject.CompareTag("Tower") &&  !target.gameObject.CompareTag("Boss"))
            {
                foreach (GameObject enemyObject in enemyList)
                {
                    if (enemyObject != null)
                    {
                        float distanceToEnemy = Vector3.Distance(targetTransform.position, enemyObject.transform.position);
                        if (distanceToEnemy < shortDistance)
                        {
                            shortDistance = distanceToEnemy;
                            targetObject = enemyObject; 
                        }
                    }
                }
                shortDistance = 150f;
                if (targetObject != null)
                {
                    targetArray[i] = targetObject.transform;
                    enemyList.Remove(targetObject);
                }
            }
            else targetArray[i] = targetTransform;
        }
        return targetArray;
    }


    // 이벤트

    // 스킬 빈도 증가
    public void SkillPercentUp()
    {
        specialAttackPercent += 20;
    }

    // 패시브 강화
    public void ReinforcePassive()
    {
        redPassiveFigure = 0.2f;
        bluePassiveFigure = new Vector2(60, 3);
        yellowPassiveFigure = new Vector2(4, 1);
        greenPassiveFigure = 1.5f;
        orangePassiveFigure = 3;
        violetPassiveFigure = new Vector3(50, 3, 400);
    }

    public void HitThrowWeapon(Enemy enemy, AttackWeapon attackWeapon)
    {
        Hit_Passive(enemy);

        if (attackWeapon.isSkill) enemy.OnDamage(skillDamage);
        else AttackEnemy(enemy);

        Destroy(attackWeapon.gameObject);
    }
}
