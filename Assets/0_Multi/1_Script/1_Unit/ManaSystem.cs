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

    public void AddMana_RPC() => photonView.RPC("AddMana", RpcTarget.All);
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
