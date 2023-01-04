using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattleScene : BaseScene
{
    protected override void Init()
    {
        if (PhotonNetwork.InRoom == false)
        {
            print("방에 없누 ㅋㅋ");
            return;
        }
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        new WorldInitializer().Init();
    }

    void Start()
    {
        FindObjectOfType<EffectInitializer>().SettingEffect(InitUserSkill());
    }

    IEnumerable<UserSkill> InitUserSkill()
    {
        List<UserSkill> userSkills = new List<UserSkill>();
        foreach (var skillType in Managers.ClientData.EquipSkillManager.EquipSkills)
        {
            if (skillType == SkillType.None)
                continue;
            var userSkill = new UserSkillFactory().GetSkill(skillType);
            userSkill.InitSkill();
            userSkills.Add(userSkill);
        }
        return userSkills;
    }

    public override void Clear()
    {
        EventIdManager.Clear();
        Managers.Pool.Clear();
    }
}

class WorldInitializer
{
    GameObject monoBehaviourContainer;

    public void Init()
    {
        InitMonoBehaviourContainer();
        Multi_SpawnManagers.Instance.Init();
        Multi_SpawnManagers.NormalEnemy.SetInfo(monoBehaviourContainer.GetComponent<EnemySpawnNumManager>());
        Show_UI();
        Managers.Camera.EnterBattleScene();
        InitSound();
        Managers.Pool.Init();
        InitEffect();
        EventInit();
    }

    void InitMonoBehaviourContainer()
    {
        monoBehaviourContainer = new GameObject("Create MonoBehaviour Container");
        monoBehaviourContainer.AddComponent<PhotonView>();
        var numManager = monoBehaviourContainer.AddComponent<EnemySpawnNumManager>();
        monoBehaviourContainer.AddComponent<StageMonsterSpawner>().SetInfo(numManager);
    }

    void EventInit()
    {
        Multi_StageManager.Instance.OnUpdateStage += monoBehaviourContainer.GetComponent<StageMonsterSpawner>().StageSpawn;
    }

    void InitSound()
    {
        var sound = Managers.Sound;
        // 빼기
        Multi_SpawnManagers.BossEnemy.rpcOnSpawn -= () => sound.PlayBgm(BgmType.Boss);
        Multi_SpawnManagers.BossEnemy.rpcOnDead -= () => sound.PlayBgm(BgmType.Default);

        Multi_SpawnManagers.BossEnemy.rpcOnDead -= () => sound.PlayEffect(EffectSoundType.BossDeadClip);
        Multi_SpawnManagers.TowerEnemy.rpcOnDead -= () => sound.PlayEffect(EffectSoundType.TowerDieClip);
        Multi_StageManager.Instance.OnUpdateStage -= (stage) => sound.PlayEffect(EffectSoundType.NewStageClip);

        // 더하기
        Multi_SpawnManagers.BossEnemy.rpcOnSpawn += () => sound.PlayBgm(BgmType.Boss);
        Multi_SpawnManagers.BossEnemy.rpcOnDead += () => sound.PlayBgm(BgmType.Default);

        Multi_SpawnManagers.BossEnemy.rpcOnDead += () => sound.PlayEffect(EffectSoundType.BossDeadClip);
        Multi_SpawnManagers.TowerEnemy.rpcOnDead += () => sound.PlayEffect(EffectSoundType.TowerDieClip);
        Multi_StageManager.Instance.OnUpdateStage += (stage) => sound.PlayEffect(EffectSoundType.NewStageClip);
    }

    void Show_UI()
    {
        Managers.UI.Init();

        Managers.UI.ShowPopupUI<BackGround>("BackGround");
        Managers.UI.ShowPopupUI<CombineResultText>("CombineResultText");
        Managers.UI.ShowPopupUI<WarningText>();
        Managers.UI.ShowPopupUI<RandomShop_UI>("InGameShop/Random Shop");

        Managers.UI.ShowSceneUI<Status_UI>();
        var buttons = Managers.UI.ShowSceneUI<BattleButton_UI>();
        buttons.GetComponentInChildren<UI_EnemySelector>().SetInfo(monoBehaviourContainer.GetComponent<EnemySpawnNumManager>());
    }

    void InitEffect()
    {
        foreach (var data in CsvUtility.CsvToArray<EffectData>(Managers.Resources.Load<TextAsset>("Data/EffectData").text))
        {
            switch (data.EffectType)
            {
                case EffectType.GameObject:
                    Managers.Pool.CreatePool_InGroup(data.Path, 3, "Effects");
                    break;
            }
        }
    }
}
