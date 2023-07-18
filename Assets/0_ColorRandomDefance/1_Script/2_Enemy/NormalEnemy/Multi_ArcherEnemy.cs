﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_ArcherEnemy : Multi_NormalEnemy
{
    const float SpeedUpRate = 1.5f;
    protected override void Passive() => _speedManager = new SpeedManager(_speedManager.OriginSpeed * SpeedUpRate);
}
