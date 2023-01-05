using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Basic : TutorialController
{
    protected override void AddTutorials()
    {
        AddReadCommend("컬러 랜덤 디펜스에 오신 것을 환영합니다!!");
        AddReadCommend("이 게임은 상대방보다 먼저 필드의 몬스터 수가 50이 넘으면 패배하는 \"버티기\" 게임입니다.");

        var explainMonster = CreateComposite();
        explainMonster.AddCommend(CreateReadCommend("게임이 시작하면 적 유닛이 나옵니다"));
        explainMonster.AddCommend(CreateSpotLightCommend(new Vector3(-45, 5, 35)));
        AddCommend(explainMonster);

        AddReadCommend("몬스터는 유닛을 뽑아서 처치할 수 있습니다.");
        AddReadCommend("버튼을 눌러 유닛을 뽑아보세요");
        // add Button 넣어야 함

        var explainUnit = CreateComposite();
        explainUnit.AddCommend(CreateReadCommend("유닛을 뽑으면 5골드가 소모되며\n빨간, 파란, 노란 기사 중 무작위로 하나를 획득합니다."));
        explainUnit.AddCommend(CreateSpotLightCommend(new Vector3(0, 5, 0)));
        AddCommend(explainUnit);

        var highLight_UI = CreateComposite();
        highLight_UI.AddCommend(CreateReadCommend("옆에 보이는 버튼들을 이용해 획득한 유닛들을\n관리할 수 있습니다."));
        highLight_UI.AddCommend(CreateUI_HighLightCommend("Paint"));

        AddReadCommend("이제 알려준 내용을 이용해서 잠시  게임을 플레이해보세요!!\n때가 되면 다시 돌아오겠습니다.");
    }
}
