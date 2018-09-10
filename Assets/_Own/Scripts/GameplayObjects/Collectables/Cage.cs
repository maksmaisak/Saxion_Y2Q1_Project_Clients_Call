using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : Collectable
{    
    protected override void OnCollected()
    {
        new OnCageOpen().PostEvent();
        // Play Animation.
    }
}
