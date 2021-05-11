using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    EnemySpawn enemySpawn;
    // 상태 변수
    public float speed;
    public int maxHp;
    public int currentHp;
    public bool isDead;
    public Slider hpSlider;

    // 이동, 회전 관련 변수
    private Transform parent;
    private Rigidbody parentRigidbody;
    private Transform wayPoint;
    public Vector3 dir;
    private int pointIndex = -1;

    private void Awake()
    {
        enemySpawn = GetComponentInParent<EnemySpawn>();
        parent = transform.parent.GetComponent<Transform>();
        parentRigidbody = GetComponentInParent<Rigidbody>();
    }

    void OnEnable() // 리스폰 시 상태 초기화
    {
        if (this.GetComponent<EnemyTower>() != null) return; // 타워는 X
        enemySpawn.currentEnemyList.Add(this.gameObject);
        isDead = false;
        SetNextPoint();
    }

    public void ResetStatus(int hp, float speed)
    {
        maxHp = hp;
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        this.speed = speed;
    }

    void SetNextPoint()
    {
        pointIndex++;
        if (pointIndex >= TurnPoint.enemyTurnPoints.Length) pointIndex = 0; // 무한반복을 위한 조건

        // 실제 이동을 위한 속도 설정
        wayPoint = TurnPoint.enemyTurnPoints[pointIndex];
        dir = (wayPoint.position - parent.transform.position).normalized;
        parentRigidbody.velocity = dir * speed;
    }

    void SetTransfrom()
    {
        transform.rotation = Quaternion.Euler(0, -90 * pointIndex, 0);
        parent.transform.position = wayPoint.position;
    }


    // 대미지 관련 함수
    public void OnDamage(int damage, TeamSoldier teamSoldier)
    {
        currentHp -= damage;
        hpSlider.value = currentHp;
        if (currentHp <= 0)
        {
            Dead();
        }
        else OnUnitPassive(teamSoldier);
    }

    // 모든 유닛의 패시브
    void OnUnitPassive(TeamSoldier teamSoldier)
    {
        if(teamSoldier == null)
        {
            Debug.Log("TeamSoldier가 널이다ㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏ");
            return;
        }
        switch (teamSoldier.unitType)
        {
            case TeamSoldier.Type.sowrdman:
                SwordmanPassive(teamSoldier);
                break;
            case TeamSoldier.Type.archer:
                ArcherPassive(teamSoldier);
                break;
            case TeamSoldier.Type.spearman:
                SpearmanPassive(teamSoldier);
                break;
            case TeamSoldier.Type.mage:
                MagePassive(teamSoldier);
                break;
        }
    }

    void SwordmanPassive(TeamSoldier teamSoldier)
    {
        switch (teamSoldier.unitColor)
        {
            case TeamSoldier.UnitColor.red:
                break;
            case TeamSoldier.UnitColor.blue:
                break;
            case TeamSoldier.UnitColor.yellow:
                break;
            case TeamSoldier.UnitColor.green:
                break;
            case TeamSoldier.UnitColor.orange:
                break;
            case TeamSoldier.UnitColor.violet:
                break;
        }
    }

    void ArcherPassive(TeamSoldier teamSoldier)
    {
        switch (teamSoldier.unitColor)
        {
            case TeamSoldier.UnitColor.red:
                break;
            case TeamSoldier.UnitColor.blue:
                break;
            case TeamSoldier.UnitColor.yellow:
                break;
            case TeamSoldier.UnitColor.green:
                break;
            case TeamSoldier.UnitColor.orange:
                break;
            case TeamSoldier.UnitColor.violet:
                Stern(5, 2);
                break;
        }
    }

    void SpearmanPassive(TeamSoldier teamSoldier)
    {
        switch (teamSoldier.unitColor)
        {
            case TeamSoldier.UnitColor.red:
                break;
            case TeamSoldier.UnitColor.blue:
                break;
            case TeamSoldier.UnitColor.yellow:
                break;
            case TeamSoldier.UnitColor.green:
                break;
            case TeamSoldier.UnitColor.orange:
                break;
            case TeamSoldier.UnitColor.violet:
                break;
        }
    }

    void MagePassive(TeamSoldier teamSoldier)
    {
        switch (teamSoldier.unitColor)
        {
            case TeamSoldier.UnitColor.red:
                break;
            case TeamSoldier.UnitColor.blue:
                break;
            case TeamSoldier.UnitColor.yellow:
                break;
            case TeamSoldier.UnitColor.green:
                break;
            case TeamSoldier.UnitColor.orange:
                break;
            case TeamSoldier.UnitColor.violet:
                break;
        }
    }

    void Stern(int sternPercent, float sternTime)
    {
        int random = Random.Range(0, 100);
        if (random < sternPercent)
        {
            StopAllCoroutines();
            StartCoroutine(SternCoroutine(sternTime));
        }
    }

    IEnumerator SternCoroutine(float sternTime)
    {
        parentRigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(sternTime);
        parentRigidbody.velocity = dir * speed;
    }

    void Dead()
    {
        parent.gameObject.SetActive(false);
        parent.position = new Vector3(500, 500, 500);
        enemySpawn.currentEnemyList.Remove(this.gameObject);
        ResetVariable();

        GameManager.instance.Gold += 1;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold); // 돈벌기 추가
    }

    void ResetVariable()
    {
        pointIndex = -1;
        maxHp = 0;
        currentHp = 0;
        hpSlider.maxValue = 0;
        hpSlider.value = 0;
        isDead = true;
        this.speed = 0;
    }

    //private TeamSoldier teamSoldier;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "WayPoint")
        {
            SetTransfrom();
            SetNextPoint();
        }
        else if(other.tag == "Attack") // 임시
        {
            AttackWeapon attackWeapon = other.GetComponentInParent<AttackWeapon>();
            TeamSoldier teamSoldier = attackWeapon.attackUnit.GetComponent<TeamSoldier>();
            if (teamSoldier.unitType == TeamSoldier.Type.archer) Destroy(other.gameObject); // 아처 공격이면 총알 삭제

            OnDamage(attackWeapon.damage, teamSoldier);
        }
    }
}
