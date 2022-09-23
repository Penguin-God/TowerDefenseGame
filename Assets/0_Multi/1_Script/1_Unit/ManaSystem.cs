using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ManaSystem : MonoBehaviourPun
{
    [SerializeField] int _maxMana;
    [SerializeField] int _addMana;
    [SerializeField] int _currentMana;

    [SerializeField] bool manaIsLock = false;
    public void LockMana() => manaIsLock = true;
    public void ReleaseMana() => manaIsLock = false;

    public bool IsManaFull => _currentMana >= _maxMana;

    private Slider manaSlider;
    public void SetInfo(int maxMana, int addMana)
    {
        canvasRectTransform = transform.GetComponentInChildren<RectTransform>();
        manaSlider = transform.GetComponentInChildren<Slider>();

        _maxMana = maxMana;
        _addMana = addMana;
        manaSlider.maxValue = maxMana;
        manaSlider.value = _currentMana;

        StopAllCoroutines();
        StartCoroutine(Co_SetCanvas());
    }

    public void AddMana_RPC()
    {
        if (manaIsLock) return;
        photonView.RPC("AddMana", RpcTarget.All);
    }

    [PunRPC]
    void AddMana()
    {
        _currentMana += _addMana;
        manaSlider.value = _currentMana;
    }

    public void ClearMana_RPC() => photonView.RPC("ClearMana", RpcTarget.All);
    [PunRPC]
    void ClearMana()
    {
        _currentMana = 0;
        manaSlider.value = 0;
    }

    private RectTransform canvasRectTransform;
    Vector3 sliderDir = new Vector3(90, 0, 0);
    IEnumerator Co_SetCanvas()
    {
        while (true)
        {
            canvasRectTransform.rotation = Quaternion.Euler(sliderDir);
            yield return null;
        }
    }
}
