using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnitKeyBuilder
{
    public string BuildAttackKey(UnitFlags flag) => FormatKey(BuildUnitKey("At", flag)); // Attakc

    public string BuildBossAttackKey(UnitFlags flag) => FormatKey(BuildUnitKey("BAt", flag)); // BossAttack

    public IEnumerable<string> BuildPassiveKeys(UnitFlags flag, int passiveCount) => Enumerable.Range(0, passiveCount).Select(i => FormatKey($"{BuildUnitKey("Pa", flag)}{i}"));

    public IEnumerable<string> BuildAllKeys(UnitFlags flag, int passiveCount) => BuildPassiveKeys(flag, passiveCount).Append(BuildAttackKey(flag)).Append(BuildBossAttackKey(flag));

    string FormatKey(string key) => "{%" + key + "}";
    string BuildUnitKey(string prefix, UnitFlags flag) => $"{prefix}{FlagToNumberText(flag)}";
    string FlagToNumberText(UnitFlags flag) => $"{flag.ColorNumber}{flag.ClassNumber}";
}

public static class TextUtility
{
    public static string RelpaceKeyToValue(string text)
    {
        foreach (var flag in UnitFlags.AllFlags)
            text = UnitKeyToValue(text, flag);
        return text;
    }

    public static string UnitKeyToValue(string text, UnitFlags flag)
    {
        foreach (var key in new UnitKeyBuilder().BuildAllKeys(flag, GetUnitPassiveCount(flag)))
            text = text.Replace(key, GetValue(key));
        return text;
    }
    static int GetUnitPassiveCount(UnitFlags flag) => Managers.Data.GetUnitPassiveStats(flag).Count();

    static string UnCapsuleKeyFormat(string key) => key.Substring(2, key.Length - 3);

    static float GetUnitPassiveStat(UnitFlags flag, int index)
    {
        if(flag.UnitColor == UnitColor.Red && index == 1)
            return Managers.Data.GetUnitPassiveStats(flag)[index] * 100;
        return Managers.Data.GetUnitPassiveStats(flag)[index];
    }

    static string GetValue(string key)
    {
        var keyAttribute = UnCapsuleKeyFormat(key);
        if (keyAttribute.StartsWith("At"))
            return Managers.Data.Unit.UnitStatByFlag[KeyToFlag(keyAttribute, "At")].Damage.ToString("#,##0");
        if (keyAttribute.StartsWith("BAt"))
            return Managers.Data.Unit.UnitStatByFlag[KeyToFlag(keyAttribute, "BAt")].BossDamage.ToString("#,##0");
        else if (keyAttribute.StartsWith("Pa"))
            return GetUnitPassiveStat(KeyToFlag(keyAttribute, "Pa"), int.Parse(keyAttribute[4].ToString())).ToString("#,##0");

        return "";

        UnitFlags KeyToFlag(string key, string keyType)
        {
            string[] values = key.Skip(keyType.Length).Select(x => x.ToString()).ToArray();
            return new UnitFlags(int.Parse(values[0]), int.Parse(values[1]));
        }
    }
}
