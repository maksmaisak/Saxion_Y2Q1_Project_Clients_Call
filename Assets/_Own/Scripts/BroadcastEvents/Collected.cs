using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collected : BroadcastEvent<Collected> {

    public Collected(Collectable collectable)
    {
        this.collectable = collectable;
    }

    public Collectable collectable { get; }
}
