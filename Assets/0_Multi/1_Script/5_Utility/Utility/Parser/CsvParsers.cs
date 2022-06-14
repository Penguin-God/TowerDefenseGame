using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public abstract class CsvParserBase
{
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

