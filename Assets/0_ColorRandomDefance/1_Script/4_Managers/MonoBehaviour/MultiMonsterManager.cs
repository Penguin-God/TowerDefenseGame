using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class MultiMonsterManager : MonoBehaviourPun, IMonsterManager
{
    MultiData<MonsterManager> _mulitMonsterManager;
    public void Init()
    {
        _mulitMonsterManager = MultiDataFactory.CreateMultiData<MonsterManager>();
    }

    public void AddNormalMonster(Multi_NormalEnemy multi_NormalEnemy)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Multi_NormalEnemy> GetNormalMonsters()
    {
        throw new NotImplementedException();
    }

    public void RegisterMonsterCountChange(Action<int> OnCountChange)
    {
        throw new NotImplementedException();
    }

    [PunRPC]
    void AddNormalMonster(int viewId)
    {

    }
}
