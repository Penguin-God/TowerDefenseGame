using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEffectDisabled : MonoBehaviour
{
	public float showTime = 0.5f;
	void OnEnable() => StartCoroutine("CheckIfAlive");

	IEnumerator CheckIfAlive()
	{
		yield return new WaitForSeconds(showTime);
		this.gameObject.SetActive(false);
	}
}
