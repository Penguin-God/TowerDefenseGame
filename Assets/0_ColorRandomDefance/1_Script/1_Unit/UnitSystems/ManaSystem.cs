using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaSystem : MonoBehaviour
{
    [SerializeField] int _addManaAmount;
    public bool IsManaFull => _manaUseCase.IsManaFull;

    private Slider manaSlider;
    ManaUseCase _manaUseCase;
    public void SetInfo(int maxMana, int addMana)
    {
        _manaUseCase = new ManaUseCase(maxMana);
        manaSlider = transform.GetComponentInChildren<Slider>();

        _addManaAmount = addMana;
        manaSlider.maxValue = maxMana;
    }

    public void AddMana() => manaSlider.value = _manaUseCase.AddMana(_addManaAmount);

    public void ClearMana()
    {
        _manaUseCase.ClearMana();
        manaSlider.value = 0;
    }

    public void LockManaForDuration(float duration) => StartCoroutine(Co_LockManaForDuration(duration));

    IEnumerator Co_LockManaForDuration(float duration)
    {
        _manaUseCase.LockMana();
        yield return new WaitForSeconds(duration);
        _manaUseCase.ReleaseMana();
    }
}
