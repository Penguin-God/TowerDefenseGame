using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterManager
{
    public void AddNormalMonster(Multi_NormalEnemy multi_NormalEnemy);
    public void RemoveNormalMonster(Multi_NormalEnemy monster);
}
