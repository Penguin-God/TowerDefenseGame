using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamblePanel : UI_Base
{
    enum GameObjects
    {
        GachaItemParent,
    }

    enum Buttons
    {
        GachaButton,
    }

    string[] rateTables;
    protected override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        rateTables = Resources.Load<TextAsset>("Data/SkillData/GamblerUnitGachaRate").text.Split('\n').Skip(1).SkipLast(1).ToArray();
    }

    TextShowAndHideController _textController;
    public void Inject(TextShowAndHideController textController) => _textController = textController;

    double[] GetRates() => rateTables[_gambleLevel - 1]
        .Split(',')
        .Select(x => x.Trim())
        .Where(x => string.IsNullOrEmpty(x) == false)
        .Select(x => double.Parse(x)).ToArray();

    int _gambleLevel = 1;
    public event Action OnGamble = null;

    public void SetupGamblePanel()
    {
        CheckInit();
        foreach (Transform child in GetObject((int)GameObjects.GachaItemParent).transform)
            Destroy(child.gameObject);

        double[] rates = GetRates();
        for (int i = 0; i < rates.Length; i++)
            Managers.UI.MakeSubItem<UI_UnitGachaItemInfo>(GetObject((int)GameObjects.GachaItemParent).transform).ShowInfo((UnitClass)i, rates[i]);
        
        GetButton((int)Buttons.GachaButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.GachaButton).onClick.AddListener(() => UnitGacha(rates));
    }

    void UnitGacha(double[] rates)
    {
        UnitFlags selectUnitFlag = new UnitFlags(UnitFlags.NormalColors.ToList().GetRandom(), (UnitClass)new GachaMachine().SelectIndex(rates));
        Multi_SpawnManagers.NormalUnit.Spawn(selectUnitFlag);
        OnGamble?.Invoke();
        if(rateTables.Length > _gambleLevel)
            _gambleLevel++;
        _textController.ShowTextForTime(BuildGameResultText(selectUnitFlag), new Vector2(0, 100));
        GetButton((int)Buttons.GachaButton).onClick.RemoveAllListeners();
        Managers.Sound.PlayEffect(EffectSoundType.DrawSwordman);
    }

    string BuildGameResultText(UnitFlags flag) => $"{UnitTextPresenter.DecorateBefore(UnitTextPresenter.GetUnitNameWithColor(flag), flag)} »Ì¾Ò½À´Ï´Ù.";
}
