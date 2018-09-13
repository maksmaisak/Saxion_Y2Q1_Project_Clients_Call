using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPowerUpCollected : BroadcastEvent<OnPowerUpCollected> {

    public OnPowerUpCollected(PowerUpInfo info)
    {
        powerUpInfo = info;
    }

    public PowerUpInfo powerUpInfo { get; }
}
