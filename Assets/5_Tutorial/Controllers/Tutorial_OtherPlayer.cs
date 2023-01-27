using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_OtherPlayer : TutorialController
{
    protected override void AddTutorials()
    {
        AddReadCommend("이곳은 상대방의 진영입니다");
        AddReadCommend("상대방 역시 자신의 유닛을 뽑아 적군을 막아야 합니다.");
        AddUI_HighLightCommend("왼쪽 위 UI를 통해 상대방의\n유닛 수, 몬스터 수를 확인할 수 있습니다.", "OpponentStatusFocus");
        AddUI_HighLightCommend("또한 오른쪽 위 UI를 통해\n상대방의 스킬을 확인할 수 있습니다", "EquipSkills");
    }

    protected override bool TutorialStartCondition() => Managers.Camera.IsLookOtherWolrd && Managers.Camera.IsLookEnemyTower == false;
}
