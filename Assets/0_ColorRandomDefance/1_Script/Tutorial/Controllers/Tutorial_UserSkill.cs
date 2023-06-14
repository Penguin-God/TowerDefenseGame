using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_UserSkill : TutorialController
{
    readonly UnitFlags yellowSowrdmanFlag = new UnitFlags(2, 0);
    readonly TaegeukStateManager _taegeukStateManager = new TaegeukStateManager();

    protected override void Init()
    {
        _swordmanGachaController.ChangeUnitSummonMaxColor(UnitColor.Blue);
        StartCoroutine(Co_MaxUnitChange());
    }

    SwordmanGachaController _swordmanGachaController;
    public void Injection(SwordmanGachaController swordmanGachaController) => _swordmanGachaController = swordmanGachaController;

    protected override void AddTutorials()
    {
        AddReadCommend("태극 스킬 발동을 취소하셨군요?");
        AddReadCommend("태극은 동일한 클래스 내에서 빨간, 파란색 유닛만\n존재할 때 버프를 주는 스킬입니다");
        AddReadCommend("지금처럼 기사에게 적용된 경우\n대미지가 25에서 275로 증가하게 됩니다.");
        AddReadCommend("하지만 지금처럼 빨강, 파랑이 아닌\n다른 색깔의 기사가 소환된다면 버프는 사라집니다");
        AddReadCommend("이 경우 유닛을 판매해 다시 태극을 발동시킬 수 있습니다.");
        AddReadCommend("그럼 저 불순물을 팔아보도록 합시다");

        AddUnitHighLightCommend("필드에 있는 유닛을 클릭하세요", yellowSowrdmanFlag, CheckYellowSowrdmanClick);
        AddClickCommend("판매 버튼을 클릭해 불순물을 팔아보도록 합시다", "UnitSellButton");
        AddReadCommend("유닛을 판매해 빨강, 파랑 색깔의\n기사들만 존재하게 됐으므로 다시 태극이 발동되었습니다.");
        AddUI_HighLightCommend("이처럼 태극은 유닛을 판매할 일이 많다보니\n서브 스킬로 판매 보상 강화를 드는 경우가 많습니다.", "SubSkill");
        AddReadCommend("이 외에도 다양한 스킬이 있으니\n여러 조합을 시도해보면서 게임을 즐기시기 바랍니다.");
    }

    protected override bool TutorialStartCondition() => CheckOnTeaguke();
    bool CheckYellowSowrdmanClick()
    {
        var window = Managers.UI.FindPopupUI<UI_UnitManagedWindow>();
        if (window == null) return false;
        return window.UnitFlags == yellowSowrdmanFlag;
    }
    bool CheckOnTeaguke() 
        => _taegeukStateManager.GetTaegeukState(UnitClass.Swordman, Managers.Unit.ExsitUnitFlags).ChangeState == TaegeukStateChangeType.TrueToFalse
            && Managers.Unit.ExsitUnitFlags.Contains(yellowSowrdmanFlag);

    IEnumerator Co_MaxUnitChange()
    {
        yield return new WaitUntil(() => new TaegeukConditionChecker().CheckTaegeuk(UnitClass.Swordman, Managers.Unit.ExsitUnitFlags));
        _swordmanGachaController.ChangeUnitSummonMaxColor(UnitColor.Yellow);
    }

    //void OnDestroy()
    //{
    //    Managers.ClientData.EquipSkillManager.ChangedEquipSkill(UserSkillClass.Main, SkillType.None);
    //    Managers.ClientData.EquipSkillManager.ChangedEquipSkill(UserSkillClass.Sub, SkillType.None);
    //}
}
