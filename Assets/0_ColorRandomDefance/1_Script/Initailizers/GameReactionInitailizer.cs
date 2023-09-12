using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameReactionInitailizer : MonoBehaviour
{
    public void InitReaction(BattleDIContainer container)
    {
        new UnitCombineNotifier(Managers.Unit, container.GetComponent<TextShowAndHideController>());
        gameObject.AddComponent<UnitClickController>();
        gameObject.AddComponent<WinOrLossController>().Inject(container.GetEventDispatcher(), container.GetComponent<TextShowAndHideController>());
        gameObject.AddComponent<OpponentStatusSender>().Init(container.GetEventDispatcher());
        gameObject.AddComponent<BuildingClickContoller>()
            .Inject(container.GetService<BattleUI_Mediator>(), Managers.UI, container.GetService<BuyAction>(), container.GetService<GoodsBuyController>());
        gameObject.AddComponent<RewradController>().Inject(container.GetComponent<Multi_BossEnemySpawner>());
        GameStart(container);
    }

    void GameStart(BattleDIContainer container)
    {
        var starter = gameObject.AddComponent<BattleStartController>();
        starter.Inject(container.GetEventDispatcher(), container.GetService<BattleUI_Mediator>());
        starter.EnterBattle(container.GetComponent<EnemySpawnNumManager>(), container.GetMultiActiveSkillData().GetData(PlayerIdManager.EnemyId));
    }
}
