using System.Collections.Generic;
using System.Linq;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        var container = new BattleDIContainer(gameObject);
        container.AddComponent<GameMatchmaker>();

        container.AddService(LoadPlayerData());
        container.AddService(container.GetService<PlayerDataManager>().SkillInventroy);
        IEnumerable<UserSkill> userSkillDatas = LoadSkillData<UserSkillData>("UserSkillData").Select(x => x.CreateUserSkill());


        container.AddService(new SkillDataGetter(LoadSkillData<SkillUpgradeData>("SkillUpgradeData"), container.GetService<PlayerDataManager>().SkillInventroy, LoadSkillData<SkillLevelData>("SkillLevelData")));
        container.AddService(new SkillUpgradeUseCase(container.GetService<SkillDataGetter>(), container.GetService<PlayerDataManager>()));
        container.AddService(new SkillDrawController(userSkillDatas, container.GetService<SkillDataGetter>(), new SkillRewardData(300, 200))); // TODO : 하드코딩 치우기

        container.AddService(new IAPController(Managers.Resources.LoadCsv<IAP_ProductData>("LobbyShopData/IAPData"), container.GetService<PlayerDataManager>()));

        // Managers.Resources.DependencyInject(new PoolManager("@PoolManager"));
        Managers.Sound.StopBgm(); // 로비 BGM 뭐하지?

        Managers.UI.ShowSceneUI<UI_Lobby>().DependencyInject(container);

        gameObject.AddComponent<LobbyTestHelper>().SetContainer(container);
        gameObject.AddComponent<GameSaver>().SetData(container.GetService<PlayerDataManager>());

    }

    IEnumerable<T> LoadSkillData<T>(string path) => Managers.Resources.LoadCsv<T>($"SkillData/{path}");

    PlayerDataManager LoadPlayerData()
    {
        if (new PlayerPrefabsLoder().Load(out var result))
            return result;
        else
        {
            var inventory = new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>());
            inventory.AddSkill(SkillType.네크로맨서);
            inventory.AddSkill(SkillType.태극스킬);
            inventory.AddSkill(SkillType.마나물약);
            inventory.AddSkill(SkillType.마나변이);
            inventory.AddSkill(SkillType.컬러마스터);
            inventory.AddSkill(SkillType.최대유닛증가);
            inventory.AddSkill(SkillType.거인학살자);
            return new PlayerDataManager(inventory, 0, 0, 0, SkillType.None, SkillType.None);
        }
    }
}
