using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_BattleStartController : UI_Base
{
    [SerializeField] Button _readyButton;

    public void EnterBattle(SkillBattleDataContainer enemySkillData)
    {
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "소환할 몬스터를 선택해 주십시오";
        _readyButton.enabled = false;
        GetComponentInChildren<UI_EquipSkillInfo>().ChangeEquipSkillImages(enemySkillData.MainSkill.SkillType, enemySkillData.SubSkill.SkillType);
    }

    public void ActiveReadyButton(UnityAction onReady)
    {
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "게임을 시작할 준비가 되었다면 이 버튼을 클릭해주세요";
        _readyButton.enabled = true;
        _readyButton.onClick.AddListener(Ready);
        _readyButton.onClick.AddListener(onReady);
    }

    void Ready()
    {
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "준비 완료";
        _readyButton.enabled = false;
    }
}
