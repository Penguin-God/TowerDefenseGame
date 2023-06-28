using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTrakerDataModel : MonoBehaviour
{
    [SerializeField] Color[] colors;
    [SerializeField] Sprite[] icons;
    [SerializeField] string[] classNames;

    public UI_UnitTrackerData BuildUnitTrackerData(UnitFlags flag)
        => new UI_UnitTrackerData(flag, icons[flag.ClassNumber], colors[flag.ColorNumber], classNames[flag.ClassNumber]);
}
