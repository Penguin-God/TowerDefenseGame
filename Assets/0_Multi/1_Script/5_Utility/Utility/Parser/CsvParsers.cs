using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public interface CsvParser
{
    void SetValue(object obj, FieldInfo info, string value);
}

public abstract class CsvParsers
{
    public static string GetTypeName(FieldInfo info)
    {
        if (info.FieldType.Name.Contains("[]"))
        {
            return info.FieldType.GetElementType().Name;
        }
        else if (info.FieldType.Name.Contains("List"))
        {
            return info.FieldType.ToString().GetMiddleString("[", "]").Replace("System.", "");
        }
        return "실패";
    }

    public static CsvParser GetParser(string typeName)
    {
        if (typeName.Contains("[]") || typeName.Contains("List"))
            return new CsvEnumerableParser();

        switch (typeName)
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
    public static CsvParser GetParser(FieldInfo info) => GetParser(info.FieldType.Name);
}

class CsvIntParser : CsvParser
{
    public void SetValue(object obj, FieldInfo info, string value)
    {
        Int32.TryParse(value, out int valueInt);
        info.SetValue(obj, valueInt);
    }
}

class CsvFloatParser : CsvParser
{
    public void SetValue(object obj, FieldInfo info, string value)
    {
        float.TryParse(value, out float valueFloat);
        info.SetValue(obj, valueFloat);
    }
}

class CsvStringParser : CsvParser
{
    public void SetValue(object obj, FieldInfo info, string value) => info.SetValue(obj, value);
}

class CsvBooleanParser : CsvParser
{
    public void SetValue(object obj, FieldInfo info, string value) => info.SetValue(obj, value == "True" || value == "TRUE");
}

class CsvUnitFlagsParser : CsvParser
{
    public void SetValue(object obj, FieldInfo info, string value)
    {
        Debug.Assert(value.Split('&').Length == 2, $"UnitFlags 데이터 입력 잘못됨 : {value}");
        info.SetValue(obj, new UnitFlags(value.Split('&')[0], value.Split('&')[1]));
    }
}

// TODO : 구현하기
class CsvEnumerableParser : CsvParser
{
    public void SetValue(object obj, FieldInfo info, string value)
    {
        CsvParsers.GetParser(GetElementTypeName(info));

        info.SetValue(obj, null);
    }

    string GetElementTypeName(FieldInfo info)
    {
        if (info.FieldType.Name.Contains("[]"))
        {
            return info.FieldType.GetElementType().Name;
        }
        else if (info.FieldType.Name.Contains("List"))
        {
            return info.FieldType.ToString().GetMiddleString("[", "]").Replace("System.", "");
        }

        return "";
    }

    void SetArray()
    {

    }

    void SetList()
    {

    }
}
