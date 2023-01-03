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
            if (skill is Taegeuk)
            {
                var taegeuk = skill as Taegeuk;
                taegeuk.OnUnitSkillFlagChanged += TaeguekEffect_RPC;
            }

            if(skill is BlackUnitUpgrade)
            {
                var blackUnitUp = skill as BlackUnitUpgrade;
                blackUnitUp.OnBlackUnitReinforce += SetUnitTrackingEffects_RPC;
            }
        }
    }

    void TaeguekEffect_RPC(UnitClass unitClass, bool isTaegeukOn)
        => photonView.RPC(nameof(TaeguekEffect), RpcTarget.MasterClient, unitClass, isTaegeukOn);

    [PunRPC]
    void TaeguekEffect(UnitClass unitClass, bool isTaegeukOn)
    {
        var flags = new UnitFlags[] { new UnitFlags(UnitColor.red, unitClass), new UnitFlags(UnitColor.blue, unitClass) };
        List<Transform> targets = new List<Transform>();
        foreach (var flag in flags)
            targets = targets.Concat(Multi_UnitManager.Instance.Master.GetUnitList(Multi_Data.instance.Id, flag).Select(x => x.transform)).ToList();

        if (isTaegeukOn)
        {
            foreach (var flag in flags)
                SetUnitTrackingEffects(flag);
        }
        else
            targets.ForEach(x => Managers.Effect.StopTargetTracking(x));
    }

    void SetUnitTrackingEffects_RPC(UnitFlags flag)
        => photonView.RPC(nameof(SetUnitTrackingEffects), RpcTarget.MasterClient, flag);

    [PunRPC]
    void SetUnitTrackingEffects(UnitFlags flag)
    {
        var targets = Multi_UnitManager.Instance.Master.GetUnitList(Multi_Data.instance.Id, flag)
            .Where(x => Managers.Effect.TargetByTrackers.ContainsKey(x.transform) == false);
        foreach (var target in targets)
        {
            _unitReinforceEffectDrawer.SetUnitReinforceEffect(target);
            photonView.RPC(nameof(SetUnitTrackingEffects_ByID), RpcTarget.Others, target.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    void SetUnitTrackingEffects_ByID(int viewID)
        => _unitReinforceEffectDrawer.SetUnitReinforceEffect(Managers.Multi.GetPhotonViewTransfrom(viewID).GetComponent<Multi_TeamSoldier>());
}

class UnitReinforceEffectDrawer
{
    Dictionary<UnitColor, Color32> _unitColorByColor = new Dictionary<UnitColor, Color32>()
        {
            {UnitColor.red, new Color32(255, 44, 0, 255) },
            {UnitColor.blue, new Color32(26, 251, 255, 255) },
            { UnitColor.black, new Color32(0, 0, 0, 255) },
        };

    public void SetUnitReinforceEffects(IEnumerable<Multi_TeamSoldier> targets)
    {
        foreach (var target in targets)
            SetUnitReinforceEffect(target);
    }

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
        if (unitColor == UnitColor.black) return "BalckableUnitReingForceEffect";
        return "UnitReinForceEffect";
    }
}
