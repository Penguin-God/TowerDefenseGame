using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitKeyBuilder
{
    public string BuildAttackKey(UnitFlags flag) => FormatKey(BuildUnitKey("At", flag));

    public string BuildBossAttackKey(UnitFlags flag) => FormatKey(BuildUnitKey("BAt", flag));

    public IEnumerable<string> BuildPassiveKeys(UnitFlags flag, int passiveCount) => Enumerable.Range(0, passiveCount).Select(i => FormatKey($"{BuildUnitKey("Pa", flag)}{i}"));

    public IEnumerable<string> BuildAllKeys(UnitFlags flag, int passiveCount) => BuildPassiveKeys(flag, passiveCount).Append(BuildAttackKey(flag)).Append(BuildBossAttackKey(flag));

    string FormatKey(string key) => "{%" + key + "}";
    string BuildUnitKey(string prefix, UnitFlags flag) => $"{prefix}{FlagToNumberText(flag)}";
    string FlagToNumberText(UnitFlags flag) => $"{flag.ColorNumber}{flag.ClassNumber}";
}

public static class DatabaseUtility
{
    public static float GetUnitPassiveStat(UnitFlags flag, int index) => Managers.Data.GetUnitPassiveStats(flag)[index];

    public static string UnitKeyToValue(string key)
    {
        var unitKey = UnCapsuleKeyFormat(key);
        if (unitKey.StartsWith("At"))
            return "";
        if (unitKey.StartsWith("BAt"))
            return "";
        else if (unitKey.StartsWith("Pa"))
            return GetUnitPassiveStat(GetFlag(unitKey, 2), int.Parse(unitKey[4].ToString())).ToString();
        return "";

        UnitFlags GetFlag(string key, int skipIndex)
        {
            string[] values = key.Skip(skipIndex).Select(x => x.ToString()).ToArray();
            return new UnitFlags(int.Parse(values[0]), int.Parse(values[1]));
        }
    }

    static string UnCapsuleKeyFormat(string key) => key.Substring(2, key.Length - 3);

    public static string UnitKeyToValue(string text, UnitFlags flag)
    {
        Debug.Log(GetUnitPassiveCount(flag));
        foreach (var key in new UnitKeyBuilder().BuildAllKeys(flag, GetUnitPassiveCount(flag)))
            text = text.Replace(key, UnitKeyToValue(key));
        return text;
    }
    static int GetUnitPassiveCount(UnitFlags flag) => Managers.Data.GetUnitPassiveStats(flag).Count();
}
