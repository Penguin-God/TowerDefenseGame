using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public abstract class CsvParserBase
{
    public static string GetTypeName(FieldInfo info)
    {

        if (info.FieldType.Name.Contains("[]"))
        {
            return info.FieldType.GetElementType().Name;
        }
        else if (info.FieldType.Name.Contains("List"))
        {
            return GetMiddleString(info.FieldType.ToString(), "[", "]");
        }
        return "실패";

        string GetMiddleString(string str, string begin, string end)
        {
            if (string.IsNullOrEmpty(str)) return null;

            string result = null;
            if (str.IndexOf(begin) > -1)
            {
                str = str.Substring(str.IndexOf(begin) + begin.Length);
                if (str.IndexOf(end) > -1) result = str.Substring(0, str.IndexOf(end));
                else result = str;
            }
            return result;
        }
    }

    public static CsvParserBase GetParser(FieldInfo info)
    {
        switch (info.FieldType.Name)
        {
            case nameof(Int32): return new CsvIntParser();
            case nameof(Single): return new CsvFloatParser();
            case nameof(Boolean): return new CsvBooleanParser();
            case nameof(String): return new CsvStringParser();
            case nameof(UnitFlags): return new CsvUnitFlagsParser();
            default: Debug.LogError("Csv 파싱 타입을 찾지 못함"); break;
        }
        return null;
    }

    public abstract void SetValue(object obj, FieldInfo info, string value);
}

class CsvIntParser : CsvParserBase
{
    public override void SetValue(object obj, FieldInfo info, string value)
    {
        Int32.TryParse(value, out int valueInt);
        info.SetValue(obj, valueInt);
    }
}

class CsvFloatParser : CsvParserBase
{
    public override void SetValue(object obj, FieldInfo info, string value)
    {
        float.TryParse(value, out float valueFloat);
        info.SetValue(obj, valueFloat);
    }
}

class CsvStringParser : CsvParserBase
{
    public override void SetValue(object obj, FieldInfo info, string value) => info.SetValue(obj, value);
}

class CsvBooleanParser : CsvParserBase
{
    public override void SetValue(object obj, FieldInfo info, string value) => info.SetValue(obj, value == "True" || value == "TRUE");
}

class CsvUnitFlagsParser : CsvParserBase
{
    public override void SetValue(object obj, FieldInfo info, string value)
    {
        Debug.Assert(value.Split('&').Length == 2, $"UnitFlags 데이터 입력 잘못됨 : {value}");
        info.SetValue(obj, new UnitFlags(value.Split('&')[0], value.Split('&')[1]));
    }
}

// TODO : 구현하기
class CsvArrayParser : CsvParserBase
{
    public override void SetValue(object obj, FieldInfo info, string value)
    {
        info.SetValue(obj, null);
    }
}
