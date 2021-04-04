using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DefenserManager : MonoBehaviour
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
        if(GameManager.instance.Gold >= 5)
        {
            CreatSoldier();
            ExpenditrueGold();
        }
    }

    Vector3 SetRandomPosition(float x, float y, float z)
    {
        float randomX = Random.Range(-x, x);
        float randomY = Random.Range(-y, y);
        float randomZ = Random.Range(-z, z);
        return new Vector3(randomX, randomY, randomZ);
    }

    void CreatSoldier()
    {
        // Soldier = transform.GetChild(randomnumber).gameObject;
        int randomnumber = Random.Range(0, 4);
        Soldier = Instantiate(transform.GetChild(randomnumber).gameObject, SetRandomPosition(10, 0, 10), transform.rotation);
        Soldier.SetActive(true);
    }

    void ExpenditrueGold() // 골드 지출
    {
        GameManager.instance.Gold -= drawPrice;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }


    public void CombineSolider()
    {

        SoldierCombineButton.gameObject.SetActive(false);
        SellSoldierButton.gameObject.SetActive(false);

    }


    public void SellSolider()
    {
        TeamSoldier teamSoldier = GameManager.instance.hitSoldier.GetComponent<TeamSoldier>();
        GameManager.instance.Gold += teamSoldier.sellPrice;
        SetActiveButton(false);
        Destroy(GameManager.instance.hitSoldier);

        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }

    public void SetActiveButton(bool show)
    {
        SoldierCombineButton.gameObject.SetActive(show);
        SellSoldierButton.gameObject.SetActive(show);
    }
}
