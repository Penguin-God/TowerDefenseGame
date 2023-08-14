using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class EffectInitializer : MonoBehaviourPun
{
    UnitReinforceEffectDrawer _unitReinforceEffectDrawer = new UnitReinforceEffectDrawer();
    public void SettingEffect(IEnumerable<UserSkill> userSkills)
    {
        foreach (var skill in userSkills)
        {
            var taegeuk = skill as TaegeukController;
            if (taegeuk != null)
                taegeuk.OnTaegeukDamageChanged += TaeguekEffect_RPC;

            if (skill.UserSkillBattleData.SkillType == SkillType.흑의결속)
                Managers.Unit.OnUnitCountChangeByFlag += TrackingBalckUnit;

            if (skill.UserSkillBattleData.SkillType == SkillType.백의결속)
                Managers.Unit.OnUnitCountChangeByFlag += TrackingWhiteUnit;
        }
    }

    void TaeguekEffect_RPC(UnitClass unitClass, bool isTaegeukOn)
        => photonView.RPC(nameof(TaeguekEffect), RpcTarget.MasterClient, unitClass, isTaegeukOn, PlayerIdManager.Id);

    [PunRPC]
    void TaeguekEffect(UnitClass unitClass, bool isTaegeukOn, byte id)
    {
        IEnumerable<UnitFlags> TeaguekFlags = new UnitFlags[] { new UnitFlags(UnitColor.Red, unitClass), new UnitFlags(UnitColor.Blue, unitClass) };
        
        if (isTaegeukOn)
        {
            foreach (var flag in TeaguekFlags)
                SetUnitTrackingEffects(flag, id);
        }
        else
        {
            List<Transform> targets = GetTeaguekUnits();
            foreach (var target in targets)
            {
                _unitReinforceEffectDrawer.CancleTracking(target); // master
                photonView.RPC(nameof(StopTracking), RpcTarget.Others, target.GetComponent<PhotonView>().ViewID); // client
            }
        }

        List<Transform> GetTeaguekUnits()
        {
            List<Transform> targets = new List<Transform>();
            foreach (var flag in TeaguekFlags)
                targets = targets.Concat(MultiServiceMidiator.Server.GetUnits(id).Where(x => x.UnitFlags == flag).Select(x => x.transform)).ToList();
            return targets;
        }
    }

    void TrackingBalckUnit(UnitFlags flag, int count)
    {
        if (flag.UnitColor == UnitColor.Black && count > 0)
            SetUnitTrackingEffects_RPC(flag);
    }

    void TrackingWhiteUnit(UnitFlags flag, int count)
    {
        if (flag.UnitColor == UnitColor.White && count > 0)
            SetUnitTrackingEffects_RPC(flag);
    }

    void SetUnitTrackingEffects_RPC(UnitFlags flag)
        => photonView.RPC(nameof(SetUnitTrackingEffects), RpcTarget.MasterClient, flag, PlayerIdManager.Id);

    [PunRPC]
    void SetUnitTrackingEffects(UnitFlags flag, byte id)
    {
        var targets = MultiServiceMidiator.Server.GetUnits(id).Where(x => x.UnitFlags == flag && _unitReinforceEffectDrawer.IsTracking(x.transform) == false);
        
        foreach (var target in targets)
        {
            _unitReinforceEffectDrawer.SetUnitReinforceEffect(target);
            photonView.RPC(nameof(SetUnitTrackingEffects_ByID), RpcTarget.Others, target.GetComponent<PhotonView>().ViewID);

            target.OnDead -= (dieUnit) => photonView.RPC(nameof(StopTracking), RpcTarget.All, dieUnit.GetComponent<PhotonView>().ViewID);
            target.OnDead += (dieUnit) => photonView.RPC(nameof(StopTracking), RpcTarget.All, dieUnit.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    void SetUnitTrackingEffects_ByID(int viewID)
        => _unitReinforceEffectDrawer.SetUnitReinforceEffect(Managers.Multi.GetPhotonViewTransfrom(viewID).GetComponent<Multi_TeamSoldier>());

    [PunRPC] void StopTracking(int viewID) => _unitReinforceEffectDrawer.CancleTracking(Managers.Multi.GetPhotonViewTransfrom(viewID));
}

class UnitReinforceEffectDrawer
{
    Dictionary<Transform, TargetTracker> _targetByTrackers = new Dictionary<Transform, TargetTracker>();
    public bool IsTracking(Transform target) => _targetByTrackers.ContainsKey(target);

    Dictionary<UnitColor, Color32> _unitColorByColor = new Dictionary<UnitColor, Color32>()
    {
        {UnitColor.Red, new Color32(255, 44, 0, 255) },
        {UnitColor.Blue, new Color32(26, 251, 255, 255) },
        {UnitColor.White, new Color32(255, 255, 255, 255) },
        {UnitColor.Black, new Color32(0, 0, 0, 255) },
    };

    public void SetUnitReinforceEffect(Multi_TeamSoldier target)
    {
        var tracker = Managers.Effect.TrackingTarget(GetUnitTarckerName(target.UnitColor), target.transform, Vector3.zero);
        _targetByTrackers.Add(target.transform, tracker);
        foreach (Transform effect in tracker.transform)
        {
            var main = effect.GetComponent<ParticleSystem>().main;
            main.startColor = new ParticleSystem.MinMaxGradient(_unitColorByColor[target.UnitColor]);
        }
    }

    public void CancleTracking(Transform target)
    {
        if (IsTracking(target) == false) return;
        Managers.Effect.CancleTracking(_targetByTrackers[target]);
        _targetByTrackers.Remove(target);
    }

    string GetUnitTarckerName(UnitColor unitColor)
    {
        if (unitColor == UnitColor.Black) return "BalckableUnitReingForceEffect";
        return "UnitReinForceEffect";
    }
}
