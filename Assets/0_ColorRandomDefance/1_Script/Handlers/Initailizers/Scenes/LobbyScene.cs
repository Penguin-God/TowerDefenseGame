using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        var container = new BattleDIContainer(gameObject);
        container.AddComponent<GameMatchmaker>();

        // 여기 null 부분 채우기
        container.AddService(new PlayerDataManager(new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>())));
        container.AddService(new SkillDrawer(null));
        container.AddService(new SkillDrawUseCase(container.GetService<SkillDrawer>(), container.GetService<PlayerDataManager>(), null));

        // Screen.SetResolution(1920, 1080, true);
        // _isFullScreen = true;
        // Managers.Resources.DependencyInject(new PoolManager("@PoolManager"));
        Managers.Sound.StopBgm(); // 로비 BGM 뭐하지?

        Managers.UI.ShowSceneUI<UI_Lobby>().DependencyInject(container);
    }

    bool _isFullScreen;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_isFullScreen)
            {
                Screen.SetResolution(800, 480, false);
                _isFullScreen = false;
            }
            else
            {
                Screen.SetResolution(1920, 1080, true);
                _isFullScreen = true;
            }
        }
    }
}
