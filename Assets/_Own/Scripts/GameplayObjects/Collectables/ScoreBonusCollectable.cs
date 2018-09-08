using UnityEngine;
using DG.Tweening;

public class ScoreBonusCollectable : Collectable
{
    [SerializeField] int scoreBonus = 10;
    [Space]
    [SerializeField] [Range(0.01f, 10f)] float disappearAnimationDuration = 0.2f;
    
    public override void OnCollected()
    {
        new ScoreChange(scoreBonus).PostEvent();

        DOTween
            .Sequence()
            .Join(transform.DOScale(Vector3.zero, disappearAnimationDuration).SetEase(Ease.InBack))
            .Join(transform.DOLocalJump(Vector3.up * 2f, 1f, 1, disappearAnimationDuration).SetRelative())
            .AppendCallback(() => Destroy(gameObject));
    }
}