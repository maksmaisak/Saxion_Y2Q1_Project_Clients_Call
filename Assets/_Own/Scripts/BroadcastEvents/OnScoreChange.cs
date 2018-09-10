using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScoreChange : BroadcastEvent<OnScoreChange>
{
    public readonly int scoreDelta;

    public OnScoreChange(int scoreDelta)
    {
        this.scoreDelta = scoreDelta;
    }
}