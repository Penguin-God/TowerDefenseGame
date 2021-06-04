using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEvent
{
    void SkillPercentUp();
    void SkillPercentDown();


    void ReinforcePassive();
    void WeakenPassive();
}
