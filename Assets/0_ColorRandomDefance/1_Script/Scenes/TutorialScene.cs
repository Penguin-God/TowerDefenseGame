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

        GetComponent<BattleReadyController>().EnterBattle(GetComponent<EnemySpawnNumManager>());
        Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").gameObject.SetActive(false);
        gameObject.AddComponent<Tutorial_AI>();
        Multi_GameManager.Instance.CreateOtherPlayerData(SkillType.검은유닛강화, SkillType.판매보상증가);

        CreatePools();
        SetPlayerSkill();
    }

    void SetPlayerSkill()
    {
        Managers.ClientData.GetExp(SkillType.태극스킬, 1);
        Managers.ClientData.GetExp(SkillType.판매보상증가, 1);
        Managers.ClientData.EquipSkillManager.ChangedEquipSkill(UserSkillClass.Main, SkillType.태극스킬);
        Managers.ClientData.EquipSkillManager.ChangedEquipSkill(UserSkillClass.Sub, SkillType.판매보상증가);
        FindObjectOfType<EffectInitializer>().SettingEffect(new UserSkillInitializer().InitUserSkill(gameObject.GetComponent<BattleDIContainer>()));
    }

    void CreatePools()
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        new UnitPoolInitializer().InitPool();
        new MonsterPoolInitializer().InitPool();
        new WeaponPoolInitializer().InitPool();
    }
}
