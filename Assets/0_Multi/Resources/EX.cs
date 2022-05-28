using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EX : MonoBehaviour
{

	//static EX _instance;
	//public static EX Instance
	//{
	//	get
	//	{
	//		if (_instance == null)
	//			_instance = new EX();
	//		return _instance;
	//	}
	//}

	public static List<Dictionary<string, object>> _data = null;

    public static List<Dictionary<string, object>> ReadEX(string file_name)
    {
        List<Dictionary<string, object>> data = CSVReader.Read(file_name);

        _data = data;

        return _data;
    }

	public static void test()
    {
		if (_data == null)
        {
			Debug.Log("_data is null");
			return;
        }
		for (var i = 0; i < _data.Count; i++)
		{
			Debug.Log("index " + (i).ToString() + " : " + _data[i]["Name"] + " " + _data[i]["Age"]);
		}
	}

	static int Check(string name)
    {
		for (var i = 0; i < _data.Count; i++)
        {
			if ((string)_data[i]["Name"] == name)
            {
				return i;
            }
        }
		return -1;

	}

	public static void SetValue(string name, string column, string value)
    {
		int _row = Check(name);
		_data[_row][column] = value;

	}

	public static string GetValue(string name, string column)
	{
		int _row = Check(name);
		return (string)_data[_row][column];
	}
}
