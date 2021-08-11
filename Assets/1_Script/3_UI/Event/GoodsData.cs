using UnityEngine;

public class GoodsData : MonoBehaviour
{    
    public int price;
    [Tooltip("선언한 텍스트 뒤에 ' 구입하시겠습니까?'라는 문구가 붙음 ")]
    public string goodsInformation;

    public int unitColorNumber;
    public int unitClassNumber;

    public int buyFoodCount;

    public int buyGoldAmount;

    [Tooltip("0 : 대미지 증가, 1 : 보스 대미지 증가, 2 : 스킬 사용 빈도 증가, 3 : 패시브 강화")]
    public int reinforceEventNumber;
    public int eventUnitNumber;

    public int eventNumber;

    public int ultimateMageNumber;
}
