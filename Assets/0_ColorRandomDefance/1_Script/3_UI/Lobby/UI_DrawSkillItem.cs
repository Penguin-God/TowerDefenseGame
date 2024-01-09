using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DrawSkillItem : UI_Base
{
    [SerializeField] Image _skillImage;
    [SerializeField] TextMeshProUGUI _skillAmount;

    public void ShowDrawSkill(SkillAmountData skillAmountData) => ViewAmount(SpriteUtility.GetSkillImage(skillAmountData.SkillType), skillAmountData.Amount.ToString());

    public void ViewAmount(Sprite sprite, string text)
    {
        _skillImage.sprite = sprite;
        _skillAmount.text = $"X{text}";
    }
}
