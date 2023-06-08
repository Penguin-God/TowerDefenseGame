using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitKeyData
{
    public UnitFlags UnitFlag { get; private set; }
    public UnitKeyData(UnitFlags flag, int passiveStatCount)
    {
        UnitFlag = flag;

        _passiveKeys = new string[passiveStatCount];
        for (int i = 0; i < passiveStatCount; i++)
            _passiveKeys[i] = $"Pa{flag.ColorNumber}{flag.ClassNumber}{i}";
    }

    string[] _passiveKeys;

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

    public static string TextKeyToValue(string text, UnitFlags flag)
    {

    }
}
