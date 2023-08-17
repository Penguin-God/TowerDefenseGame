using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    int layerMask = (1 << 10) | (1 << 11) | (1 << 12) | (1 << 13) | (1 << 8);

    public void ApplySlowToEnemiesInRange(float range, float slowPercent, float slowTime)
    {
        // ���� ��ġ���� ���� �ݰ� ���� ���� �����մϴ�.
        IEnumerable<Multi_NormalEnemy> enemiesInRange = Physics.OverlapSphere(transform.position, range, layerMask).Select(x => x.GetComponent<Multi_NormalEnemy>());

        foreach (var enemy in enemiesInRange)
            enemy?.OnSlowWithTime(slowPercent, slowTime);
    }
}
