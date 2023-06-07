using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

        MultiServiceMidiator.Instance.Init();
        Managers.Unit.Init(new UnitControllerAttacher().AttacherUnitController(MultiServiceMidiator.Instance.gameObject), Managers.Data);

        new WorldInitializer(monoBehaviourContainer).Init();
        CreatePools();
        GetComponent<BattleReadyController>().EnterBattle(monoBehaviourContainer.GetComponent<EnemySpawnNumManager>());
    }

    void CreatePools()
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        new UnitPoolInitializer().InitPool();
        new MonsterPoolInitializer().InitPool();
        new WeaponPoolInitializer().InitPool();
    }

    public override void Clear()
    {
        EventIdManager.Clear();
        Managers.Pool.Clear();
    }
}

class UserSkillInitializer
{
    public IEnumerable<UserSkill> InitUserSkill(BattleDIContainer container)
    {
        List<UserSkill> userSkills = new List<UserSkill>();
        foreach (var skillType in Managers.ClientData.EquipSkillManager.EquipSkills)
        {
            if (skillType == SkillType.None)
                continue;
            var userSkill = UserSkillFactory.CreateUserSkill(skillType, container);
            userSkill.InitSkill();
            userSkills.Add(userSkill);
        }
        return userSkills;
    }
}

class WorldInitializer
{
    GameObject monoBehaviourContainer;
    BattleDIContainer _battleDIContainer;
    public WorldInitializer(GameObject go)
    {
        monoBehaviourContainer = go;
    }

    public void Init()
    {
        _battleDIContainer = monoBehaviourContainer.AddComponent<BattleDIContainer>();
        new MultiInitializer().InjectionBattleDependency(_battleDIContainer);

        InitMonoBehaviourContainer();
        Show_UI();
        Managers.Camera.EnterBattleScene();
        InitSound();
        Managers.Pool.Init();
        InitEffect();
        SetUnit();
    }

    void SetUnit()
    {
        Multi_SpawnManagers.NormalUnit.OnSpawn += Managers.Unit.AddUnit;
        Managers.Unit.OnCombine += new UnitPassiveController().AddYellowSwordmanCombineGold;
    }

    void InitMonoBehaviourContainer()
    {
        monoBehaviourContainer.AddComponent<UnitColorChangerRpcHandler>();
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
        Managers.UI.ShowPopupUI<CombineResultText>("CombineResultText");
        Managers.UI.ShowSceneUI<BattleButton_UI>().SetInfo(_battleDIContainer.GetService<SwordmanGachaController>());

        var enemySelector = Managers.UI.ShowSceneUI<UI_EnemySelector>();
        enemySelector.SetInfo(monoBehaviourContainer.GetOrAddComponent<EnemySpawnNumManager>());
        Managers.Camera.OnIsLookMyWolrd += (isLookMy) => enemySelector.gameObject.SetActive(!isLookMy);
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
