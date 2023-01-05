using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tutorial_GameStart : MonoBehaviour, ITutorial
{
    public void EndAction()
    {
        throw new System.NotImplementedException();
    }

    public bool EndCondition() => Input.GetMouseButtonUp(0);

    public void TutorialAction() => StartCoroutine(Co_TutoialActoin());

    IEnumerator Co_TutoialActoin()
    {
        Multi_GameManager.instance.GameStart();
        yield return new WaitForSecondsRealtime(0.1f);
        FindObjectOfType<TutorialFuntions>().OffLigth();
        FindObjectOfType<TutorialFuntions>().Set_SpotLight(FindObjectsOfType<Multi_NormalEnemy>().Where(x => x.UsingId == 0).FirstOrDefault().transform.position);
    }
}
