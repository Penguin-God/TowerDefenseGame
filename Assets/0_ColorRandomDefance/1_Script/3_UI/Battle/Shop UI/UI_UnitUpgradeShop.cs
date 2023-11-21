using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_UnitUpgradeShop : UI_Popup
{
    enum GameObjects
    {
        GoldGoodsParent,
        RuneGoodsParent,
    }

    enum Texts
    {
        ValueUpgradeInfoText,
        ScaleUpgradeInfoText,
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));

        CreateGoods(GetObject((int)GameObjects.GoldGoodsParent).transform, _unitUpgradeDataUseCase.AddData);
        CreateGoods(GetObject((int)GameObjects.RuneGoodsParent).transform, _unitUpgradeDataUseCase.ScaleData);

        GetTextMeshPro((int)Texts.ValueUpgradeInfoText).text = UnitUpgradeGoodsPresenter.BuildAddUpgradeText(_unitUpgradeDataUseCase.AddData);
        GetTextMeshPro((int)Texts.ScaleUpgradeInfoText).text = UnitUpgradeGoodsPresenter.BuildScaleUpgradeText(_unitUpgradeDataUseCase.ScaleData);
    }

    MultiUnitStatController _statController;
    ShopDataUseCase _unitUpgradeDataUseCase;
    public void DependencyInject(MultiUnitStatController statController, ShopDataUseCase unitUpgradeDataUseCase)
    {
        _statController = statController;
        _unitUpgradeDataUseCase = unitUpgradeDataUseCase;
    }

    void CreateGoods(Transform goodsParent, UnitUpgradeGoodsData unitUpgradeGoodsData)
    {
        foreach (Transform child in goodsParent)
            Destroy(child.gameObject);

        foreach (var color in UnitFlags.NormalColors)
            Managers.UI.MakeSubItem<UI_UnitUpgradeIcon>(goodsParent).FillGoods(color, unitUpgradeGoodsData, _statController, _unitUpgradeDataUseCase.UpgradeMaxLevel);
    }
}
