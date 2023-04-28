using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class BattleScene : BaseScene
{
    [SerializeField] GameObject monoBehaviourContainer;

    protected override void Init()
    {
        if (PhotonNetwork.InRoom == false)
        {
            print("방에 없누 ㅋㅋ");
            return;
        }
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        new WorldInitializer(monoBehaviourContainer).Init();
        CreatePools();
    }

    void CreatePools()
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        new UnitPoolInitializer().InitPool();
        new MonsterPoolInitializer().InitPool();
        new WeaponPoolInitializer().InitPool();
    }

    void Start()
    {
        FindObjectOfType<EffectInitializer>().SettingEffect(new UserSkillInitializer().InitUserSkill());
    }

    public override void Clear()
    {
        EventIdManager.Clear();
        Managers.Pool.Clear();
    }
}

class UserSkillInitializer
{
    public IEnumerable<UserSkill> InitUserSkill()
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
}

class WorldInitializer
{
    GameObject monoBehaviourContainer;

    public WorldInitializer(GameObject go)
    {
        monoBehaviourContainer = go;
    }

    public void Init()
    {
        InitMonoBehaviourContainer();
        Multi_SpawnManagers.Instance.Init();
        Show_UI();
        Managers.Camera.EnterBattleScene();
        InitSound();
        Managers.Pool.Init();
        InitEffect();
        SetUnit();
    }

    void SetUnit()
    {
        Multi_SpawnManagers.NormalUnit.OnSpawn += Multi_UnitManager.Instance.AddUnit;
        Multi_UnitManager.Instance.OnCombine += new UnitPassiveController().AddYellowSwordmanCombineGold;
    }

    void InitMonoBehaviourContainer()
    {
        monoBehaviourContainer.AddComponent<MonsterSpawnerContorller>();
        monoBehaviourContainer.AddComponent<UnitColorChangerRpcHandler>();
        monoBehaviourContainer.AddComponent<WinOrLossController>();
        monoBehaviourContainer.AddComponent<OpponentStatusSynchronizer>();
    }

    void InitSound()
    {
        var sound = Managers.Sound;
        // 빼기
        Multi_SpawnManagers.BossEnemy.rpcOnSpawn -= () => sound.PlayBgm(BgmType.Boss);
        Multi_SpawnManagers.BossEnemy.rpcOnDead -= () => sound.PlayBgm(BgmType.Default);

        Multi_SpawnManagers.BossEnemy.rpcOnDead -= () => sound.PlayEffect(EffectSoundType.BossDeadClip);
        Multi_SpawnManagers.TowerEnemy.rpcOnDead -= () => sound.PlayEffect(EffectSoundType.TowerDieClip);
        StageManager.Instance.OnUpdateStage -= (stage) => sound.PlayEffect(EffectSoundType.NewStageClip);

        // 더하기
        Multi_SpawnManagers.BossEnemy.rpcOnSpawn += () => sound.PlayBgm(BgmType.Boss);
        Multi_SpawnManagers.BossEnemy.rpcOnDead += () => sound.PlayBgm(BgmType.Default);

        Multi_SpawnManagers.BossEnemy.rpcOnDead += () => sound.PlayEffect(EffectSoundType.BossDeadClip);
        Multi_SpawnManagers.TowerEnemy.rpcOnDead += () => sound.PlayEffect(EffectSoundType.TowerDieClip);
        StageManager.Instance.OnUpdateStage += (stage) => sound.PlayEffect(EffectSoundType.NewStageClip);
    }

    void Show_UI()
    {
        Managers.UI.ShowPopupUI<BackGround>("BackGround").gameObject.SetActive(false);
        Managers.UI.ShowPopupUI<CombineResultText>("CombineResultText");

        Managers.UI.ShowSceneUI<UI_Status>();

        var buttons = Managers.UI.ShowSceneUI<BattleButton_UI>();
        buttons.GetComponentInChildren<UI_EnemySelector>(true).SetInfo(monoBehaviourContainer.GetOrAddComponent<EnemySpawnNumManager>());
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
