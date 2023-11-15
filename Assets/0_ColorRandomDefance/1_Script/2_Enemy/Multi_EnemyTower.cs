using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Multi_EnemyTower : Multi_Enemy
{
    [SerializeField] int _level;
    public int Level => _level;
    public BossData TowerData { get; private set; }
    
    public void Setinfo(int level)
    {
        enemyType = EnemyType.Tower;
        _level = level;
        photonView.RPC(nameof(SetLevelText), RpcTarget.All, level);
        TowerData = Managers.Data.TowerDataByLevel[_level];
        SetStatus_RPC(TowerData.Hp);
    }

    void SetStatus_RPC(int hp) => photonView.RPC(nameof(SetStatus), RpcTarget.All, hp, false);

    [PunRPC]
    void SetLevelText(int level) => gameObject.GetComponentInChildren<TextMeshProUGUI>().text = level.ToString();

    [ContextMenu("죽음")]
    void 죽음() => Dead();
}
