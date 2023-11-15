using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MouseOverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    float _tooltipDelay = 0.2f;
    public void SetDelayTime(float delay) => _tooltipDelay = delay;

    Coroutine _showInfoCoroutine;

    public event Action OnPointerEnterEvent;
    public event Action OnPointerEnterDelayedEvent;
    public event Action OnPointerExitEvent;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterEvent?.Invoke();
        if (_showInfoCoroutine != null)
            StopCoroutine(_showInfoCoroutine);
        _showInfoCoroutine = StartCoroutine(ShowTooltipDelayed());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_showInfoCoroutine != null)
        {
            StopCoroutine(_showInfoCoroutine);
            _showInfoCoroutine = null;
        }
        OnPointerExitEvent?.Invoke();
    }

    IEnumerator ShowTooltipDelayed()
    {
        yield return new WaitForSeconds(_tooltipDelay);
        OnPointerEnterDelayedEvent?.Invoke();
    }

    void OnDisable()
    {
        OnPointerExitEvent?.Invoke();
    }
}
