﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Multi_ArcherEnemy : Multi_NormalEnemy
{
    protected override void Passive()
    {
        speed *= 1.5f;
    }
}
