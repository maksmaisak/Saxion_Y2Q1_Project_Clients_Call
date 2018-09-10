using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.UIElements;

public class ScoreBonusCollectable : Collectable
{
    [SerializeField] int scoreBonus = 10;
    [Header("Disappear animation parameters")]
    [SerializeField] [Range(0.01f, 1f)] float animationDuration = 0.25f;
    [SerializeField] [Range(0f   , 5f)] float jumpHeight = 3f;
    [SerializeField] [Range(0f   , 5f)] float jumpPower  = 1f;
    
    protected override void OnCollected()
    {
        new OnScoreChange(scoreBonus).PostEvent();

        DOTween
            .Sequence()
            .Join(transform.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack))
            .Join(transform.DOLocalJump(Vector3.up * jumpHeight, jumpPower, 1, animationDuration).SetRelative())
            .AppendCallback(() => Destroy(gameObject));
    }
}