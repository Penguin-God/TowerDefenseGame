﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
using System.Text;

enum EnumerableType
{
    Unknown,
    Array,
    List,
    Dictionary,
}

public interface ICsvIEnumeralbeParser
{
    string[] GetCsvValues(object obj, FieldInfo info);
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
        if (TypeIdentifier.IsIEnumerable(info.FieldType))
            return new EnumerableTypeParser();
        else if (IsPair(info.FieldType.Name))
            return new CsvPairParser();
        else
            return new PrimitiveTypeParser();

        bool IsPair(string typeName) => typeName == "KeyValuePair`2";
    }
}


class PrimitiveTypeParser : CsvParser
{
    public static CsvPrimitiveTypeParser GetPrimitiveParser(Type type)
    {
        if (type == typeof(int)) return new CsvIntParser();
        else if (type == typeof(string)) return new CsvStringParser();
        else if (type == typeof(float)) return new CsvFloatParser();
        else if (type == typeof(bool)) return new CsvBooleanParser();
        else if (type == typeof(UnitFlags)) return new CsvUnitFlagsParser();
        else if (type.IsEnum) return new CsvEnumParser(type);
        else Debug.LogError("Csv 파싱 타입을 찾지 못함");
        return null;
    }

    object GetParserValue(Type type, string value) => GetPrimitiveParser(type).GetParserValue(value);
    public void SetValue(object obj, FieldInfo info, string[] value) => info.SetValue(obj, GetParserValue(info.FieldType, value[0]));
}

class EnumerableTypeParser : CsvParser
{
    EnumerableType GetEnumableType(Type type)
    {
        if (type.IsArray) return EnumerableType.Array;
        else if (TypeIdentifier.IsList(type)) return EnumerableType.List;
        else if (TypeIdentifier.IsDictionary(type)) return EnumerableType.Dictionary;
        return EnumerableType.Unknown;
    }

    public void SetValue(object obj, FieldInfo info, string[] values)
    {
        values = SetValue(values);
        switch (GetEnumableType(info.FieldType))
        {
            case EnumerableType.Array: new CsvArrayParser().SetValue(obj, info, values); break;
            case EnumerableType.List: new CsvListParser().SetValue(obj, info, values); break;
            case EnumerableType.Dictionary: new CsvDictionaryParser().SetValue(obj, info, values); break;
        }
    }

    string[] SetValue(string[] values)
    {
        const char replaceMark = '+';
        StringBuilder builder = new StringBuilder();

        foreach (string value in values.Where(x => !string.IsNullOrEmpty(x)))
        {
            builder.Append(replaceMark);
            builder.Append(value);
        }
        return builder.ToString().Split(replaceMark).Skip(1).Select(x => x.Trim()).ToArray();
    }

    public ICsvIEnumeralbeParser GetIEnumerableParser(Type type)
    {
        if (type.IsArray) return new CsvArrayParser();
        else if (TypeIdentifier.IsList(type)) return new CsvListParser();
        else if (TypeIdentifier.IsDictionary(type)) return new CsvDictionaryParser();
        return null;
    }

    public string[] GetIEnumerableValues(object obj, FieldInfo info)
    {
        ICsvIEnumeralbeParser parser = GetIEnumerableParser(info.FieldType);
        if (parser != null)
            return parser.GetCsvValues(obj, info);
        else
            return null;
    }
}

#region 기본형 파싱

class CsvIntParser : CsvPrimitiveTypeParser
{
    public object GetParserValue(string value)
    {
        Int32.TryParse(value, out int valueInt);
        return valueInt;
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

class CsvEnumParser : CsvPrimitiveTypeParser
{
    Type _type;
    public CsvEnumParser(Type type)
    {
        _type = type;
    }

    public object GetParserValue(string value) => Enum.Parse(_type, value);
    public IEnumerable GetParserEnumerable(string[] value) => value.Select(x => Convert.ChangeType(GetParserValue(x), _type));
    public Type GetParserType() => _type;
}

class CsvUnitFlagsParser : CsvPrimitiveTypeParser
{
    public object GetParserValue(string value)
    {
        if (value.Split('&').Length == 2)
            return new UnitFlags(value.Split('&')[0], value.Split('&')[1]);
        else
        {
            Debug.Assert(Multi_Managers.Data.UnitNameDataByUnitKoreaName.ContainsKey(value), $"잘못된 유닛 이름 : {value}");
            return Multi_Managers.Data.UnitNameDataByUnitKoreaName[value].UnitFlags;
        }
    }

