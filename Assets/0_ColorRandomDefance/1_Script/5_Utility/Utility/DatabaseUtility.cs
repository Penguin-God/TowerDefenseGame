using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitKeyBuilder
{
    public string BuildAttackKey(UnitFlags flag) => FormatKey(BuildUnitKey("At", flag));

    public string BuildBossAttackKey(UnitFlags flag) => FormatKey(BuildUnitKey("BAt", flag));

    public IReadOnlyList<string> BuildPassiveKeys(UnitFlags flag, int count) => Enumerable.Range(0, count).Select(i => FormatKey($"{BuildUnitKey("Pa", flag)}{i}")).ToList();

    string FormatKey(string key) => "{%" + key + "}";
    string BuildUnitKey(string prefix, UnitFlags flag) => $"{prefix}{FlagToNumberText(flag)}";
    string FlagToNumberText(UnitFlags flag) => $"{flag.ColorNumber}{flag.ClassNumber}";
}

public static class DatabaseUtility
{
    public static float GetUnitPassiveStat(UnitFlags flag, int index) => Managers.Data.GetUnitPassiveStats(flag)[index];

    public static string GetValueText(string key)
    {
        var values = key.Skip(2).Select(x => int.Parse(x.ToString())).ToArray();
        if (key.StartsWith("Pa"))
            return GetUnitPassiveStat(new UnitFlags(values[0], values[1]), values[2]).ToString();
        return "";
    }

    public static string FormatTextToValue(string key) => GetValueText(key.Substring(2, key.Length - 3));

    public static string TextKeyToValue(string text)
    {
        foreach (var key in Managers.Data.TextKeys.Select(BulidKeyFormat))
            text = text.Replace(key, FormatTextToValue(key));
        return text;

        string BulidKeyFormat(string key) => $"<%{key}>".Replace('<', '{').Replace('>', '}');
    }

    public static string UnitTextKeyToValue(string text, UnitFlags flag)
    {
        return text;
    }
}
