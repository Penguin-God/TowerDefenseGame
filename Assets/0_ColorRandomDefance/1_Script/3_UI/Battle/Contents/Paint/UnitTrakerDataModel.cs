using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct UI_UnitTrackerData
{
    [SerializeField] Sprite _icon;
    [SerializeField] Color _backGroundColor;
    [SerializeField] string _unitClassName;

    public UI_UnitTrackerData(Sprite icon, Color color, string unitClassName)
    {
        _icon = icon;
        _backGroundColor = color;
        _unitClassName = unitClassName;
    }

    public Sprite Icon => _icon;
    public Color BackGroundColor => _backGroundColor;
    public string UnitClassName => _unitClassName;
}

public class UnitTrakerDataModel : MonoBehaviour
{
    [SerializeField] Color[] colors;
    [SerializeField] Sprite[] icons;
    [SerializeField] string[] classNames;

    public UI_UnitTrackerData BuildUnitTrackerData(UnitFlags flag)
        => new UI_UnitTrackerData(icons[flag.ClassNumber], colors[flag.ColorNumber], classNames[flag.ClassNumber]);
}
