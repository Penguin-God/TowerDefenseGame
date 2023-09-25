using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSkillController
{
    public abstract void DoSkill();
    protected void PlaySkillSound(EffectSoundType type) => Managers.Sound.PlayEffect(type);
    protected GameObject SpawnSkill(SkillEffectType type, Vector3 spawnPos) => Managers.Resources.Instantiate(new ResourcesPathBuilder().BuildEffectPath(type), spawnPos);
}

public class GainGoldController : UnitSkillController
{
    readonly int AddGold;
    readonly Transform _transform;
    readonly byte OwerId;
    readonly Vector3 OffSet = new Vector3(0, 0.6f, 0);
    public GainGoldController(Transform transform, int addGold, byte owerId)
    {
        AddGold = addGold;
        _transform = transform;
        OwerId = owerId;
    }

    public override void DoSkill()
    {
        SpawnSkill(SkillEffectType.YellowMagicCircle, _transform.position + OffSet);
        Debug.Log(OwerId);
        if(PhotonNetwork.IsMasterClient)
            Multi_GameManager.Instance.AddGold_RPC(AddGold, OwerId);
    }
}