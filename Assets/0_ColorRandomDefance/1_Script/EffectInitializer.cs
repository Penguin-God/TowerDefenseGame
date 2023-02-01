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
            var taegeuk = skill as Taegeuk;
            if (taegeuk != null)
                taegeuk.OnTaegeukDamageChanged += TaeguekEffect_RPC;

            var blackUnitUp = skill as BlackUnitUpgrade;
            if (blackUnitUp != null)
                blackUnitUp.OnBlackUnitReinforce += SetUnitTrackingEffects_RPC;
        }
    }

    void TaeguekEffect_RPC(UnitClass unitClass, bool isTaegeukOn)
        => photonView.RPC(nameof(TaeguekEffect), RpcTarget.MasterClient, unitClass, isTaegeukOn, Multi_Data.instance.Id);

    [PunRPC]
    void TaeguekEffect(UnitClass unitClass, bool isTaegeukOn, int id)
    {
        IReadOnlyList<UnitFlags> TeaguekUnitFlags = new UnitFlags[] { new UnitFlags(UnitColor.Red, unitClass), new UnitFlags(UnitColor.Blue, unitClass) };
        
        if (isTaegeukOn)
        {
            foreach (var flag in TeaguekUnitFlags)
                SetUnitTrackingEffects(flag, id);
        }
        else
        {
            List<Transform> targets = GetTeaguekUnits();
            targets.ForEach(x => Managers.Effect.StopTargetTracking(x));
            foreach (var target in targets)
                photonView.RPC(nameof(StopTracking), RpcTarget.Others, target.GetComponent<PhotonView>().ViewID);
        }

        List<Transform> GetTeaguekUnits()
        {
            List<Transform> targets = new List<Transform>();
            foreach (var flag in TeaguekUnitFlags)
                targets = targets.Concat(Multi_UnitManager.Instance.Master.GetUnitList(id, flag).Select(x => x.transform)).ToList();
            return targets;
        }
    }

    void SetUnitTrackingEffects_RPC(UnitFlags flag)
        => photonView.RPC(nameof(SetUnitTrackingEffects), RpcTarget.MasterClient, flag, Multi_Data.instance.Id);

    [PunRPC]
    void SetUnitTrackingEffects(UnitFlags flag, int id)
    {
        var targets = Multi_UnitManager.Instance.Master.GetUnitList(id, flag)
            .Where(x => Managers.Effect.TargetByTrackers.ContainsKey(x.transform) == false);

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

    [PunRPC]
    void StopTracking(int viewID)
        => Managers.Effect.StopTargetTracking(Managers.Multi.GetPhotonViewTransfrom(viewID));
}

class UnitReinforceEffectDrawer
{
    Dictionary<UnitColor, Color32> _unitColorByColor = new Dictionary<UnitColor, Color32>()
    {
        {UnitColor.Red, new Color32(255, 44, 0, 255) },
        {UnitColor.Blue, new Color32(26, 251, 255, 255) },
        { UnitColor.Black, new Color32(0, 0, 0, 255) },
    };

    public void SetUnitReinforceEffect(Multi_TeamSoldier target)
    {
        var tracker = Managers.Effect.TrackingToTarget(GetUnitTarckerName(target.unitColor), target.transform, Vector3.zero);
        foreach (Transform effect in tracker.transform)
        {
            var main = effect.GetComponent<ParticleSystem>().main;
            main.startColor = new ParticleSystem.MinMaxGradient(_unitColorByColor[target.unitColor]);
        }
    }

    string GetUnitTarckerName(UnitColor unitColor)
    {
        if (unitColor == UnitColor.Black) return "BalckableUnitReingForceEffect";
        return "UnitReinForceEffect";
    }
}
