using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitWolrdChangedButton : MonoBehaviour
{
    [SerializeField] UnitFlags _flag;
    [SerializeField] Button button;
    [SerializeField] Text text;
    public void Setup(UnitFlags flag)
    {
        _flag = flag;

        text.text = (Multi_Managers.Camera.IsLookEnemyTower) ? "월드로" : "적군의 성으로";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(UnitWorldChanged);
    }

    void UnitWorldChanged() => Multi_UnitManager.Instance.UnitWorldChanged_RPC(Multi_Data.instance.Id, _flag);
}
