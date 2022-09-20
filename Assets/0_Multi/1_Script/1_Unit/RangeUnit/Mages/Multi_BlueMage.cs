using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_BlueMage : Multi_Unit_Mage
{
    Multi_BluePassive bluePassive = null;
    [SerializeField] float freezeTime;
    // 패시브
    [SerializeField] float radiusRange;
    [SerializeField] float slowPercent;
    public override void SetMageAwake()
    {
        bluePassive = GetComponent<Multi_BluePassive>();
        // TODO : 데이터 매니저에서 로드하기
        radiusRange = 25;
        slowPercent = 60;

        GetComponentInChildren<SphereCollider>().radius = radiusRange;
        freezeTime = skillStats[0];
    }

    protected override void MageSkile()
        => SkillSpawn(transform.position + (Vector3.up * 2)).GetComponent<Multi_HitSkill>().SetHitActoin(enemy => enemy.OnFreeze_RPC(freezeTime));

    protected override void PlaySkillSound() => PlaySound(EffectSoundType.BlueMageSkill);

    private void OnTriggerStay(Collider other)
    {
        if (pv.IsMine == false) return;

        if (other.GetComponentInParent<Multi_NormalEnemy>() != null) // 나가기 전까진 무한 슬로우
            other.GetComponentInParent<Multi_NormalEnemy>().OnSlow_RPC(slowPercent, -1);
    }

    private void OnTriggerExit(Collider other)
    {
        if (pv.IsMine == false) return;

        if (other.GetComponentInParent<Multi_NormalEnemy>() != null)
            other.GetComponentInParent<Multi_NormalEnemy>().ExitSlow(RpcTarget.All); // TODO : 나중에 동기화 마스터한테 옮기고 이게 맞는지 확인해보기
    }
}
