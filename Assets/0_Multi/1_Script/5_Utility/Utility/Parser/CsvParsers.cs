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

    IEnumerable GetParserEnumerable(string[] values);
    Type GetParserType();
}

public interface CsvParser
{
    void SetValue(object obj, FieldInfo info, string[] values);
}

public abstract class CsvParsers
{
    public static CsvParser GetParser(FieldInfo info)
    {
        // Debug.Log(IsEnumerable(info.FieldType.Name));
        if (IsEnumerable(info.FieldType.Name))
            return new EnumerableTypeParser(info);
        else
            return new PrimitiveTypeParser();

        bool IsEnumerable(string typeName) => typeName.Contains("[]") || typeName.Contains("List");
    }

    public static CsvPrimitiveTypeParser GetPrimitiveParser(string typeName)
    {
        typeName = typeName.Replace("System.", "");
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
}

class EnumerableTypeParser : CsvParser
{
    string _typeName;
    string _elementTypeName;
    EnumerableType _type;
    public EnumerableTypeParser(FieldInfo info)
    {
        _typeName = info.FieldType.Name;
        _elementTypeName = GetElementTypeName().Replace("System.", "");
        _type = GetEnumableType(_typeName);

        string GetElementTypeName()
        {
            if (info.FieldType.Name.Contains("[]"))
                return info.FieldType.GetElementType().Name;
            else if (info.FieldType.Name.Contains("List"))
                return info.FieldType.ToString().GetMiddleString("[", "]");

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
        if (_type == EnumerableType.Array)
        {
            new CsvArrayParser().SetValue(obj, info, values, _elementTypeName);
            //ConstructorInfo constructor = List<object>.FieldType.GetConstructors()[2];
            //info.SetValue(obj, CsvParsers.GetPrimitiveParser(_elementTypeName).GetParserEnumerable(values).);

            //switch (_elementTypeName)
            //{
            //    case nameof(Int32): parserValue = valuesList.Select(x => (int)x).ToArray(); break;
            //    case nameof(Single): parserValue = valuesList.Select(x => (float)x).ToArray(); break;
            //    case nameof(Boolean): parserValue = valuesList.Select(x => (bool)x).ToArray(); break;
            //    case nameof(String): parserValue = valuesList.Select(x => (string)x).ToArray(); break;
            //    case nameof(UnitFlags): parserValue = valuesList.Select(x => (UnitFlags)x).ToArray(); break;
            //    default: Debug.LogError("Csv 파싱 타입을 찾지 못함"); break;
            //}
        }
        else if (_type == EnumerableType.List)
        {
            
            //switch (_elementTypeName)
            //{
            //    case nameof(Int32): parserValue = valuesList.Select(x => (int)x).ToList(); break;
            //    case nameof(Single): parserValue = valuesList.Select(x => (float)x).ToList(); break;
            //    case nameof(Boolean): parserValue = valuesList.Select(x => (bool)x).ToList(); break;
            //    case nameof(String): parserValue = valuesList.Select(x => (string)x).ToList(); break;
            //    case nameof(UnitFlags): parserValue = valuesList.Select(x => (UnitFlags)x).ToList(); break;
            //    default: Debug.LogError("Csv 파싱 타입을 찾지 못함"); break;
            //}
            new CsvListParser().SetValue(obj, info, values, _elementTypeName);
        }
    }
}

public class CsvListParser
{
    public void SetValue(object obj, FieldInfo info, string[] values, string typeName)
    {
        ConstructorInfo constructor = info.FieldType.GetConstructors()[2];
        info.SetValue(obj, constructor.Invoke(new object[] { CsvParsers.GetPrimitiveParser(typeName).GetParserEnumerable(values) }));
    }
}

public class CsvArrayParser
{
    public void SetValue(object obj, FieldInfo info, string[] values, string typeName)
    {
        Array array = Array.CreateInstance(CsvParsers.GetPrimitiveParser(typeName).GetParserType(), values.Length);
        for (int i = 0; i < array.Length; i++)
            array.SetValue(CsvParsers.GetPrimitiveParser(typeName).GetParserValue(values[i]), i);
        info.SetValue(obj, array);
    }
}


class PrimitiveTypeParser : CsvParser
{
    public object GetParserValue(string typeName, string value)
    {
        typeName = typeName.Replace("System.", "");
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

    public IEnumerable<int> GetParserEnumerables(string[] value)
    {
        return value.Select(x => (int)GetParserValue(x));
    }

    public IEnumerable GetParserEnumerable(string[] value) => value.Select(x => (int)GetParserValue(x));

    public Type GetParserType() => typeof(int);
}

class CsvFloatParser : CsvPrimitiveTypeParser
{
    public object GetParserValue(string value)
    {
        float.TryParse(value, out float valueFloat);
        return valueFloat;
    }

    public IEnumerable GetParserEnumerable(string[] value) => value.Select(x => (float)GetParserValue(x));
    public Type GetParserType() => typeof(float);
}

class CsvStringParser : CsvPrimitiveTypeParser
{
    public object GetParserValue(string value) => value;
    public IEnumerable GetParserEnumerable(string[] value) => value.AsEnumerable();
    public Type GetParserType() => typeof(string);
}

class CsvBooleanParser : CsvPrimitiveTypeParser
{
    public object GetParserValue(string value) => value == "True" || value == "TRUE";
    public IEnumerable GetParserEnumerable(string[] value) => value.Select(x => (bool)GetParserValue(x));
    public Type GetParserType() => typeof(bool);
}

class CsvUnitFlagsParser : CsvPrimitiveTypeParser
{
    public object GetParserValue(string value)
    {
        Debug.Assert(value.Split('&').Length == 2, $"UnitFlags 데이터 입력 잘못됨 : {value}");
        return new UnitFlags(value.Split('&')[0], value.Split('&')[1]);
    }

    public IEnumerable GetParserEnumerable(string[] value) => value.Select(x => (UnitFlags)GetParserValue(x));
    public Type GetParserType() => typeof(UnitFlags);
}

class CsvPairParser : CsvPrimitiveTypeParser
{
    public IEnumerable GetParserEnumerable(string[] values)
    {
        throw new NotImplementedException();
    }

    public Type GetParserType()
    {
        throw new NotImplementedException();
    }

    public object GetParserValue(string value)
    {
        return null;
    }

    public object GetParserValue(string value, Type type)
    {
        
        return null;
    }
}