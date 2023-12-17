using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        var container = new BattleDIContainer(gameObject);
        container.AddComponent<GameMatchmaker>();

        container.AddService(new PlayerDataManager(new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>()), 0, 0));
        IEnumerable<UserSkill> userSkillDatas = Managers.Resources.LoadCsv<UserSkillData>("SkillData/UserSkillData").Select(x => x.CreateUserSkill());
        container.AddService(new SkillDrawer(userSkillDatas));
        // container.AddService(new SkillDrawUseCase(container.GetService<SkillDrawer>(), container.GetService<PlayerDataManager>(), null));

        // Screen.SetResolution(1920, 1080, true);
        // _isFullScreen = true;
        // Managers.Resources.DependencyInject(new PoolManager("@PoolManager"));
        Managers.Sound.StopBgm(); // 로비 BGM 뭐하지?

        Managers.UI.ShowSceneUI<UI_Lobby>().DependencyInject(container);

        gameObject.AddComponent<LobbyTestHelper>().SetContainer(container);
    }
}
