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

        Multi_SpawnManagers.Instance.Init();
        Show_UI();
        Managers.Camera.EnterBattleScene();
        InitSound();

        if (PhotonNetwork.IsMasterClient == false) return;
        Managers.Pool.Init();
        var multi = Managers.Multi;
        Managers.Effect.Init(multi.Instantiater);
    }

    void Start()
    {
        var skills = InitUserSkill();
        FindObjectOfType<EffectInitializer>().SettingEffect(skills);
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
        Managers.UI.ShowSceneUI<BattleButton_UI>();
    }

    public override void Clear()
    {
        EventIdManager.Clear();
        Managers.Pool.Clear();
    }
}
