using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.UIElements;

public class ScoreBonusCollectable : Collectable
{
    [Space]
    [SerializeField] int scoreBonus = 10;
    [SerializeField] StandardCollectableAnimationSettings animations;
    
    protected override void OnCollected()
    {
        new OnScoreChange(scoreBonus).PostEvent();

        animations
            .PlayDisappearAnimation(gameObject)
            .AppendCallback(() => Destroy(gameObject));
    }
}