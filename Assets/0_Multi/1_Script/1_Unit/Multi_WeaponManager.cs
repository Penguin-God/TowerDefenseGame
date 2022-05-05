using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Arrow,
    Spear,
    Mageball,
    MageSkill,
}

public class Multi_WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject[] arrows;
    [SerializeField] GameObject[] spears;
    [SerializeField] GameObject[] mageballs;
    [SerializeField] GameObject[] mageSkills;

    public static string GroupName => "Weapon";
    public static string ArrowPath => "Weapon/Arrows";
    public static string SpearPath => "Weapon/Spears";
    public static string MageballPath => "Weapon/Mageballs";
    public static string MageSkillPath => "Weapon/MageSkills";

    static Dictionary<WeaponType, string> pathByWeaponType = new Dictionary<WeaponType, string>();
    public static string BuildPath(WeaponType weaponType, string weaponName) => BuildPath(pathByWeaponType[weaponType], weaponName);
    static string BuildPath(string path, string weaponName) => $"{path}/{weaponName}";

    void Start()
    {
        pathByWeaponType.Clear();
        pathByWeaponType.Add(WeaponType.Arrow, ArrowPath);
        pathByWeaponType.Add(WeaponType.Spear, SpearPath);
        pathByWeaponType.Add(WeaponType.Mageball, MageballPath);
        pathByWeaponType.Add(WeaponType.MageSkill, MageSkillPath);

        CreateWeaponPool(arrows, ArrowPath, 15);
        CreateWeaponPool(spears, SpearPath, 5);
        CreateWeaponPool(mageballs, MageballPath, 5);
        CreateWeaponPool(mageSkills, MageSkillPath, 3);
    }

    void CreateWeaponPool(GameObject[] _weapons, string _path, int count)
    {
        foreach (var weapon in _weapons)
        {
            string _weaponPath = $"{_path}/{weapon.name}";
            Multi_Managers.Pool.CreatePool(weapon, _weaponPath, count, GroupName);
        }
    }
}
