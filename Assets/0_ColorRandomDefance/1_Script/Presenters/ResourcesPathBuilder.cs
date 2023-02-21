using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourcesPathBuilder
{
    static Dictionary<int, string> _numberByName = new Dictionary<int, string>()
    {
        {3, "Swordman" },
        {0, "Archer" },
        {2, "Spearman" },
        {1, "Mage" },
    };
    public string BuildMonsterPath(int monsterNumber) => $"Enemy/Normal/Enemy_{_numberByName[monsterNumber]} 1";
    public string BuildBossMonsterPath(int monsterNumber) => $"Enemy/Boss/Boss_Enemy_{_numberByName[monsterNumber]} 1";
    public string BuildEnemyTowerPath(int towerLevel) => $"Enemy/Tower/Lvl{towerLevel}_Twoer";

    public string BuildUnitPath(UnitFlags flag)
        => $"Unit/{Enum.GetName(typeof(UnitClass), flag.UnitClass)}/{Enum.GetName(typeof(UnitColor), flag.UnitColor)}_{Enum.GetName(typeof(UnitClass), flag.UnitClass)} 1";

    static Dictionary<UnitClass, string> _unitClassByWeaponName = new Dictionary<UnitClass, string>()
    {
        {UnitClass.Archer, "ArrowTrail 1" },
        {UnitClass.Spearman, "_Spear 1" },
        {UnitClass.Mage, "MageBall 1" },
    };

    static Dictionary<UnitClass, string> _unitClassByWeaponFolderName = new Dictionary<UnitClass, string>()
    {
        {UnitClass.Archer, "Arrows" },
        {UnitClass.Spearman, "Spears" },
        {UnitClass.Mage, "MageBalls" },
    };
    public string BuildUnitWeaponPath(UnitFlags flag)
        => $"Weapon/{_unitClassByWeaponFolderName[flag.UnitClass]}/{Enum.GetName(typeof(UnitColor), flag.UnitColor)}{ _unitClassByWeaponName[flag.UnitClass]}";

    public string BuildMageSkillEffectPath(UnitColor unitColor)
    {
        string effectName = "";
        switch (unitColor)
        {
            case UnitColor.Red: effectName = "Meteor 1"; break;
            case UnitColor.Blue: effectName = "IceCloud 1"; break;
            case UnitColor.Yellow: effectName = "Yellow Skile Object 1"; break;
            case UnitColor.Green: effectName = "GreenMage BounceBall 1"; break;
            case UnitColor.Orange: effectName = "OrangeMage SkillEffect 1"; break;
            case UnitColor.Violet: effectName = "MagePosionEffect 1"; break;
            case UnitColor.Black: effectName = "BlackMageSkileBall 1"; break;
        }
        return $"Weapon/MageSkills/{effectName}";
    }
}