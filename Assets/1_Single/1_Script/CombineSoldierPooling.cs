using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSoldierPooling : MonoBehaviour
{
    public static CombineSoldierPooling Instance;
    //[SerializeField] private GameObject poolingObjectPrefab;
    Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();

    public CreateDefenser createDefenser;
    public UnitDataBase unitDataBase;

    Dictionary<string,Queue<GameObject>> poolingDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < unitDataBase.unitTags.Length; i++)
        {
            poolingDictionary.Add(unitDataBase.unitTags[i], new Queue<GameObject>());
        }

        InitSwordman(5);
        InitArcher(4);
        InitSpearman(3);
        InitMage(2);
    }
    private void InitSwordman(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            poolingDictionary["RedSwordman"].Enqueue(createDefenser.CreateSoldier(0, 0));
            poolingDictionary["BlueSwordman"].Enqueue(createDefenser.CreateSoldier(1, 0));
            poolingDictionary["YellowSwordman"].Enqueue(createDefenser.CreateSoldier(2, 0));
            poolingDictionary["GreenSwordman"].Enqueue(createDefenser.CreateSoldier(3, 0));
            poolingDictionary["OrangeSwordman"].Enqueue(createDefenser.CreateSoldier(4, 0));
            poolingDictionary["VioletSwordman"].Enqueue(createDefenser.CreateSoldier(5, 0));
            poolingDictionary["BlackSwordman"].Enqueue(createDefenser.CreateSoldier(6, 0));
            poolingDictionary["WhiteSwordman"].Enqueue(createDefenser.CreateSoldier(7, 0));

        }     
    }

    private void InitArcher(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            poolingDictionary["RedArcher"].Enqueue(createDefenser.CreateSoldier(0, 1));
            poolingDictionary["BlueArcher"].Enqueue(createDefenser.CreateSoldier(1, 1));
            poolingDictionary["YellowArcher"].Enqueue(createDefenser.CreateSoldier(2, 1));
            poolingDictionary["GreenArcher"].Enqueue(createDefenser.CreateSoldier(3, 1));
            poolingDictionary["OrangeArcher"].Enqueue(createDefenser.CreateSoldier(4, 1));
            poolingDictionary["VioletArcher"].Enqueue(createDefenser.CreateSoldier(5, 1));
            poolingDictionary["BlackArcher"].Enqueue(createDefenser.CreateSoldier(6, 1));
            poolingDictionary["WhiteArcher"].Enqueue(createDefenser.CreateSoldier(7, 1));

        }
    }

    private void InitSpearman(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            poolingDictionary["RedSpearman"].Enqueue(createDefenser.CreateSoldier(0, 2));
            poolingDictionary["BlueSpearman"].Enqueue(createDefenser.CreateSoldier(1, 2));
            poolingDictionary["YellowSpearman"].Enqueue(createDefenser.CreateSoldier(2, 2));
            poolingDictionary["GreenSpearman"].Enqueue(createDefenser.CreateSoldier(3, 2));
            poolingDictionary["OrangeSpearman"].Enqueue(createDefenser.CreateSoldier(4, 2));
            poolingDictionary["VioletSpearman"].Enqueue(createDefenser.CreateSoldier(5, 2));
            poolingDictionary["BlackSpearman"].Enqueue(createDefenser.CreateSoldier(6, 2));
            poolingDictionary["WhiteSpearman"].Enqueue(createDefenser.CreateSoldier(7, 2));

        }
    }

    private void InitMage(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            poolingDictionary["RedMage"].Enqueue(createDefenser.CreateSoldier(0, 3));
            poolingDictionary["BlueMage"].Enqueue(createDefenser.CreateSoldier(1, 3));
            poolingDictionary["YellowMage"].Enqueue(createDefenser.CreateSoldier(2, 3));
            poolingDictionary["GreenMage"].Enqueue(createDefenser.CreateSoldier(3, 3));
            poolingDictionary["OrangeMage"].Enqueue(createDefenser.CreateSoldier(4, 3));
            poolingDictionary["VioletMage"].Enqueue(createDefenser.CreateSoldier(5, 3));
            poolingDictionary["BlackMage"].Enqueue(createDefenser.CreateSoldier(6, 3));
            poolingDictionary["WhiteMage"].Enqueue(createDefenser.CreateSoldier(7, 3));

        }
    }
    private GameObject CreateNewObject(int Colornumber, int Soldiernumber) 
    {
        var newObj = createDefenser.CreateSoldier(Colornumber,Soldiernumber);      
        return newObj;
    }
    //public static GameObject GetObject(int Colornumber, int Soldiernumber) {
    //    if (Instance.poolingObjectQueue.Count > 0)
    //    { 
    //        var obj = Instance.poolingObjectQueue.Dequeue();
    //        //obj.transform.SetParent(null);
    //        obj.gameObject.SetActive(true);
    //        return obj;
    //    }
    //    else 
    //    {
    //        var newObj = Instance.CreateNewObject(Colornumber, Soldiernumber);
    //        newObj.gameObject.SetActive(true);
    //        //newObj.transform.SetParent(null);
    //        return newObj; 
    //    } 
    //}

    public static GameObject GetObject(string Soldier, int Colornumber, int Soldiernumber)
    {
        if (Instance.poolingDictionary[Soldier].Count > 0)
        {
            var obj = Instance.poolingDictionary[Soldier].Dequeue();
            //obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject(Colornumber, Soldiernumber);
            newObj.gameObject.SetActive(true);
            //newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnObject(GameObject obj, string Soldier) 
    {
        obj.gameObject.SetActive(false);
        //obj.transform.SetParent(Instance.transform);
        Instance.poolingDictionary[Soldier].Enqueue(obj);
    }
}
