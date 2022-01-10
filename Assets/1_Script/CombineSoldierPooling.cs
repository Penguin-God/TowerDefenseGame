using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSoldierPooling : MonoBehaviour
{
    public static CombineSoldierPooling Instance;
    [SerializeField] private GameObject poolingObjectPrefab;
    Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();

    public CreateDefenser createDefenser;

    
    private void Awake()
    {
        Instance = this;
        Initialize();
    }
    private void Initialize()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                poolingObjectQueue.Enqueue(createDefenser.CreateSoldier(i, j));
            }
        } 
    }
    private GameObject CreateNewObject() 
    {
        var newObj = Instantiate(poolingObjectPrefab).GetComponent<GameObject>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }
    public static GameObject GetObject() {
        if (Instance.poolingObjectQueue.Count > 0)
        { 
            var obj = Instance.poolingObjectQueue.Dequeue();
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
    public static void ReturnObject(GameObject obj) 
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }

}
