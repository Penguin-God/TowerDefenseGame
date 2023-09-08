using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler
{
    bool MouseOverUI() => EventSystem.current.IsPointerOverGameObject();

    public bool MouseClickRayCastHit(out RaycastHit hit, int layerMask = ~0)
    {
        hit = new RaycastHit();
        if (Input.GetMouseButtonDown(0) && MouseOverUI() == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
        }
        else return false;
    }
}
