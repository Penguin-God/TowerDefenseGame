using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataContainer/ThrowSpearData")]
public class ThrowSpearDataContainer : ScriptableObject
{
    [SerializeField] bool _isMagic;
    public bool IsMagicSpear { get => _isMagic; private set => _isMagic = value; }

    [SerializeField] Vector3 _rotateVector;
    public Vector3 RotateVector { get => _rotateVector; private set => _rotateVector = value; }

    [SerializeField] float _waitForVisibilityTime;
    public float WaitForVisibilityTime { get => _waitForVisibilityTime; private set => _waitForVisibilityTime = value; }

    public float AttackRate { get; private set; }
    public ThrowSpearDataContainer ChangeAttackRate(float rate)
    {
        var result = GetClone();
        result.AttackRate = rate;
        return result;
    }

    ThrowSpearDataContainer GetClone()
    {
        ThrowSpearDataContainer clone = Instantiate(this);
        clone.RotateVector = this.RotateVector;
        clone.WaitForVisibilityTime = this.WaitForVisibilityTime;
        clone.AttackRate = this.AttackRate;
        return clone;
    }
}
