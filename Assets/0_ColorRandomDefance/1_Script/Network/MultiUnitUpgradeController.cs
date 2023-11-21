using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;


public class MultiUnitStatController : MonoBehaviourPun
{
    UnitStatController _unitStatCotroller;
    public void DependencyInject(UnitStatController worldUnitDamageManager) => _unitStatCotroller = worldUnitDamageManager;
    public UnitDamageInfo GetDamageInfo(UnitFlags flag) => _unitStatCotroller.GetDamageInfo(flag, Id);

    byte Id => PlayerIdManager.Id;

    public void AddUnitDamage(UnitFlags flag, UnitDamageInfo info)
    {
        UpgradeUnitDamage(flag, info.BaseDamage, info.BaseBossDamage, Id);
        ClientToMaster(nameof(UpgradeUnitDamage), flag, info.BaseDamage, info.BaseBossDamage);
    }

    public void AddUnitDamage(UnitColor color, UnitDamageInfo info)
    {
        UpgradeUnitDamage(color, info.BaseDamage, info.BaseBossDamage, Id);
        ClientToMaster(nameof(UpgradeUnitDamage), color, info.BaseDamage, info.BaseBossDamage);
    }

    public void ScaleUnitDamage(UnitColor color, UnitDamageInfo info)
    {
        ScaleUnitDamage(color, info.DamageRate, info.BossDamageRate, Id);
        ClientToMaster(nameof(ScaleUnitDamage), color, info.DamageRate, info.BossDamageRate);
    }

    public void ScaleAllUnitDamage(UnitDamageInfo info)
    {
        ScaleAllUnitDamage(info.DamageRate, info.BossDamageRate, Id);
        ClientToMaster(nameof(ScaleAllUnitDamage), info.DamageRate, info.BossDamageRate);
    }

    [PunRPC] void UpgradeUnitDamage(UnitFlags flag, int damage, int bossDamage, byte id) => AddUnitDamage(x => x == flag, damage, bossDamage, id);
    [PunRPC] void UpgradeUnitDamage(UnitColor color, int damage, int bossDamage, byte id) => AddUnitDamage(x => x.UnitColor == color, damage, bossDamage, id);
    [PunRPC] void ScaleUnitDamage(UnitColor color, float dmaRate, float bossDamRate, byte id) => ScaleUnitDamage(x => x.UnitColor == color, dmaRate, bossDamRate, id);
    [PunRPC] void ScaleAllUnitDamage(float dmaRate, float bossDamRate, byte id) => ScaleUnitDamage(_ => true, dmaRate, bossDamRate, id);

    void AddUnitDamage(Func<UnitFlags, bool> conditoin, int dam, int bossDam, byte id) => UpgradeUnit(conditoin, new UnitDamageInfo(dam, bossDam, damRate:0), id);
    void ScaleUnitDamage(Func<UnitFlags, bool> conditoin, float damRate, float bossDamRate, byte id) => UpgradeUnit(conditoin, new UnitDamageInfo(damRate: damRate, bossDamRate: bossDamRate), id);
    void UpgradeUnit(Func<UnitFlags, bool> conditoin, UnitDamageInfo info, byte id) => _unitStatCotroller.UpgradeUnitDamage(conditoin, info, id);

    void ClientToMaster(string methodName, params object[] objects)
    {
        if (PhotonNetwork.IsMasterClient) return;
        object[] parmeters = objects.Concat(new object[] { Id }).ToArray();
        photonView.RPC(methodName, RpcTarget.MasterClient, parmeters);
    }
}
