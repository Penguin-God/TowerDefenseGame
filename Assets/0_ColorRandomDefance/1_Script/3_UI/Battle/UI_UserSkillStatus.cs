using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_UserSkillStatus : UI_Scene
{
    TextMeshProUGUI _text;
    protected override void Init()
    {
        base.Init();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        UpdateKillCount();
    }

    Necromencer _necromencer;
    public void Injction(Necromencer necromencer) => _necromencer = necromencer;

    public void UpdateKillCount()
    {
        _text.text = $"{_necromencer.CurrentKillCount}/{_necromencer.NeedKillCountForSummon}";
    }
}
