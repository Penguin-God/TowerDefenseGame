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
        Managers.Unit.Init(new UnitControllerAttacher().AttacherUnitController(MultiServiceMidiator.Instance.gameObject), Managers.Data);
        new WorldInitializer(gameObject).Init();

        // Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").gameObject.SetActive(false);
        gameObject.AddComponent<Tutorial_AI>();
        Multi_GameManager.Instance.CreateOtherPlayerData(SkillType.검은유닛강화, SkillType.판매보상증가);

        SetPlayerSkill();
        InitTutorial();
    }

    void SetPlayerSkill()
    {
        Managers.ClientData.GetExp(SkillType.태극스킬, 1);
        Managers.ClientData.GetExp(SkillType.판매보상증가, 1);
        Managers.ClientData.EquipSkillManager.ChangedEquipSkill(UserSkillClass.Main, SkillType.태극스킬);
        Managers.ClientData.EquipSkillManager.ChangedEquipSkill(UserSkillClass.Sub, SkillType.판매보상증가);
        FindObjectOfType<EffectInitializer>().SettingEffect(new UserSkillInitializer().InitUserSkill(gameObject.GetComponent<BattleDIContainer>()));
    }

    void InitTutorial()
    {
        gameObject.AddComponent<Tutorial_Basic>();
        gameObject.AddComponent<Tutorial_OtherPlayer>();
        gameObject.AddComponent<Tutorial_Tower>();
        gameObject.AddComponent<Tutorial_Boss>();
        gameObject.AddComponent<Tutorial_Combine>();
        // gameObject.AddComponent<Tutorial_UserSkill>();
    }
}
