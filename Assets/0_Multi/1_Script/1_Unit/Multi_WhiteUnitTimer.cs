using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Multi_WhiteUnitTimer : MonoBehaviourPun
{
    [SerializeField] Vector3 offSet;

    private Slider slider;
    public Slider Slider => slider;
    
    Transform target;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Setup_RPC(Transform unit, float aliveTime)
    {
        target = unit;
        photonView.RPC("Setup", RpcTarget.All, aliveTime);
    }

    [PunRPC]
    void Setup(float aliveTime)
    {
        slider.maxValue = aliveTime;
        slider.value = aliveTime;
        StartCoroutine(Co_Timer());
    }

    public void Off()
    {
        GetComponent<RPCable>().SetActive_RPC(false);
        slider.onValueChanged.RemoveAllListeners();
    }

    IEnumerator Co_Timer()
    {
        while (true)
        {
            if (target != null)
                transform.position = target.position + offSet;
            slider.value -= Time.deltaTime;
            yield return null;
        }
    }
}
