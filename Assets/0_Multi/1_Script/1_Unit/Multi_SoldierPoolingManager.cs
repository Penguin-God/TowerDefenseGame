using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Multi_CurrentUnitManager
{
    private Dictionary<string, List<Multi_TeamSoldier>> UnitDictionary = new Dictionary<string, List<Multi_TeamSoldier>>();
    private Dictionary<UnitColor, List<Multi_TeamSoldier>> UnitColorDictionary = new Dictionary<UnitColor, List<Multi_TeamSoldier>>();
    private Dictionary<UnitClass, List<Multi_TeamSoldier>> UnitClassDictionary = new Dictionary<UnitClass, List<Multi_TeamSoldier>>();
    private Dictionary<KeyValuePair<UnitColor, UnitClass>, List<Multi_TeamSoldier>> UnitPairDictionary = 
        new Dictionary<KeyValuePair<UnitColor, UnitClass>, List<Multi_TeamSoldier>>();
    private List<Multi_TeamSoldier> AllUnit = new List<Multi_TeamSoldier>();

    public Multi_CurrentUnitManager(string[] _unitTags)
    {
        SettingUnitDictionary(_unitTags);
        SettingColorDictionary();
        SettingClassDictionary();
        SettingPairDictionary();
    }

    void SettingUnitDictionary(string[] _unitTags)
    {
        UnitDictionary = new Dictionary<string, List<Multi_TeamSoldier>>();
        for (int i = 0; i < _unitTags.Length; i++) UnitDictionary.Add(_unitTags[i], new List<Multi_TeamSoldier>());
    }

    void SettingColorDictionary()
    {
        foreach (UnitColor _color in Enum.GetValues(typeof(UnitColor)))
            UnitColorDictionary.Add(_color, new List<Multi_TeamSoldier>());
    }

    void SettingClassDictionary()
    {
        foreach (UnitClass _class in Enum.GetValues(typeof(UnitClass)))
            UnitClassDictionary.Add(_class, new List<Multi_TeamSoldier>());
    }

    void SettingPairDictionary()
    {
        foreach (UnitColor _color in Enum.GetValues(typeof(UnitColor)))
        {
            foreach (UnitClass _class in Enum.GetValues(typeof(UnitClass)))
            {
                KeyValuePair<UnitColor, UnitClass> _pair = new KeyValuePair<UnitColor, UnitClass>(_color, _class);
                UnitPairDictionary.Add(_pair, new List<Multi_TeamSoldier>());
            }
        }
    }

    public void AddUnit(Multi_TeamSoldier _unit)
    {
        UnitDictionary[_unit.gameObject.tag].Add(_unit);
        UnitColorDictionary[_unit.unitColor].Add(_unit);
        UnitClassDictionary[_unit.unitClass].Add(_unit);
        UnitPairDictionary[new KeyValuePair<UnitColor, UnitClass>(_unit.unitColor, _unit.unitClass)].Add(_unit);
        AllUnit.Add(_unit);
    }

    public void RemoveUnit(Multi_TeamSoldier _unit)
    {
        UnitDictionary[_unit.gameObject.tag].Remove(_unit);
        UnitColorDictionary[_unit.unitColor].Remove(_unit);
        UnitClassDictionary[_unit.unitClass].Remove(_unit);
        UnitPairDictionary[new KeyValuePair<UnitColor, UnitClass>(_unit.unitColor, _unit.unitClass)].Remove(_unit);
        AllUnit.Remove(_unit);
    }

    public Multi_TeamSoldier[] GetUnits(string _tag) => UnitDictionary[_tag].ToArray();

    public Multi_TeamSoldier[] GetUnits(string _tag, out int _count)
    {
        Multi_TeamSoldier[] _units = UnitDictionary[_tag].ToArray();
        _count = _units.Length;
        return _units;
    }

    public Multi_TeamSoldier[] GetUnits(UnitColor _color) => UnitColorDictionary[_color].ToArray();
    public Multi_TeamSoldier[] GetUnits(UnitColor _color, out int _count)
    {
        Multi_TeamSoldier[] _units = UnitColorDictionary[_color].ToArray();
        _count = _units.Length;
        return _units;
    }

    public Multi_TeamSoldier[] GetUnits(UnitClass _class) => UnitClassDictionary[_class].ToArray();

    public Multi_TeamSoldier[] GetUnits(UnitColor _color, UnitClass _class)
    {
        Multi_TeamSoldier[] _units = UnitPairDictionary[new KeyValuePair<UnitColor, UnitClass>(_color, _class)].ToArray();
        return _units;
    }

    public Multi_TeamSoldier[] GetAllUnit() => AllUnit.ToArray();
    public int GetUnitCount() => AllUnit.Count;
}

