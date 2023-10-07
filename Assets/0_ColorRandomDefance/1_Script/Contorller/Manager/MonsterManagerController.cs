using System.Collections;
using System.Collections.Generic;

public class MonsterManagerController
{
    readonly WorldObjectManager<Multi_NormalEnemy> _normalMonsterManager = new();

    BattleEventDispatcher _eventDispatcher;
    public MonsterManagerController(BattleEventDispatcher eventDispatcher) => _eventDispatcher = eventDispatcher;

    public void AddNormalMonster(Multi_NormalEnemy monster)
    {
        _normalMonsterManager.AddObject(monster, monster.UsingId);
        NotifyNormalMonsterCountChange(monster);
    }
    public void RemoveNormalMonster(Multi_NormalEnemy monster)
    {
        _normalMonsterManager.RemoveObject(monster, monster.UsingId);
        NotifyNormalMonsterCountChange(monster);
        if (monster.UsingId == PlayerIdManager.Id) // 지금은 담당 월드의 몬스터가 죽은 경우만 알림
            _eventDispatcher.NotifyMonsterDead(monster);
    }

    public IEnumerable<Multi_NormalEnemy> GetNormalMonsters(byte id) => _normalMonsterManager.GetList(id);

    void NotifyNormalMonsterCountChange(Multi_NormalEnemy monster) 
        => _eventDispatcher.NotifyMonsterCountChange(monster.UsingId, _normalMonsterManager.GetCount(monster.UsingId));
}