using UnityEngine;

public class GoodsData : MonoBehaviour
{    
    public int price;

    public int unitColorNumber;
    public int unitClassNumber;

    public int buyFoodCount;

    public int buyGoldAmount;

    [Tooltip("0 : 대미지 증가, 1 : 보스 대미지 증가, 2 : 스킬 사용 빈도 증가, 3 : 패시브 강화")]
    public int eventNumber;
    public int eventUnitNumber;
}
