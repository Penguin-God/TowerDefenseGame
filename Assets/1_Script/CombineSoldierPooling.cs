using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSoldierPooling : MonoBehaviour
{
    public static CombineSoldierPooling Instance;
    [SerializeField] private GameObject poolingObjectPrefab;
    Queue<Unit_Swordman> poolingObjectQueue = new Queue<Unit_Swordman>();
    private void Awake()
    {
        Instance = this; Initialize(10);
    }
    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject()); 
        } 
    }
    private Unit_Swordman CreateNewObject() 
    {
        var newObj = Instantiate(poolingObjectPrefab).GetComponent<Unit_Swordman>(); newObj.gameObject.SetActive(false); newObj.transform.SetParent(transform); return newObj;
    }
    public static Unit_Swordman GetObject() {
        if (Instance.poolingObjectQueue.Count > 0)
        { var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null); obj.gameObject.SetActive(true);
            return obj;
        }
        else 
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj; 
        } 
    }
    public static void ReturnObject(Unit_Swordman obj) 
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }

}
