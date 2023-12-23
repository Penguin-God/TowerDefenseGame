using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class TrapCreator : MonoBehaviourPun
{
    MonsterPathLocationFinder _locationFinder;
    readonly Vector3 Offset = new Vector3(0, 6, 0);
    const float TrapRange = 5;

    void Awake()
    {
        _locationFinder = new MonsterPathLocationFinder(MultiData.instance.GetEnemyTurnPoints(PlayerIdManager.Id).Select(x => x.position).ToArray());
        _traps = new MultiData<AreaSlowApplier[]>();
    }

    public void CreateTraps(int trapCount, byte id)
    {
        photonView.RPC(nameof(CreateTrap), RpcTarget.All, id, (byte)trapCount);
    }

    [PunRPC]
    void CreateTrap(byte id, byte trapCount)
    {
        _traps.SetData(id, new AreaSlowApplier[trapCount]);
        for (int i = 0; i < trapCount; i++)
        {
            _traps.GetData(id)[i] = Managers.Resources.Instantiate("SlowTrap").GetComponent<AreaSlowApplier>();
            _traps.GetData(id)[i].gameObject.SetActive(false);
        }
    }

    MultiData<AreaSlowApplier[]> _traps;
    public void SpawnTraps(float slowIntensity, byte id)
    {
        for (int i = 0; i < _traps.GetData(id).Length; i++)
            photonView.RPC(nameof(SpawnTrap), RpcTarget.All, id, (byte)i, slowIntensity, _locationFinder.CalculateMonsterPathLocation());
    }
    
    [PunRPC]
    void SpawnTrap(byte id, byte index, float slowIntensity, Vector3 pos)
    {
        var trap = _traps.GetData(id)[index];
        trap.gameObject.SetActive(false);
        trap.transform.position = pos + Offset;
        trap.gameObject.SetActive(true);
        trap.Inject(slowIntensity, TrapRange);
    }
}
