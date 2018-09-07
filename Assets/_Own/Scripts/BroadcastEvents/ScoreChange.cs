using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreChange : BroadcastEvent<ScoreChange>
{
    public readonly int scoreDelta;

    public ScoreChange(int scoreDelta)
    {
        this.scoreDelta = scoreDelta;
    }
}