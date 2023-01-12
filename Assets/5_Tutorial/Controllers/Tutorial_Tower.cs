using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Tower : TutorialController
{
    UnitFlags orangeSowrdmanFlag = new UnitFlags(4, 0);
    protected override void AddTutorials()
    {
        AddReadCommend("여기는 적군의 성이 있는 진영입니다.");
        AddUI_HighLightCommend("가운데에 있는 성을 부셔서 보상을 얻기도 하고", "TowerFocus");
        AddUI_HighLightCommend("상점에서 여러가지 상품을 사기도 하는 곳입니다.", "ShopFocus");
        AddClickCommend("버튼을 클릭해서 돌아간 후\n이곳에 병력을 보내는 법을 알아봅시다.", "StoryWolrd_EnterButton");
        AddActionCommend(() => Multi_SpawnManagers.NormalUnit.Spawn(orangeSowrdmanFlag));
        AddSpotLightActionCommend("적군의 성에 유닛을 보내기 위해\n제가 주황 기사라는 작은 선물을 드렸습니다"
            , () => Multi_UnitManager.Instance.FindUnit(0, orangeSowrdmanFlag).transform.position + new Vector3(0, 5, 0));
        AddReadCommend("주황 유닛의 고유 능력은 보스 공격력 증가이기 때문에\n안성맞춤인 선물이라고 할 수 있죠");
        AddReadCommend("그럼 이제 주황 기사를 적군의 성으로 보내 봅시다");
        AddClickCommend("페인트를 클릭하고", "PaintButton");
        AddClickCommend("방패를 클릭하세요", "ClassButtonBackGround");
        AddClickCommend("기사 모양을 클릭하세요.", "SwordmanButton");
        AddClickCommend("보유 중인 색깔의 기사를 클릭하세요.", "Orange");
        AddClickCommend("적군의 성으로 버튼을 클릭하면\n해당 유닛이 적군의 성으로 이동해 성을 공격합니다.", "Unit World Changed Button");
        AddClickCommend("적군의 성으로 가서 보낸 유닛을 확인하세요.", "StoryWolrd_EnterButton");
        AddReadCommend("이제 성이 부서지면 보상을 획득할 수 있습니다.\n하지만 적군의 성은 매우 단단하니\n잘 생각하고 유닛을 보내시기 바랍니다.");
    }

    protected override bool TutorialStartCondition() => Managers.Camera.IsLookEnemyTower && Managers.Camera.IsLookOtherWolrd == false;
}