    public IEnumerable GetParserEnumerable(string[] value) => value.Select(x => (UnitFlags)GetParserValue(x));
    public Type GetParserType() => typeof(UnitFlags);
}

public class CsvPairParser : CsvParser
{
    public void SetValue(object obj, FieldInfo info, string[] values)
    {
        if (values.Length != 2) Debug.LogError($"{info.Name} 입력이 올바르지 않습니다. Key Value 쌍을 정확히 입력했는지 확인해주세요");
        Type[] elementTypes = info.FieldType.GetGenericArguments();
        ConstructorInfo constructor = info.FieldType.GetConstructors()[0];
        info.SetValue(obj, constructor.Invoke(new object[] { PrimitiveTypeParser.GetPrimitiveParser(elementTypes[0]).GetParserValue(values[0]),
                                                             PrimitiveTypeParser.GetPrimitiveParser(elementTypes[1]).GetParserValue(values[1]) }));
    }
}

#endregion 기본형 파싱 End




#region 열거형 파싱
public class CsvListParser : ICsvIEnumeralbeParser
{
    public void SetValue(object obj, FieldInfo info, string[] values)
    {
        Type elementType = info.FieldType.GetGenericArguments()[0];
        ConstructorInfo constructor = info.FieldType.GetConstructors()[2];
        info.SetValue(obj, constructor.Invoke(new object[] { PrimitiveTypeParser.GetPrimitiveParser(elementType).GetParserEnumerable(values) }));
    }

    public string[] GetCsvValues(object obj, FieldInfo info)
    {
        IList list = info.GetValue(obj) as IList;
        List<string> result = new List<string>();
        foreach (var item in list) result.Add(item.ToString());
        return result.ToArray();
    }
}

public class CsvArrayParser : ICsvIEnumeralbeParser
{
    public void SetValue(object obj, FieldInfo info, string[] values)
    {
        Type elementType = info.FieldType.GetElementType();
        Array array = Array.CreateInstance(PrimitiveTypeParser.GetPrimitiveParser(elementType).GetParserType(), values.Length);
        for (int i = 0; i < array.Length; i++)
            array.SetValue(PrimitiveTypeParser.GetPrimitiveParser(elementType).GetParserValue(values[i]), i);
        info.SetValue(obj, array);
    }

    public string[] GetCsvValues(object obj, FieldInfo info)
    {
        Array array = info.GetValue(obj) as Array;
        List<string> result = new List<string>();
        foreach (var item in array) result.Add(item.ToString());
        return result.ToArray();
    }
}

public class CsvDictionaryParser : ICsvIEnumeralbeParser
{
    public void SetValue(object obj, FieldInfo info, string[] values)
    {
        if (values.Length % 2 != 0) Debug.LogError($"{info.Name} 입력이 올바르지 않습니다. Key Value 쌍을 정확히 입력했는지 확인해주세요");

        Type[] elementTypes = info.FieldType.GetGenericArguments();
        MethodInfo methodInfo = info.FieldType.GetMethod("Add");
        for (int i = 0; i < values.Length; i += 2)
        {
            methodInfo.Invoke(info.GetValue(obj), new object[] { PrimitiveTypeParser.GetPrimitiveParser(elementTypes[0]).GetParserValue(values[i]),
                                                                 PrimitiveTypeParser.GetPrimitiveParser(elementTypes[1]).GetParserValue(values[i+1]) });
        }
    }

