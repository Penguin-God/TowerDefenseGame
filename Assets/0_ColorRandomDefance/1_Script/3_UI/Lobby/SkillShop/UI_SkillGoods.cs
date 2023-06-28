using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillGoods : UI_Base
{
    enum Images
    {
        SkillImage,
    }

    enum Texts
    {
        NameText,
        PriceText,
    }

    enum Buttons
    {
        BuyButton,
    }

    protected override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        _initDone = true;
        RefreshUI();
    }

    UserSkillGoodsData _skillData;
    public void SetInfo(UserSkillGoodsData data)
    {
        _skillData = data;
        RefreshUI();
    }

    void RefreshUI()
    {
        if (_initDone == false || _skillData == null) return;

        GetText((int)Texts.NameText).text = _skillData.SkillName;
        GetText((int)Texts.PriceText).text = $"{new MoneyPresenter().GetKoreaText(_skillData.MoneyType)} {10}개";
        GetImage((int)Images.SkillImage).sprite = _skillData.ImageSprite;

        GetButton((int)Buttons.BuyButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.BuyButton).onClick.AddListener(
            () => Managers.UI.ShowPopupUI<UI_ComfirmPopup>().SetInfo("상품을 구매하시겠습니까?", () => new UserSkillShopUseCase().BuyUserSkill(_skillData.SkillType)));
    }
}
