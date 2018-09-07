using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollected : BroadcastEvent<PowerUpCollected> {

    public PowerUpCollected(PowerUpInfo info)
    {
        powerUpInfo = info;
    }

    public PowerUpInfo powerUpInfo { get; }
}
