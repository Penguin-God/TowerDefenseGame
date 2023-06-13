using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tutorial_Tower : TutorialController
{
    readonly UnitFlags orangeSowrdmanFlag = new UnitFlags(4, 0);
    protected override void Init() => ChangeShopColliderEnable(false);

    protected override void AddTutorials()
    {
        AddReadCommend("여기는 적군의 성이 있는 진영입니다.");
        AddUI_HighLightCommend("가운데에 있는 성을 파괴하면 보상을 얻을 수 있습니다.", "TowerFocus");
        AddUI_HighLightCommend("아래에 있는 상점에서는 특수 유닛을 소환하거나\n유닛의 최대 개수를 늘릴 수 있습니다.", "ShopFocus");
        AddClickCommend("버튼을 클릭해서 돌아간 후\n이곳에 병력을 보내는 법을 알아봅시다.", "StoryWolrd_EnterButton");
        AddActionCommend(() => Multi_SpawnManagers.NormalUnit.Spawn(orangeSowrdmanFlag));
        AddUnitHighLightCommend("적군의 성에 유닛을 보내기 위해\n제가 주황 기사라는 작은 선물을 드렸습니다", orangeSowrdmanFlag);
        AddReadCommend("주황 유닛의 고유 능력은 타워와 보스 공격력 증가이기 때문에\n안성맞춤인 선물이라고 할 수 있죠");
        AddReadCommend("그럼 이제 주황 기사를 적군의 성으로 보내 봅시다");
        AddUnitHighLightCommend("필드에 있는 유닛을 클릭하세요", orangeSowrdmanFlag, CheckOrangeSowrdmanClick);
        AddClickCommend("적군의 성으로 버튼을 클릭하면\n해당 유닛이 성으로 이동 후 공격합니다.", "Unit_World_Changed_Button");
        AddClickCommend("적군의 성으로 가서 보낸 유닛을 확인해보세요.", "StoryWolrd_EnterButton");
        AddReadCommend("이렇게 유닛을 보내 성을 부수면 보상으로\n고기와 골드를 획득할 수 있습니다.");
        AddActionCommend(() => ChangeShopColliderEnable(true));
    }

    protected override bool TutorialStartCondition() => Managers.Camera.IsLookEnemyTower && Managers.Camera.IsLookOtherWolrd == false;
    bool CheckOrangeSowrdmanClick()
    {
        var window = Managers.UI.FindPopupUI<UI_UnitManagedWindow>();
        if (window == null) return false;
        return window.UnitFlags == orangeSowrdmanFlag;
    }
    void ChangeShopColliderEnable(bool isEnabled)
    {
        FindObjectsOfType<ShopObject>()
            .Select(x => x.GetComponent<BoxCollider>())
            .ToList()
            .ForEach(x => x.enabled = isEnabled);
    }
}
