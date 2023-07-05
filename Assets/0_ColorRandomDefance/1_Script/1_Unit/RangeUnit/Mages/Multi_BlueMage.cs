using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_BlueMage : Multi_Unit_Mage
{
    [SerializeField] float passiveSlowPercent;
    [SerializeField] float freezeTime;
    public override void SetMageAwake()
    {
        var passiveStats = Managers.Data.GetUnitPassiveStats(UnitFlags);
        passiveSlowPercent = passiveStats[0];
        GetComponentInChildren<SphereCollider>().radius = passiveStats[1];
        
        freezeTime = skillStats[0];
    }

    protected override void MageSkile()
        => SkillSpawn(transform.position + (Vector3.up * 2)).GetComponent<Multi_HitSkill>().SetHitActoin(enemy => enemy.OnFreeze_RPC(freezeTime));

    protected override void PlaySkillSound() => PlaySound(EffectSoundType.BlueMageSkill);

    [SerializeField] List<Multi_NormalEnemy> _passiveTargets = new List<Multi_NormalEnemy>();
    void FixedUpdate()
    {
        foreach (var enemy in _passiveTargets)
        {
            if(enemy.IsSlow == false)
                enemy.OnSlow(passiveSlowPercent, -1);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        var enemy = other.GetComponentInParent<Multi_NormalEnemy>();
        if (enemy != null)
            _passiveTargets.Add(enemy);
    }

    void OnTriggerExit(Collider other)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        var enemy = other.GetComponentInParent<Multi_NormalEnemy>();
        if (enemy != null && _passiveTargets.Contains(enemy))
        {
            _passiveTargets.Remove(enemy);
            enemy.ExitSlow(RpcTarget.All); // TODO : 나중에 동기화 마스터한테 옮기고 이게 맞는지 확인해보기
        }
    }
}
