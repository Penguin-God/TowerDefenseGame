using System.Collections;
using UnityEngine;
using System;

abstract public class MageSkill : MonoBehaviour
{
    [SerializeField] string soundCilpName;

    [SerializeField] protected SphereCollider sphereCollider;
    public float hitTime; // 콜라이더가 켜지기 전 공격 대기 시간

    // 법사 스킬 사용 가능 조건을 만들고 조건을 만족할 때까지 대기하고 무조건 스킬 사용하게 하기
    public void OnSkile(Unit_Mage mage)
    {
        DoSkile = () => MageSkile(mage);
        gameObject.SetActive(true);
        if (sphereCollider != null) StartCoroutine(Co_OnCollider(hitTime));
    }

    public abstract void MageSkile(Unit_Mage mage);

    Action DoSkile;

    private void OnEnable()
    {
        if (DoSkile != null) DoSkile();
    }

    protected IEnumerator Co_OnCollider(float delayTIme)
    {
        yield return new WaitForSeconds(delayTIme);
        if(sphereCollider != null) sphereCollider.enabled = true;
    }

    public virtual void HitSkile(Enemy enemy) { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            HitSkile(enemy);
        }
    }

    //void MageSkile(Enemy enemy)
    //{
    //    switch (teamSoldier.unitColor)
    //    {
    //        case TeamSoldier.UnitColor.red:
    //            enemy.EnemyStern(100, 5);
    //            enemy.OnDamage(400000);
    //            Destroy(transform.parent.gameObject, 3);
    //            break;
    //        case TeamSoldier.UnitColor.blue:
    //            enemy.EnemySlow(99, 5, true);
    //            if (teamSoldier.GetComponent<Unit_Mage>().isUltimate) enemy.OnDamage(20000);
    //            break;
    //        case TeamSoldier.UnitColor.yellow:
    //            break;
    //        case TeamSoldier.UnitColor.green:
    //            break;
    //        case TeamSoldier.UnitColor.orange:
    //            break;
    //        case TeamSoldier.UnitColor.violet:
    //            enemy.EnemyPoisonAttack(25, 8, 0.3f, 120000);
    //            break;
    //    }
    //}
}