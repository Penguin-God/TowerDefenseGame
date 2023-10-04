using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public enum SkillEffectType
{
    Meteor,
    IceCloud,
    YellowMagicCircle,
    GreenEnergyBall,
    BlackEnergyBall,
    OrangeWater,
    PosionCloud,
}

public class ResourcesPathBuilder
{
    static Dictionary<int, string> _numberByMonsterName = new Dictionary<int, string>()
    {
        {3, "Swordman" },
        {0, "Archer" },
        {2, "Spearman" },
        {1, "Mage" },
    };
    public string BuildMonsterPath(int monsterNumber) => $"Enemy/Normal/Enemy_{_numberByMonsterName[monsterNumber]} 1";
    public string BuildBossMonsterPath(int monsterNumber) => $"Enemy/Boss/Boss_Enemy_{_numberByMonsterName[monsterNumber]} 1";
    public string BuildEnemyTowerPath(int towerLevel) => $"Enemy/Tower/Lvl{towerLevel}_Twoer";

    public string BuildUnitPath(UnitFlags flag)
    {
        if(flag.UnitClass == UnitClass.Archer || flag.UnitClass == UnitClass.Swordman || flag.UnitClass == UnitClass.Spearman)
            return $"Unit/{GetClassName(flag.UnitClass)}/{Enum.GetName(typeof(UnitColor), flag.UnitColor)}{GetClassName(flag.UnitClass)}";
        else if(flag.UnitClass == UnitClass.Mage && NewMages.Contains( flag.UnitColor))
            return $"Unit/{GetClassName(flag.UnitClass)}/{Enum.GetName(typeof(UnitColor), flag.UnitColor)}{GetClassName(flag.UnitClass)}";
        else
            return $"Unit/{GetClassName(flag.UnitClass)}/{Enum.GetName(typeof(UnitColor), flag.UnitColor)}_{GetClassName(flag.UnitClass)} 1";
    }
    IReadOnlyList<UnitColor> NewMages = new List<UnitColor>() { UnitColor.Yellow, UnitColor.Violet, UnitColor.Orange, UnitColor.Black, UnitColor.Blue, UnitColor.Red};
    string GetClassName(UnitClass unitClass) => Enum.GetName(typeof(UnitClass), unitClass);

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
    public string BuildUnitWeaponPath(UnitFlags flag) => $"Weapon/{_unitClassByWeaponFolderName[flag.UnitClass]}/{BuildUnitWeaponName(flag)}";

    string BuildUnitWeaponName(UnitFlags flag) => $"{Enum.GetName(typeof(UnitColor), flag.UnitColor)}{ _unitClassByWeaponName[flag.UnitClass]}";

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

    public string BuildEffectPath(SkillEffectType effectType)
    {
        string effectName = "";
        switch (effectType)
        {
            case SkillEffectType.Meteor: // effectName = "Meteor 1"; break;
            case SkillEffectType.IceCloud: effectName = "IceCloud"; break;
            case SkillEffectType.YellowMagicCircle: effectName = "YellowMagicCircle"; break;
            case SkillEffectType.GreenEnergyBall: // effectName = "GreenMage BounceBall 1"; break;
            case SkillEffectType.BlackEnergyBall: effectName = "BlackEnergyBall"; break;
            case SkillEffectType.OrangeWater: effectName = "OrangeFountain"; break;
            case SkillEffectType.PosionCloud: effectName = "PosionCloud"; break;
        }
        return $"Effects/{effectName}";
    }

    string GetColorText(UnitColor color) => Enum.GetName(typeof(UnitColor), color);

    public string BuildMagicSpaerPath(UnitColor color) => $"Weapon/MagicSpear/{GetColorText(color)}_MagicSpear";
}