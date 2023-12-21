﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        var container = new BattleDIContainer(gameObject);
        container.AddComponent<GameMatchmaker>();

        // container.AddService(new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>()));
        // container.AddService(new PlayerDataManager(container.GetService<SkillInventroy>(), 0, 0, 0));
        container.AddService(new PlayerPrefabsLoder().Load());
        container.AddService(container.GetService<PlayerDataManager>().SkillInventroy);
        IEnumerable<UserSkill> userSkillDatas = Managers.Resources.LoadCsv<UserSkillData>("SkillData/UserSkillData").Select(x => x.CreateUserSkill());
        container.AddService(new SkillDrawer(userSkillDatas));

        container.AddService(new SkillDataGetter(Managers.Resources.LoadCsv<SkillUpgradeData>("SkillData/SkillUpgradeData"), Managers.Resources.LoadCsv<UserSkillLevelData>("SkillData/SkillLevelData"), container.GetService<PlayerDataManager>().SkillInventroy));
        container.AddService(new SkillUpgradeUseCase(container.GetService<SkillDataGetter>(), container.GetService<PlayerDataManager>()));
        container.AddService(new EquipSkillManager());
        // Screen.SetResolution(1920, 1080, true);
        // _isFullScreen = true;
        // Managers.Resources.DependencyInject(new PoolManager("@PoolManager"));
        Managers.Sound.StopBgm(); // 로비 BGM 뭐하지?

        Managers.UI.ShowSceneUI<UI_Lobby>().DependencyInject(container);

        gameObject.AddComponent<LobbyTestHelper>().SetContainer(container);
    }

    IEnumerable<T> LoadSkillData<T>(string path) => Managers.Resources.LoadCsv<T>($"SkillData/{path}");
}
