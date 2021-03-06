﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ScoreView : MyBehaviour, IEventReceiver<OnScoreChange>
{
    [SerializeField] TMP_Text textMesh;
    [Space] 
    [SerializeField] float animationScale    = 0.5f;
    [SerializeField] float animationDuration = 0.2f;
    [SerializeField] int   animationVibrato  = 10;

    void Start()
    {
        Assert.IsNotNull(textMesh);
        UpdateText();
    }

    public void On(OnScoreChange message)
    {
        // Do next frame in case global state gets the message later than this.
        this.DoNextFrame(UpdateText);
        
        if (message.playBumpEffect) PlayBumpEffect();
    }

    private void UpdateText()
    {
        int score = GlobalState.instance.playerScore;
        string scoreString = score > 99 ? score.ToString() : score.ToString("D2");
        textMesh.text = "Score: " + scoreString;
    }

    private void PlayBumpEffect()
    {
        textMesh.rectTransform.DOKill(complete: true);
        textMesh.rectTransform.DOPunchScale(Vector3.one * animationScale, animationDuration, animationVibrato);
    }
}