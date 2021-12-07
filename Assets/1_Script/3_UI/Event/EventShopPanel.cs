using UnityEngine;
using UnityEngine.UI;

abstract public class EventShopPanel : MonoBehaviour
{
    protected Shop shop = null;
    Button myButton = null;
    [SerializeField] protected string guideText = "";

    private void Start()
    {
        shop = GetComponentInParent<Shop>();
        myButton = GetComponent<Button>();
        // 자기 자신 클릭 시 상점 판넬 Yes 버튼에 함수 추가
        myButton.onClick.AddListener(() => shop.SetPanel(guideText, OnClickYseButton));
    }

    // 판넬에서 Yes 버튼을 클릭 시 할 행동을 정의
    public abstract void OnClickYseButton();
}