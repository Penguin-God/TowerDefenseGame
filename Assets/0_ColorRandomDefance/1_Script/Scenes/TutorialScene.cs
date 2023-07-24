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
        var contaner = new WorldInitializer(gameObject)
            .Init(BattleSkillDataCreater.CreateSkillData(SkillType.검은유닛강화, 1, SkillType.판매보상증가, 1, Managers.Data.UserSkill));
        gameObject.AddComponent<Tutorial_AI>();
        
        // SetPlayerSkill();
        InitTutorial(contaner);
    }

    void SetPlayerSkill()
    {
        Managers.ClientData.GetExp(SkillType.태극스킬, 1);
        Managers.ClientData.GetExp(SkillType.판매보상증가, 1);
        Managers.ClientData.EquipSkillManager.ChangedEquipSkill(UserSkillClass.Main, SkillType.태극스킬);
        Managers.ClientData.EquipSkillManager.ChangedEquipSkill(UserSkillClass.Sub, SkillType.판매보상증가);
        FindObjectOfType<EffectInitializer>().SettingEffect(new UserSkillInitializer().InitUserSkill(gameObject.GetComponent<BattleDIContainer>()));
    }

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
