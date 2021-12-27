using UnityEngine;
using UnityEngine.UI;

public class TowerShopWindow : MonoBehaviour
{
    delegate void On_Sell();
    On_Sell Sell;

    private void Awake()
    {
        Sell = () => UnitManager.instance.ExpendMaxUnit(3);
    }

    int LimitCount = 0; // 유닛 최대 갯수 제한
    public void SpendFood()
    {
        if(GameManager.instance.Food >= price && LimitCount <= 4)
        {
            GameManager.instance.Food -= price;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
            Sell();
            UpdatePrice();
            LimitCount += 1;
        }
    }

    int price = 5;
    [SerializeField] Text text = null;
    void UpdatePrice()
    {
        price = price + 3;
        text.text = string.Format("구입 시 유닛의 최대 갯수를 늘릴 수 있습니다. \n\n 가격: 식량 {0}개", price);
    }
}