    public string[] GetCsvValues(object obj, FieldInfo info)
    {
        IDictionary dictionary = info.GetValue(obj) as IDictionary;
        List<string> keys = new List<string>();
        List<string> values = new List<string>();

        foreach (var item in dictionary.Keys) keys.Add(item.ToString());
        foreach (var item in dictionary.Values) values.Add(item.ToString());

        List<string> result = new List<string>();
        for (int i = 0; i < keys.Count; i++)
        {
            result.Add(keys[i]);
            result.Add(values[i]);
        }

        return result.ToArray();
    }
}
#endregion 열거형 파싱 End

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Reflection;
//using System;
//using System.Linq;

//enum EnumerableType
//{
//    Unknown,
//    Array,
//    List,
//    Dictionary,
//}

//public interface CsvPrimitiveTypeParser
//{
//    object GetParserValue(string value);

//    IEnumerable GetParserEnumerable(string[] values);
//    Type GetParserType();
//}

//public interface CsvParser
//{
//    void SetValue(object obj, FieldInfo info, string[] values);
//}

//public abstract class CsvParsers
//{
//    public static CsvParser GetParser(FieldInfo info)
//    {
//        if (IsEnumerable(info.FieldType.Name))
//            return new EnumerableTypeParser(info);
//        else
//            return new PrimitiveTypeParser();

//        bool IsEnumerable(string typeName) => typeName.Contains("[]") || typeName.Contains("List");
//    }
//}


//class PrimitiveTypeParser : CsvParser
//{
//    public static CsvPrimitiveTypeParser GetPrimitiveParser(string typeName)
//    {
//        typeName = typeName.Replace("System.", "");
//        switch (typeName)
//        {
//            case nameof(Int32): return new CsvIntParser();
//            case nameof(Single): return new CsvFloatParser();
//            case nameof(Boolean): return new CsvBooleanParser();
//            case nameof(String): return new CsvStringParser();
//            case nameof(UnitFlags): return new CsvUnitFlagsParser();
//            default: Debug.LogError("Csv 파싱 타입을 찾지 못함"); break;
//        }
//        return null;
//    }

//    object GetParserValue(string typeName, string value) => GetPrimitiveParser(typeName).GetParserValue(value);

//    public void SetValue(object obj, FieldInfo info, string[] value)
//        => info.SetValue(obj, GetParserValue(info.FieldType.Name, value[0]));
//}

//class EnumerableTypeParser : CsvParser
//{
//    string _typeName;
//    string _elementTypeName;
//    EnumerableType _type;
//    public EnumerableTypeParser(FieldInfo info)
//    {
//        _typeName = info.FieldType.Name;
//        _elementTypeName = GetElementTypeName().Replace("System.", "");
//        _type = GetEnumableType(_typeName);

//        string GetElementTypeName()
//        {
//            if (info.FieldType.Name.Contains("[]"))
//                return info.FieldType.GetElementType().Name;
//            else if (info.FieldType.Name.Contains("List"))
//                return info.FieldType.ToString().GetMiddleString("[", "]");

//            return "";
//        }

//        EnumerableType GetEnumableType(string typeName)
//        {
//            if (typeName.Contains("[]")) return EnumerableType.Array;
//            else if (typeName.Contains("List")) return EnumerableType.List;
//            return EnumerableType.Unknown;
//        }
//    }

//    public void SetValue(object obj, FieldInfo info, string[] values)
//    {
//        values = values.Where(x => !string.IsNullOrEmpty(x)).ToArray();
//        if (_type == EnumerableType.Array)
//            new CsvArrayParser().SetValue(obj, info, values, _elementTypeName);
//        else if (_type == EnumerableType.List)
//            new CsvListParser().SetValue(obj, info, values, _elementTypeName);
//    }
//}

//#region 기본형 파싱

//class CsvIntParser : CsvPrimitiveTypeParser
//{
//    public object GetParserValue(string value)
//    {
//        Int32.TryParse(value, out int valueInt);
//        return valueInt;
//    }

//    public IEnumerable GetParserEnumerable(string[] value) => value.Select(x => (int)GetParserValue(x));

//    public Type GetParserType() => typeof(int);
//}

//class CsvFloatParser : CsvPrimitiveTypeParser
//{
//    public object GetParserValue(string value)
//    {
//        float.TryParse(value, out float valueFloat);
//        return valueFloat;
//    }

//    public IEnumerable GetParserEnumerable(string[] value) => value.Select(x => (float)GetParserValue(x));
//    public Type GetParserType() => typeof(float);
//}

//class CsvStringParser : CsvPrimitiveTypeParser
//{
//    public object GetParserValue(string value) => value;
//    public IEnumerable GetParserEnumerable(string[] value) => value.AsEnumerable();
//    public Type GetParserType() => typeof(string);
//}

//class CsvBooleanParser : CsvPrimitiveTypeParser
//{
//    public object GetParserValue(string value) => value == "True" || value == "TRUE";
//    public IEnumerable GetParserEnumerable(string[] value) => value.Select(x => (bool)GetParserValue(x));
//    public Type GetParserType() => typeof(bool);
//}

//class CsvUnitFlagsParser : CsvPrimitiveTypeParser
//{
//    public object GetParserValue(string value)
//    {
//        if (value.Split('&').Length == 2)
//            return new UnitFlags(value.Split('&')[0], value.Split('&')[1]);
//        else
//        {
//            Debug.Assert(Multi_Managers.Data.UnitNameDataByUnitKoreaName.ContainsKey(value), $"잘못된 유닛 이름 : {value}");
//            return Multi_Managers.Data.UnitNameDataByUnitKoreaName[value].UnitFlags;
//        }
//    }

//    public IEnumerable GetParserEnumerable(string[] value) => value.Select(x => (UnitFlags)GetParserValue(x));
//    public Type GetParserType() => typeof(UnitFlags);
//}

//#endregion 기본형 파싱 End


//#region 열거형 파싱
//public class CsvListParser
//{
//    public void SetValue(object obj, FieldInfo info, string[] values, string typeName)
//    {
//        ConstructorInfo constructor = info.FieldType.GetConstructors()[2];
//        info.SetValue(obj, constructor.Invoke(new object[] { PrimitiveTypeParser.GetPrimitiveParser(typeName).GetParserEnumerable(values) }));
//    }
//}

//public class CsvArrayParser
//{
//    public void SetValue(object obj, FieldInfo info, string[] values, string typeName)
//    {
//        Array array = Array.CreateInstance(PrimitiveTypeParser.GetPrimitiveParser(typeName).GetParserType(), values.Length);
//        for (int i = 0; i < array.Length; i++)
//            array.SetValue(PrimitiveTypeParser.GetPrimitiveParser(typeName).GetParserValue(values[i]), i);
//        info.SetValue(obj, array);
//    }
//}
//#endregion 열거형 파싱 End
