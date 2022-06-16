using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public interface CsvParser
{
    void SetValue(object obj, FieldInfo info, string value);
    void SetValue(object obj, FieldInfo info, string[] values);
    Type GetParserType();
    T GetParserValue<T>(string value);
}

public abstract class CsvParsers
{
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
    public Type GetParserType() => typeof(int);

    public T GetParserValue<T>(string value)
    {
        Int32.TryParse(value, out int valueInt);
        return (T)((object)valueInt);
    }

    public void SetValue(object obj, FieldInfo info, string value)
    {
        Int32.TryParse(value, out int valueInt);
        info.SetValue(obj, valueInt);
    }

    public void SetValue(object obj, FieldInfo info, string[] values)
    {
        SetValue(obj, info, values[0]);
    }
}

class CsvFloatParser : CsvParser
{
    public Type GetParserType() => typeof(float);

    public T GetParserValue<T>(string value)
    {
        float.TryParse(value, out float valueFloat);
        return (T)((object)valueFloat);
    }

    public void SetValue(object obj, FieldInfo info, string value)
    {
        float.TryParse(value, out float valueFloat);
        info.SetValue(obj, valueFloat);
    }

    public void SetValue(object obj, FieldInfo info, string[] values)
    {
        SetValue(obj, info, values[0]);
    }
}

class CsvStringParser : CsvParser
{
    public Type GetParserType() => typeof(string);

    public T GetParserValue<T>(string value)
    {

        return (T)((object)value);
    }

    public void SetValue(object obj, FieldInfo info, string value) => info.SetValue(obj, value);

    public void SetValue(object obj, FieldInfo info, string[] values)
    {
        SetValue(obj, info, values[0]);

    }
}

class CsvBooleanParser : CsvParser
{
    public Type GetParserType() => typeof(bool);

    public T GetParserValue<T>(string value)
    {
        bool boolValue = value == "True" || value == "TRUE";
        return (T)((object)boolValue);
    }

    public void SetValue(object obj, FieldInfo info, string value) => info.SetValue(obj, value == "True" || value == "TRUE");

    public void SetValue(object obj, FieldInfo info, string[] values)
    {
        SetValue(obj, info, values[0]);

    }
}

class CsvUnitFlagsParser : CsvParser
{
    public Type GetParserType() => typeof(UnitFlags);

    public T GetParserValue<T>(string value)
    {
        UnitFlags flags = new UnitFlags(value.Split('&')[0], value.Split('&')[1]);
        return (T)((object)flags);
    }

    public void SetValue(object obj, FieldInfo info, string value)
    {
        Debug.Assert(value.Split('&').Length == 2, $"UnitFlags 데이터 입력 잘못됨 : {value}");
        info.SetValue(obj, new UnitFlags(value.Split('&')[0], value.Split('&')[1]));
    }

    public void SetValue(object obj, FieldInfo info, string[] values)
    {
        SetValue(obj, info, values[0]);
    }
}

// TODO : 구현하기
class CsvEnumerableParser : CsvParser
{
    public Type GetParserType() => typeof(IEnumerable);

    public T GetParserValue<T>(string value)
    {
        throw new NotImplementedException();
    }

    public void SetValue(object obj, FieldInfo info, string value)
    {
        CsvParser parser = CsvParsers.GetParser(GetElementTypeName(info));
        Type type = parser.GetParserType();
        //parser.GetParserValue<typeof(type)>
        //object aa = null;
        info.SetValue(obj, null);
    }

    public void SetValue(object obj, FieldInfo info, string[] values)
    {

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
}
