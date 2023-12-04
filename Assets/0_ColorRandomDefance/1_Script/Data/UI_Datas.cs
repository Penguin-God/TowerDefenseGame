using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UI_UnitWindowData
{
    [SerializeField] UnitFlags _unitFlags;
    [SerializeField] string _description;

    public UnitFlags UnitFlags => _unitFlags;
    public string Description => _description.Replace("\\n", "\n");
}

[Serializable]
public struct UnitJobTooltipData
{
    [SerializeField] UnitClass _unitClass;
    [SerializeField] string _text;

    public UnitClass UnitClass => _unitClass;
    public string Text => _text.Replace("\\n", "\n");
}