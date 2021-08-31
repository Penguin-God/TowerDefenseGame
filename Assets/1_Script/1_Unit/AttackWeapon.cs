using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWeapon : MonoBehaviour
{
    [SerializeField] bool isAOE; // area of effect : 광역 공격
    public bool isSkill;
    public TeamSoldier teamSoldier;
    public GameObject attackUnit; // 무기와 적이 충돌할때 Enemy script에서 관련 정보를 가져가도록 하기위한 변수 
    // attackUnit 변수 TeamSolider Script 변수로 만들기

    private void Start()
    {
        Destroy(gameObject, 5);
        if (attackUnit == null) return;

        teamSoldier = attackUnit.GetComponent<TeamSoldier>();
        isSkill = teamSoldier.isSkillAttack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (teamSoldier.delegate_OnHit != null) teamSoldier.delegate_OnHit(enemy);

            if (!isAOE) Destroy(gameObject);
        }
    }
}
