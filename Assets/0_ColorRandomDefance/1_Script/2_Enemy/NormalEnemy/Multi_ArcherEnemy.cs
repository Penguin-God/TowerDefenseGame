using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_ArcherEnemy : Multi_NormalEnemy
{
    protected override void Passive()
    {
        ChangeMaxSpeed(maxSpeed * 1.5f);
    }
}
