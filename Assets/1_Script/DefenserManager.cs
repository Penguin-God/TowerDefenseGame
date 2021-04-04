using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DefenserManager : MonoBehaviour // UI 버튼에서 사용되는 함수는 DarwSoldier(), SellSolider()
{
    public int drawPrice; // 뽑기 가격
    // public GameObject Soldierprefab;
    private GameObject Soldier;
    public Button SoldierCombineButton;
    public Button SellSoldierButton;

    private void Awake()
    {
        SoldierCombineButton.GetComponent<Button>();
        SellSoldierButton.GetComponent<Button>();
    }

    void Start()
    {
      gameObject.SetActive(true);
    }

    public void DarwSoldier() // 유닛 뽑기
    {
        if(GameManager.instance.Gold >= drawPrice)
        {
            CreatSoldier();
            ExpenditrueGold();
        }
    }

    void CreatSoldier()
    {
        int randomnumber = Random.Range(0, 4);
        Soldier = Instantiate(transform.GetChild(randomnumber).gameObject, SetRandomPosition(10, 0, 10), transform.rotation);
        Soldier.SetActive(true);
    }

    void ExpenditrueGold() // 골드 지출
    {
        GameManager.instance.Gold -= drawPrice;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }


    public void SellSolider() // 유닛 판매
    {
        RemoveSolider();
        IncomeSellSolider();
    } 

    void RemoveSolider()
    {
        SetActiveButton(false);
        Destroy(hitSoldier);
    }

    void IncomeSellSolider() // 판매한 솔져 수익
    {
        TeamSoldier teamSoldier = hitSoldier.GetComponent<TeamSoldier>(); // 클릭한 유닛의 스크립트를 가져옴
        GameManager.instance.Gold += teamSoldier.sellPrice;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }

    public void SetActiveButton(bool show) // 인수로 받은 값을 SetActive에 적용시킴
    {
        SoldierCombineButton.gameObject.SetActive(show);
        SellSoldierButton.gameObject.SetActive(show);
    }

    Vector3 SetRandomPosition(float x, float y, float z)
    {
        float randomX = Random.Range(-x, x);
        float randomY = Random.Range(-y, y);
        float randomZ = Random.Range(-z, z);
        return new Vector3(randomX, randomY, randomZ);
    }

    public void CombineSolider()
    {

        SoldierCombineButton.gameObject.SetActive(false);
        SellSoldierButton.gameObject.SetActive(false);

    }

    GameObject hitSoldier;
    public void Chilk()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject);
                hitSoldier = hit.transform.gameObject; // 클릭한 게임오브젝트를 변수에 담아서 사용할 수 있도록 함
            }
        }
    }
}
