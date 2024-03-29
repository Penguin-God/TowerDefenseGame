using UnityEngine;

public class GameReactionInitailizer : MonoBehaviour
{
    public void InitReaction(BattleDIContainer container)
    {
        container.GetEventDispatcher().OnUnitCombine += new UnitPassiveHandler().AddYellowSwordmanCombineGold;
        gameObject.AddComponent<UnitClickController>();
        gameObject.AddComponent<WinOrLossController>().Inject(container.GetEventDispatcher(), container.GetComponent<TextShowAndHideController>());
        gameObject.AddComponent<OpponentStatusSender>().Init(container.GetEventDispatcher());
        container.Inject(gameObject.AddComponent<BuildingClickContoller>());
        //gameObject.AddComponent<BuildingClickContoller>()
        //    .Inject(container.GetService<BattleUI_Mediator>(), container.GetService<BuyAction>(), container.GetService<GoodsBuyController>());
        GameStart(container);
    }

    void GameStart(BattleDIContainer container)
    {
        var starter = gameObject.AddComponent<BattleStartController>();
        starter.Inject(container.GetEventDispatcher(), container.GetService<BattleUI_Mediator>());
        starter.EnterBattle(container.GetComponent<EnemySpawnNumManager>(), container.GetMultiActiveSkillData().GetData(PlayerIdManager.EnemyId));
    }
}
