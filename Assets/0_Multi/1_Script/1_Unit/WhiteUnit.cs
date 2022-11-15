using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WhiteUnit : MonoBehaviour
{
    [SerializeField] int classNumber;
    [SerializeField] int maxColor = 5;
    [SerializeField] float aliveTime;
    Multi_WhiteUnitTimer timer = null;

    void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        timer = Multi_SpawnManagers.Effect.ShwoForTime(Effects.WhiteUnit_Timer, transform.position, aliveTime).GetComponent<Multi_WhiteUnitTimer>();
        timer.Setup_RPC(transform, aliveTime);
        timer.Slider.onValueChanged.AddListener(ChangedColor);
    }

    public void ChangedColor(float value)
    {
        if(value <= 0)
        {
            var unit = GetComponent<Multi_TeamSoldier>();
            Multi_SpawnManagers.NormalUnit.Spawn(Random.Range(0, maxColor), (int)unit.unitClass, unit.transform.position, unit.transform.rotation, unit.UsingID);
            unit.Dead();
            timer.Off();
            timer = null;
        }
    }
}