public class Multi_SoldierPoolingManager : MonoBehaviour
{
    public static Multi_SoldierPoolingManager Instance;
    //Queue<Multi_TeamSoldier> soldierQueue = new Queue<Multi_TeamSoldier>();

    [SerializeField] Multi_SoldierSpawner soldierSpawner;
    [SerializeField] Multi_UnitDataBase unitDataBase;

    Dictionary<string, Queue<Multi_TeamSoldier>> poolingDictionary = new Dictionary<string, Queue<Multi_TeamSoldier>>();

    private void Awake()
    {
        currentUnitManager = new Multi_CurrentUnitManager(unitDataBase.UnitTags);

        Instance = this;
        for (int i = 0; i < unitDataBase.UnitTags.Length; i++)
            poolingDictionary.Add(unitDataBase.UnitTags[i], new Queue<Multi_TeamSoldier>());

        InitSwordman(5);
        InitArcher(4);
        InitSpearman(3);
        InitMage(2);
    }
    private void InitSwordman(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            poolingDictionary["RedSwordman"].Enqueue(soldierSpawner.SpawnSoldier(0, 0));
            poolingDictionary["BlueSwordman"].Enqueue(soldierSpawner.SpawnSoldier(1, 0));
            poolingDictionary["YellowSwordman"].Enqueue(soldierSpawner.SpawnSoldier(2, 0));
            poolingDictionary["GreenSwordman"].Enqueue(soldierSpawner.SpawnSoldier(3, 0));
            poolingDictionary["OrangeSwordman"].Enqueue(soldierSpawner.SpawnSoldier(4, 0));
            poolingDictionary["VioletSwordman"].Enqueue(soldierSpawner.SpawnSoldier(5, 0));
            poolingDictionary["BlackSwordman"].Enqueue(soldierSpawner.SpawnSoldier(6, 0));
            poolingDictionary["WhiteSwordman"].Enqueue(soldierSpawner.SpawnSoldier(7, 0));

        }
    }

    private void InitArcher(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            poolingDictionary["RedArcher"].Enqueue(soldierSpawner.SpawnSoldier(0, 1));
            poolingDictionary["BlueArcher"].Enqueue(soldierSpawner.SpawnSoldier(1, 1));
            poolingDictionary["YellowArcher"].Enqueue(soldierSpawner.SpawnSoldier(2, 1));
            poolingDictionary["GreenArcher"].Enqueue(soldierSpawner.SpawnSoldier(3, 1));
            poolingDictionary["OrangeArcher"].Enqueue(soldierSpawner.SpawnSoldier(4, 1));
            poolingDictionary["VioletArcher"].Enqueue(soldierSpawner.SpawnSoldier(5, 1));
            poolingDictionary["BlackArcher"].Enqueue(soldierSpawner.SpawnSoldier(6, 1));
            poolingDictionary["WhiteArcher"].Enqueue(soldierSpawner.SpawnSoldier(7, 1));

        }
    }

    private void InitSpearman(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            poolingDictionary["RedSpearman"].Enqueue(soldierSpawner.SpawnSoldier(0, 2));
            poolingDictionary["BlueSpearman"].Enqueue(soldierSpawner.SpawnSoldier(1, 2));
            poolingDictionary["YellowSpearman"].Enqueue(soldierSpawner.SpawnSoldier(2, 2));
            poolingDictionary["GreenSpearman"].Enqueue(soldierSpawner.SpawnSoldier(3, 2));
            poolingDictionary["OrangeSpearman"].Enqueue(soldierSpawner.SpawnSoldier(4, 2));
            poolingDictionary["VioletSpearman"].Enqueue(soldierSpawner.SpawnSoldier(5, 2));
            poolingDictionary["BlackSpearman"].Enqueue(soldierSpawner.SpawnSoldier(6, 2));
            poolingDictionary["WhiteSpearman"].Enqueue(soldierSpawner.SpawnSoldier(7, 2));

        }
    }

    private void InitMage(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            poolingDictionary["RedMage"].Enqueue(soldierSpawner.SpawnSoldier(0, 3));
            poolingDictionary["BlueMage"].Enqueue(soldierSpawner.SpawnSoldier(1, 3));
            poolingDictionary["YellowMage"].Enqueue(soldierSpawner.SpawnSoldier(2, 3));
            poolingDictionary["GreenMage"].Enqueue(soldierSpawner.SpawnSoldier(3, 3));
            poolingDictionary["OrangeMage"].Enqueue(soldierSpawner.SpawnSoldier(4, 3));
            poolingDictionary["VioletMage"].Enqueue(soldierSpawner.SpawnSoldier(5, 3));
            poolingDictionary["BlackMage"].Enqueue(soldierSpawner.SpawnSoldier(6, 3));
            poolingDictionary["WhiteMage"].Enqueue(soldierSpawner.SpawnSoldier(7, 3));

        }
    }
   
    public static Multi_TeamSoldier GetSoldier(string _tag, int Colornumber, int Soldiernumber)
    {
        if (Instance.poolingDictionary[_tag].Count > 0)
        {
            Multi_TeamSoldier _soldier= Instance.poolingDictionary[_tag].Dequeue();
            currentUnitManager.AddUnit(_soldier);
            Multi_UIManager.instance.UpdateCurrentUnitText(CurrentUnitCount, Multi_UnitManager.instance.MaxUnit);
            _soldier.gameObject.SetActive(true);
            return _soldier;
        }
        else
        {
            Multi_TeamSoldier _newSoldier = Instance.CreateNewSoldier(Colornumber, Soldiernumber);
            currentUnitManager.AddUnit(_newSoldier);
            Multi_UIManager.instance.UpdateCurrentUnitText(CurrentUnitCount, Multi_UnitManager.instance.MaxUnit);
            _newSoldier.gameObject.SetActive(true);
            return _newSoldier;
        }
    }
    private Multi_TeamSoldier CreateNewSoldier(int Colornumber, int Soldiernumber)
    {
        var newObj = soldierSpawner.SpawnSoldier(Colornumber, Soldiernumber);
        return newObj;
    }

    public static void ReturnObject(Multi_TeamSoldier _soldier, string _tag)
    {
        _soldier.gameObject.SetActive(false);
        Instance.poolingDictionary[_tag].Enqueue(_soldier);
        currentUnitManager.RemoveUnit(_soldier);
        Multi_UIManager.instance.UpdateCurrentUnitText(CurrentUnitCount, Multi_UnitManager.instance.MaxUnit);
    }


    private static Multi_CurrentUnitManager currentUnitManager;
    public static int CurrentUnitCount => currentUnitManager.GetUnitCount();
    public Multi_TeamSoldier[] AllUnits => currentUnitManager.GetAllUnit();
    public static Multi_TeamSoldier[] GetCurrentSoldiers(string _tag) => currentUnitManager.GetUnits(_tag);
    public Multi_TeamSoldier[] GetCurrentSoldiers(UnitClass _class) => currentUnitManager.GetUnits(_class);
    public Multi_TeamSoldier[] GetCurrentSoldiers(UnitColor _color) => currentUnitManager.GetUnits(_color);
    public Multi_TeamSoldier[] GetCurrentSoldiers(UnitColor _color, UnitClass _class) => currentUnitManager.GetUnits(_color, _class);
}
