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
        Multi_Managers.Camera.EnterBattleScene();
        InitSound();

        if (PhotonNetwork.IsMasterClient == false) return;
        Multi_Managers.Pool.Init();
    }

    void Start()
    {
        Multi_Managers.Skill.Init();
    }

    void InitSound()
    {
        var sound = Multi_Managers.Sound;
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
        Multi_Managers.UI.Init();

        Multi_Managers.UI.ShowPopupUI<BackGround>("BackGround");
        Multi_Managers.UI.ShowPopupUI<CombineResultText>("CombineResultText");
        Multi_Managers.UI.ShowPopupUI<WarningText>();
        Multi_Managers.UI.ShowPopupUI<RandomShop_UI>("InGameShop/Random Shop");

        Multi_Managers.UI.ShowSceneUI<Status_UI>();
        Multi_Managers.UI.ShowSceneUI<BattleButton_UI>();
    }

    public override void Clear()
    {
        EventIdManager.Clear();
        Multi_Managers.Pool.Clear();
    }
}
