using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteUnitEvent : MonoBehaviour
{
    public int classNumber;
    [SerializeField] int unitColorCount = 0;
    [SerializeField] GameObject timerObject;
    GameObject realTimer = null;

    void Awake()
    {
        realTimer = Instantiate(timerObject, timerObject.transform.position, timerObject.transform.rotation);
        realTimer.GetComponent<WhiteUnitTimer>().targetUnit = transform;
    }

    void OnEnable()
    {
        //realTimer.SetActive(true); // 풀링 때문에 비활성화 해놓음
    }

    public void UnitTransform()
    {
        UnitColor _color = (UnitColor)Random.Range(0, unitColorCount);
        string _getUnit = UnitManager.instance.GetUnitKey(_color, (UnitClass)classNumber);
        GameObject _newUnit = CombineSoldierPooling.GetObject(_getUnit, (int)_color, classNumber);
        _newUnit.transform.position = transform.position;

        CombineSoldierPooling.ReturnObject(gameObject, transform.GetChild(0).gameObject.tag);

        SoundManager.instance.PlayEffectSound_ByName("TransformWhiteUnit");
    }
}
