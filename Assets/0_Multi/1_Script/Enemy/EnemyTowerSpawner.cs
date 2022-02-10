using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyTowerSpawner : MonoBehaviourPun
{
    [SerializeField] GameObject[] towerPrefabs;
    [SerializeField] GameObject[] towers;
    [SerializeField] Vector3 SpawnPos;

    int[] arr_TowersHp;
    void Awake()
    {
        SpawnPos = Multi_Data.instance.EnemyTowerSpawnPos;

        towers = new GameObject[towerPrefabs.Length];
        for(int i = 0; i < towerPrefabs.Length; i++)
        {
            GameObject _newTower = PhotonNetwork.Instantiate(towerPrefabs[i].name, Vector3.one * 400, Quaternion.identity);
            _newTower.transform.SetParent(Multi_Data.instance.EnemyTowerParent);
            towers[i] = _newTower;
        }
        Set_EnemyTowerHpDictionary();
        Multi_GameManager.instance.OnStart += () => arr_TowersHp = Dic_enemyTowerHp[Multi_GameManager.instance.Difficult];
        Multi_GameManager.instance.OnStart += () => RespawnTower();
    }

    private Dictionary<string, int[]> Dic_enemyTowerHp;
    void Set_EnemyTowerHpDictionary() // key : 난이도, value : 레벨 1~6 까지 적군의 성 체력
    {
        Dic_enemyTowerHp = new Dictionary<string, int[]>
        {
            { "Baby", new int[] { 40000, 80000, 300000, 800000, 2000000, 10000000 } },
            { "Easy", new int[] { 80000, 200000, 600000, 2000000, 6000000, 20000000 } },
            { "Normal", new int[] { 600000, 2000000, 6000000, 20000000, 60000000, 100000000 } },
            { "Hard", new int[] { 1000000, 2400000, 8000000, 30000000, 80000000, 300000000 } },
            { "Impossiable", new int[] { 1500000, 4000000, 15000000, 40000000, 140000000, 500000000 } },
        };
    }


    public Multi_EnemyTower currentTower = null;
    private int currentTowerLevel = 0;
    void RespawnTower()
    {
        // 처음에는 0
        currentTower = towers[currentTowerLevel].GetComponent<Multi_EnemyTower>();
        currentTowerLevel = currentTower.level;

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
        currentTower.OnDeath += () => Multi_GameManager.instance.OnEventShop(currentTowerLevel, TriggerType.EnemyTower);
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
