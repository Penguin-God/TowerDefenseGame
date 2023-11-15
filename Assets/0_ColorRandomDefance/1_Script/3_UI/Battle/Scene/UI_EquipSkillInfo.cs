using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipSkillInfo : UI_Base
{
    enum Images
    {
        MainSkill,
        SubSkill,
    }

    protected override void Init()
    {
        Bind<Image>(typeof(Images));
    }

    public void ChangeEquipSkillImages(SkillType mainSkill, SkillType subSkill)
    {
        CheckInit();
        ChangeSkill_Image(mainSkill, (int)Images.MainSkill);
        ChangeSkill_Image(subSkill, (int)Images.SubSkill);
    }

    void ChangeSkill_Image(SkillType skill, int imageIndex)
    {
        if (skill == SkillType.None)
            GetImage(imageIndex).color = new Color(1, 1, 1, 0);
        else
        {
            GetImage(imageIndex).sprite = Managers.Data.UserSkill.GetSkillGoodsData(skill).ImageSprite;
            GetImage(imageIndex).color = new Color(1, 1, 1, 1);
        }
    }
}
