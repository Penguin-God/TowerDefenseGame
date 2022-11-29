using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    void TaeguekEffect_RPC(UnitClass unitClass, bool isTaegeukOn)
        => photonView.RPC(nameof(TaeguekEffect), RpcTarget.MasterClient, unitClass, isTaegeukOn);

    [PunRPC]
    void TaeguekEffect(UnitClass unitClass, bool isTaegeukOn)
    {
        var flags = new UnitFlags[] { new UnitFlags(UnitColor.red, unitClass), new UnitFlags(UnitColor.blue, unitClass) };

        foreach (var flag in flags)
        {
            if (_flagByTrackers.ContainsKey(flag) == false) continue;
            _flagByTrackers[flag].ForEach(x => Multi_Managers.Pool.Push(x.GetComponent<Poolable>()));
            _flagByTrackers.Remove(flag);
        }
        if (isTaegeukOn == false) return;

        foreach (var flag in flags)
        {   
            var targets = Multi_UnitManager.Instance.Master.GetUnitList(Multi_Data.instance.Id, flag);
            _flagByTrackers.Add(flag, new List<TargetTracker>());
            foreach (var target in targets)
                _flagByTrackers[flag].Add(Multi_Managers.Effect.ChaseToTarget("UnitReinForceEffect", target.transform, Vector3.zero));
        }
    }

}
