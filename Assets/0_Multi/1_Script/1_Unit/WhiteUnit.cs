using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WhiteUnit : MonoBehaviourPun
{
    [SerializeField] int classNumber;
    [SerializeField] int maxColor = 5;
    [SerializeField] float aliveTime;
    Multi_WhiteUnitTimer timer = null;

    void OnEnable()
    {
        timer = Managers.Effect.TrackingToTarget("WhiteUnitTimer", transform, new Vector3(0, 4, 3)).GetComponent<Multi_WhiteUnitTimer>();
        timer.Setup(transform, aliveTime);

        if (PhotonNetwork.IsMasterClient == false) return;
        timer.Slider.onValueChanged.AddListener(ChangedColor);
    }

    void OnDisable()
    {
        timer.Off();
        timer = null;
    }

    public void ChangedColor(float value)
    {
        if(value <= 0)
        {
            var unit = GetComponent<Multi_TeamSoldier>();
            Multi_UnitManager.Instance.ColorChangeHandler.ChangeUnitColor(photonView.ViewID);
            //Multi_SpawnManagers.NormalUnit.Spawn(Random.Range(0, maxColor), (int)unit.unitClass, unit.transform.position, unit.transform.rotation, unit.UsingID);
            //unit.Dead();
        }
    }
}
