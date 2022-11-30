using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class EffectInitializer : MonoBehaviourPun
{
    public void SettingEffect(IEnumerable<UserSkill> userSkills)
    {
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

    Dictionary<UnitFlags, List<TargetTracker>> _flagByTrackers = new Dictionary<UnitFlags, List<TargetTracker>>();
    Dictionary<Transform, TargetTracker> _targetByTrackers = new Dictionary<Transform, TargetTracker>();

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
                Multi_Managers.Effect.TrackingToTarget("UnitReinForceEffect", target, Vector3.zero);
            }
        }
        else
            targets.ForEach(x => Multi_Managers.Effect.StopTargetTracking(x));
    }
}
