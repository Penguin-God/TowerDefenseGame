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
        
        Managers.Data.Init();
        IMultiSkillDataCreater multiSkillDataCreater;
        multiSkillDataCreater = PhotonNetwork.CurrentRoom.PlayerCount == 1 ? new TestBattleSkillDataCreater() : gameObject.AddComponent<MultiBattleSkillDataCreater>();
        multiSkillDataCreater.CreateMultiSKillData();
        StartCoroutine(Co_InitGame(multiSkillDataCreater));
    }

    BattleDIContainer _battleDIContainer;
    public BattleDIContainer GetBattleContainer() => _battleDIContainer;

    IEnumerator Co_InitGame(IMultiSkillDataCreater multiSkillDataCreater)
    {
        yield return new WaitUntil(() => multiSkillDataCreater.SKillDataCreateDone());
        InitGame(multiSkillDataCreater.GetMultiSkillData());
    }
    void InitGame(MultiData<SkillBattleDataContainer> multiData)
    {
        MultiServiceMidiator.Instance.Init();
        _battleDIContainer = new BattleDIContainer(gameObject);
        new WorldInitializer(_battleDIContainer).Init(multiData);
        GetComponentInChildren<GameReactionInitailizer>().InitReaction(_battleDIContainer);
    }

    public override void Clear()
    {
        EventIdManager.Clear();
    }
}

class WorldInitializer
{
    BattleDIContainer _battleDIContainer;
    public WorldInitializer(GameObject go)
    {
        _battleDIContainer = new BattleDIContainer(go);
    }

    public WorldInitializer(BattleDIContainer container)
    {
        _battleDIContainer = container;
    }

    public BattleDIContainer Init(MultiData<SkillBattleDataContainer> multiSkillData)
    {
        InitObjectPools();
        new BattleDIContainerInitializer().InjectBattleDependency(_battleDIContainer, multiSkillData);

        Managers.Camera.EnterBattleScene();
        BindUnitEvent();
        return _battleDIContainer;
    }

    void InitObjectPools()
    {
        var poolManager = new PoolManager("@PoolManager");
        Managers.Resources.DependencyInject(poolManager);
        Managers.Multi.DependencyInject(poolManager);

        new WeaponPoolCreator().InitPool(poolManager);
        if (PhotonNetwork.IsMasterClient == false) return;
        
        new UnitPoolInitializer(poolManager).InitPool();
        new MonsterPoolInitializer(poolManager).InitPool();
        new EffectPoolInitializer().InitPool(poolManager);
    }

    void BindUnitEvent()
    {
        Managers.Unit.OnCombine += new UnitPassiveHandler().AddYellowSwordmanCombineGold;
    }
}
