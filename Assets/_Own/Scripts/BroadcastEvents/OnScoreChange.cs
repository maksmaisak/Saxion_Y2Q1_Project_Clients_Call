using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScoreChange : BroadcastEvent<OnScoreChange>
{
    public readonly int scoreDelta;
    public readonly bool playBumpEffect;

    public OnScoreChange(int scoreDelta, bool playBumpEffect = true)
    {
        this.scoreDelta = scoreDelta;
        this.playBumpEffect = playBumpEffect;
    }
}