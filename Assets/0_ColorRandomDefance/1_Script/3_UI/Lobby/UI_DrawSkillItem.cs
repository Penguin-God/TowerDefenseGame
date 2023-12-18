using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DrawSkillItem : UI_Base
{
    [SerializeField] Image _skillImage;
    [SerializeField] TextMeshProUGUI _skillAmount;

    public void ShowDrawSkill(SkillAmountData skillAmountData)
    {
        _skillImage.sprite = SpriteUtility.GetSkillImage(skillAmountData.SkillType);
        _skillAmount.text = $"X{skillAmountData.Amount}";
    }
}
