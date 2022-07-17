using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AutoEffectDisabled : MonoBehaviour
{
	public float showTime = 0.5f;
	void OnEnable()
    {
		if (PhotonNetwork.IsMasterClient == false) return;
		StartCoroutine("CheckIfAlive");
	}

	IEnumerator CheckIfAlive()
	{
		yield return new WaitForSeconds(showTime);

		if (GetComponent<Poolable>() != null)
			Multi_Managers.Pool.Push(GetComponent<Poolable>());
		else
			gameObject.GetComponent<RPCable>().SetActive_RPC(false);
	}
}
