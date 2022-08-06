using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_EnemyTowerSpawner : MonoBehaviourPun
{
    [SerializeField] GameObject[] towerPrefabs;
    [SerializeField] GameObject[] towers;
    [SerializeField] Vector3 SpawnPos;

    int[] arr_TowersHp;

    public Multi_EnemyTower currentTower = null;
    private int currentTowerLevel = 0;
    void RespawnTower()
    {
        // 처음에는 0
        currentTower = towers[currentTowerLevel].GetComponent<Multi_EnemyTower>();

        int _hp = arr_TowersHp[currentTowerLevel - 1];
        currentTower.photonView.RPC("Setup", RpcTarget.All, _hp, 0f);

        if (currentTower != null)
        {
            SetDeadAction();
            currentTower.photonView.RPC("Spawn", RpcTarget.All, SpawnPos);
        }
    }


    // 후에 유닛 동기화하고 죽음 구현하면서 수정해야함
    void SetDeadAction()
    {
        // tower 레밸에 따라 다음 타워 소환할지 안할지 결정
        if (currentTowerLevel < towers.Length) currentTower.OnDeath += () => StartCoroutine(Co_AfterRespawnTower(1.5f));
        else currentTower.OnDeath += () => StartCoroutine(ClearLastTower());

        currentTower.OnDeath += () => SoundManager.instance.PlayEffectSound_ByName("TowerDieClip");
        // currentTower.OnDeath += () => Multi_GameManager.instance.OnEventShop(currentTowerLevel, TriggerType.EnemyTower);
    }

    IEnumerator Co_AfterRespawnTower(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        RespawnTower();
    }

    // 마지막 성 클리어 시 
    public CreateDefenser createDefenser;
    IEnumerator ClearLastTower() // 검은 창병 두마리 소환 후 모든 유닛 필드로 옮기기
    {
        yield return new WaitForSeconds(0.1f); // 상점 이용 후 유닛이동하기 위해서 대기
        for (int i = 0; i < 2; i++) createDefenser.CreateSoldier(6, 2);
        UnitManager.instance.UnitTranslate_To_EnterStroyMode();
    }
}
