using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

enum EnumerableType
{
    Unknown,
    Array,
    List,
    Dictionary,
}

public interface CsvPrimitiveTypeParser
{
    object GetParserValue(string value);
}

public interface CsvParser
{
    void SetValue(object obj, FieldInfo info, string[] values);
}

public abstract class CsvParsers
{
    public static CsvParser GetParser(FieldInfo info)
    {
        if (IsEnumerable(info.FieldType.Name))
            return new EnumerableTypeParser(info);
        else
            return new PrimitiveTypeParser();

        bool IsEnumerable(string typeName) => typeName.Contains("[]") || typeName.Contains("List");
    }
}

class EnumerableTypeParser : CsvParser
{
    string _typeName;
    string _elementTypeName;
    EnumerableType _type;
    public EnumerableTypeParser(FieldInfo info)
    {
        _typeName = info.FieldType.Name;
        _elementTypeName = GetElementTypeName();
        _type = GetEnumableType(_typeName);

        string GetElementTypeName()
        {
            if (info.FieldType.Name.Contains("[]"))
                return info.FieldType.GetElementType().Name;
            else if (info.FieldType.Name.Contains("List"))
                return info.FieldType.ToString().GetMiddleString("[", "]").Replace("System.", "");

            return "";
        }

        EnumerableType GetEnumableType(string typeName)
        {
            if (typeName.Contains("[]")) return EnumerableType.Array;
            else if (typeName.Contains("List")) return EnumerableType.List;
            return EnumerableType.Unknown;
        }
    }

    public void SetValue(object obj, FieldInfo info, string[] values)
    {
        IEnumerable parserValue = null;

        List<object> valuesList = new List<object>();
        values.ToList().ForEach(x => valuesList.Add(new PrimitiveTypeParser().GetParserValue(_elementTypeName, x)));

        // TODO : 타입을 추가하면 아래 두 부분을 바꿔야하는게 마음에 안듬. 방법 찾아보기
        if (_type == EnumerableType.Array)
        {
            switch (_elementTypeName)
            {
                case nameof(Int32): parserValue = valuesList.Select(x => (int)x).ToArray(); break;
                case nameof(Single): parserValue = valuesList.Select(x => (float)x).ToArray(); break;
                case nameof(Boolean): parserValue = valuesList.Select(x => (bool)x).ToArray(); break;
                case nameof(String): parserValue = valuesList.Select(x => (string)x).ToArray(); break;
                case nameof(UnitFlags): parserValue = valuesList.Select(x => (UnitFlags)x).ToArray(); break;
                default: Debug.LogError("Csv 파싱 타입을 찾지 못함"); break;
            }
        }
        else if (_type == EnumerableType.List)
        {
            switch (_elementTypeName)
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
}


class PrimitiveTypeParser : CsvParser
{
    public object GetParserValue(string typeName, string value)
    {
        switch (typeName)
        {
            case nameof(Int32): return new CsvIntParser().GetParserValue(value);
            case nameof(Single): return new CsvFloatParser().GetParserValue(value);
            case nameof(Boolean): return new CsvBooleanParser().GetParserValue(value);
            case nameof(String): return new CsvStringParser().GetParserValue(value);
            case nameof(UnitFlags): return new CsvUnitFlagsParser().GetParserValue(value);
            default: Debug.LogError("Csv 파싱 타입을 찾지 못함"); break;
        }
        return null;
    }

    public void SetValue(object obj, FieldInfo info, string[] value)
        => info.SetValue(obj, GetParserValue(info.FieldType.Name, value[0]));
}

class CsvIntParser : CsvPrimitiveTypeParser
{
    public object GetParserValue(string value)
    {
        Int32.TryParse(value, out int valueInt);
        return valueInt;
    }
}

class CsvFloatParser : CsvPrimitiveTypeParser
{
    public object GetParserValue(string value)
    {
        float.TryParse(value, out float valueFloat);
        return valueFloat;
    }
}

class CsvStringParser : CsvPrimitiveTypeParser
{
    public object GetParserValue(string value) => value;
}

class CsvBooleanParser : CsvPrimitiveTypeParser
{
    public object GetParserValue(string value) => value == "True" || value == "TRUE";
}

class CsvUnitFlagsParser : CsvPrimitiveTypeParser
{
    public object GetParserValue(string value)
    {
        Debug.Assert(value.Split('&').Length == 2, $"UnitFlags 데이터 입력 잘못됨 : {value}");
        return new UnitFlags(value.Split('&')[0], value.Split('&')[1]); ;
    }
}