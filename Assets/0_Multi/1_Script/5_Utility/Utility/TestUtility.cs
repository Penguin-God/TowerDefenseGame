using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ProjectileData
{
    [SerializeField] Multi_Projectile original;
    [SerializeField] Transform spawnPos;
    private Action<Multi_Enemy> hitAction;
    public void SetHitAction(Action<Multi_Enemy> action) => hitAction = action;

    public ProjectileData(Multi_Projectile original, Transform spawnPos, Action<Multi_Enemy> hitAction)
    {
        this.original = original;
        this.spawnPos = spawnPos;
        this.hitAction = hitAction;
    }

    public Multi_Projectile Original => original;
    public Vector3 SpawnPos => spawnPos.position;
    public Action<Multi_Enemy> HitAction => hitAction;
}

public class TestUtility : MonoBehaviour
{
    [SerializeField] ProjectileData data;

    void Start()
    {
        data.SetHitAction(null);
    }

    [ContextMenu("Test UsedWeapon")]
    void TestUsedWeapon()
    {
        ShotProjectile(data, transform.forward);
    }

    protected Multi_Projectile ShotProjectile(ProjectileData data, Vector3 dir)
    {
        Multi_Projectile UseWeapon = Multi_SpawnManagers.Weapon.Spawn(data.Original.gameObject, data.SpawnPos).GetComponent<Multi_Projectile>();
        UseWeapon.Shot(dir, data.HitAction);

        return UseWeapon;
    }
}
