using System.Collections.Generic;
using System.Linq;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        var container = new BattleDIContainer(gameObject);
        container.AddComponent<GameMatchmaker>();

        container.AddService(new PlayerPrefabsLoder().Load());
        container.AddService(container.GetService<PlayerDataManager>().SkillInventroy);
        IEnumerable<UserSkill> userSkillDatas = LoadSkillData<UserSkillData>("UserSkillData").Select(x => x.CreateUserSkill());
        container.AddService(new SkillDrawer(userSkillDatas));

        container.AddService(new SkillDataGetter(LoadSkillData<SkillUpgradeData>("SkillUpgradeData"), LoadSkillData<UserSkillLevelData>("SkillLevelData"), container.GetService<PlayerDataManager>().SkillInventroy));
        container.AddService(new SkillUpgradeUseCase(container.GetService<SkillDataGetter>(), container.GetService<PlayerDataManager>()));

        container.AddService(new IAPController(Managers.Resources.LoadCsv<IAP_ProductData>("LobbyShopData/IAPData"), container.GetService<PlayerDataManager>()));

        // Managers.Resources.DependencyInject(new PoolManager("@PoolManager"));
        Managers.Sound.StopBgm(); // 로비 BGM 뭐하지?

        Managers.UI.ShowSceneUI<UI_Lobby>().DependencyInject(container);

        gameObject.AddComponent<LobbyTestHelper>().SetContainer(container);
        gameObject.AddComponent<GameSaver>().SetData(container.GetService<PlayerDataManager>());

    }

    IEnumerable<T> LoadSkillData<T>(string path) => Managers.Resources.LoadCsv<T>($"SkillData/{path}");
}
