using UnityEngine;

public class UI_NotifySkillDrawWindow : UI_Popup
{
    [SerializeField] Transform _skillInfoParnet;

    public void ShowSkillsInfo(SkillDrawResultData drawResult)
    {
        foreach (Transform child in _skillInfoParnet)
            Destroy(child.gameObject);

        foreach (var data in drawResult.DrawSkills)
            Managers.UI.MakeSubItem<UI_DrawSkillItem>(_skillInfoParnet).ShowDrawSkill(data);


        if(drawResult.RewardGoldWhenDrawOver > 0)
            Managers.UI.MakeSubItem<UI_DrawSkillItem>(_skillInfoParnet).ViewAmount(SpriteUtility.GetGoldImage(), drawResult.RewardGoldWhenDrawOver.ToString("N0"));

        if (drawResult.RewardGoldWhenDrawOver > 0)
            print(drawResult.RewardGoldWhenDrawOver.ToString("N0"));
    }
}
