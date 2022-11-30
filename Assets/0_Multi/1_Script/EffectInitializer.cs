using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class EffectInitializer : MonoBehaviourPun
{
    Dictionary<UnitColor, Color32> _unitColorByColor;
    public void SettingEffect(IEnumerable<UserSkill> userSkills)
    {
        _unitColorByColor = new Dictionary<UnitColor, Color32>()
        {
            {UnitColor.red, new Color32(255, 44, 0, 255) },
            {UnitColor.blue, new Color32(26, 251, 255, 255) },
            {UnitColor.black, new Color32(0, 0, 0, 255) },
        };
        foreach (var skill in userSkills)
        {
            if (skill is Taegeuk)
            {
                print("일단 태극");
                var taegeuk = skill as Taegeuk;
                taegeuk.OnUnitSkillFlagChanged += TaeguekEffect_RPC;
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
            foreach (var target in targets)
            {
                if (Multi_Managers.Effect.TargetByTrackers.ContainsKey(target))
                    continue;
                SetUnitReinforceEffect(target.GetComponent<Multi_TeamSoldier>().unitColor, target);
            }
        }
        else
            targets.ForEach(x => Multi_Managers.Effect.StopTargetTracking(x));
    }

    void SetUnitReinforceEffect(UnitColor unitColor, Transform target)
    {
        var tracker = Multi_Managers.Effect.TrackingToTarget("UnitReinForceEffect", target, Vector3.zero);
        foreach (Transform effect in tracker.transform)
        {
            var main = effect.GetComponent<ParticleSystem>().main;
            main.startColor = new ParticleSystem.MinMaxGradient(_unitColorByColor[unitColor]);
        }
    }
}
