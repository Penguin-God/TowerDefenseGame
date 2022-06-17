using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

public interface CsvParser
{
    void SetValue(object obj, FieldInfo info, string value);
    void SetValue(object obj, FieldInfo info, string[] values);
    Type GetParserType();
    object GetParserValue(string value);
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

    public object GetParserValue(string value)
    {
        Int32.TryParse(value, out int valueInt);
        return valueInt;
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

    public object GetParserValue(string value)
    {
        float.TryParse(value, out float valueFloat);
        return valueFloat;
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

    public object GetParserValue(string value)
    {

        return value;
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

    public object GetParserValue(string value)
    {
        bool boolValue = value == "True" || value == "TRUE";
        return boolValue;
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

    public object GetParserValue(string value)
    {
        UnitFlags flags = new UnitFlags(value.Split('&')[0], value.Split('&')[1]);
        return flags;
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

    public object GetParserValue(string value)
    {
        throw new NotImplementedException();
    }

    public void SetValue(object obj, FieldInfo info, string value)
    {
        
    }

    public void SetValue(object obj, FieldInfo info, string[] values)
    {
        IEnumerable parserValue = null;

        List<object> valuesList = new List<object>();
        values.ToList().ForEach(x => Debug.Log(x));
        values.ToList().ForEach(x => valuesList.Add(CsvParsers.GetParser(GetElementTypeName(info)).GetParserValue(x)));

        if (info.FieldType.Name.Contains("[]"))
        {
            switch (GetElementTypeName(info))
            {
                case nameof(Int32): parserValue = valuesList.Select(x => (int)x).ToArray(); break;
                case nameof(Single): parserValue = valuesList.Select(x => (float)x).ToArray(); break;
                case nameof(Boolean): parserValue = valuesList.Select(x => (bool)x).ToArray(); break;
                case nameof(String): parserValue = valuesList.Select(x => (string)x).ToArray(); break;
                case nameof(UnitFlags): parserValue = valuesList.Select(x => (UnitFlags)x).ToArray(); break;
                default: Debug.LogError("Csv 파싱 타입을 찾지 못함"); break;
            }
        }
        else if (info.FieldType.Name.Contains("List"))
        {
            switch (GetElementTypeName(info))
            {
                case nameof(Int32): parserValue = valuesList.Select(x => (int)x).ToList(); break;
                case nameof(Single): parserValue = valuesList.Select(x => (float)x).ToList(); break;
                case nameof(Boolean): parserValue = valuesList.Select(x => (bool)x).ToList(); break;
                case nameof(String): parserValue = valuesList.Select(x => (string)x).ToList(); break;
                case nameof(UnitFlags): parserValue = valuesList.Select(x => (UnitFlags)x).ToList(); break;
                default: Debug.LogError("Csv 파싱 타입을 찾지 못함"); break;
            }
        }
        info.SetValue(obj, parserValue);
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
