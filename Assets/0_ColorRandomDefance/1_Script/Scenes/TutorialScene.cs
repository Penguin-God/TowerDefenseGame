using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TutorialScene : BaseScene
{
    protected override void Init()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.JoinRandomRoom();

        MultiServiceMidiator.Instance.Init();
        var tutorialSKillData = CreateTutorialSKillData();
        var container = new WorldInitializer(gameObject).Init(tutorialSKillData);
        FindObjectOfType<EffectInitializer>().SettingEffect(new UserSkillInitializer().InitUserSkill(container, tutorialSKillData.GetData(PlayerIdManager.Id)));
        gameObject.AddComponent<Tutorial_AI>();
        
        // SetPlayerSkill();
        InitTutorial(container);
    }

    MultiData<SkillBattleDataContainer> CreateTutorialSKillData()
    {
        var result = new MultiData<SkillBattleDataContainer>();
        result.SetData(PlayerIdManager.Id, CreateSKillData(SkillType.태극스킬, SkillType.판매보상증가));
        result.SetData(PlayerIdManager.EnemyId, CreateSKillData(SkillType.검은유닛강화, SkillType.판매보상증가));
        return result;
    }

    SkillBattleDataContainer CreateSKillData(SkillType main, SkillType sub) => BattleSkillDataCreater.CreateSkillData(main, 1, sub, 1, Managers.Data.UserSkill);

    void InitTutorial(BattleDIContainer container)
    {
        gameObject.AddComponent<Tutorial_Basic>();
        gameObject.AddComponent<Tutorial_OtherPlayer>();
        gameObject.AddComponent<Tutorial_Tower>();
        gameObject.AddComponent<Tutorial_Boss>();
        gameObject.AddComponent<Tutorial_Combine>();
        gameObject.AddComponent<Tutorial_UserSkill>().Injection(container.GetComponent<SwordmanGachaController>());
    }
}
