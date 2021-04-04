using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int Stage;
    public int Gold;

    //public GameObject target;

    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    private static GameManager m_instance;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        Gold = 20;
        UIManager.instance.UpdateGoldText(Gold);
    }

    
    void Update()
    {
        
    }

    //public GameObject hitSoldier;
    //public void Chilk()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            Debug.Log(hit.transform.gameObject);
    //            hitSoldier = hit.transform.gameObject; // 클릭한 게임오브젝트를 public 변수에 담아서 다른 스크립트에서 사용할 수 있도록 함
    //        }
    //    }
    //}
}
