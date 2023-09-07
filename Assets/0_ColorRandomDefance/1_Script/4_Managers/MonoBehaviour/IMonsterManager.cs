using System.Collections;
using System.Collections.Generic;

public interface IMonsterManager
{
    public void AddNormalMonster(Multi_NormalEnemy multi_NormalEnemy);
    public void RemoveNormalMonster(Multi_NormalEnemy monster);
    public IReadOnlyList<Multi_NormalEnemy> GetNormalMonsters();
}
