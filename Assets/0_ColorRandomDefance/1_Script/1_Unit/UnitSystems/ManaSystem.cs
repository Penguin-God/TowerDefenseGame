﻿using System.Collections;
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

    public bool IsManaFull => _manaUseCase.IsManaFull;

    private Slider manaSlider;
    ManaUseCase _manaUseCase;
    public void SetInfo(int maxMana, int addMana)
    {
        _manaUseCase = new ManaUseCase(maxMana);
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
        photonView.RPC(nameof(AddMana), RpcTarget.All);
    }

    [PunRPC]
    void AddMana()
    {
        _currentMana = _manaUseCase.AddMana(_addMana);
        manaSlider.value = _currentMana;
    }

    public void ClearMana_RPC() => photonView.RPC(nameof(ClearMana), RpcTarget.All);
    [PunRPC]
    void ClearMana()
    {
        _currentMana = 0;
        _manaUseCase.ClearMana();
        manaSlider.value = 0;
    }

    public void LockManaForDuration(float duration) => StartCoroutine(Co_LockManaForDuration(duration));

    IEnumerator Co_LockManaForDuration(float duration)
    {
        manaIsLock = true;
        yield return new WaitForSeconds(duration);
        manaIsLock = false;
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
