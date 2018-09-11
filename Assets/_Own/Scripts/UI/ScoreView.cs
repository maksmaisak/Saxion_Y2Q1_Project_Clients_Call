using System.Collections;
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

    private int currentScore;

    void Start()
    {
        Assert.IsNotNull(textMesh);
    }

    public void On(OnScoreChange message)
    {
        currentScore = WorldRepresentation.Instance.playerScore += message.scoreDelta;
        UpdateText();
    }

    private void UpdateText()
    {
        textMesh.text = $"Score: {currentScore}";

        textMesh.rectTransform.DOKill(complete: true);
        textMesh.rectTransform.DOPunchScale(Vector3.one * animationScale, animationDuration, animationVibrato);
    }
}