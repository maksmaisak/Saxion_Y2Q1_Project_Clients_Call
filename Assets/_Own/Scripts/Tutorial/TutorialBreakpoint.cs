using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class TutorialBreakpoint : MonoBehaviour
{
    [SerializeField] Transform uiTransform;
    [SerializeField] float appearDuration = 0.4f;
    [SerializeField] float disappearDuration = 0.4f;
    [SerializeField] bool dontStartIdConditionMetEarly;

    private Tween appearTween;
    private bool wasTriggered;

    void Start()
    {
        Assert.IsNotNull(uiTransform);
        appearTween = uiTransform
            .DOScale(Vector3.zero, appearDuration)
            .From()
            .SetEase(Ease.InExpo)
            .SetUpdate(isIndependentUpdate: true)
            .Pause();
    }

    void Update()
    {
        if (!wasTriggered)
        {
            if (dontStartIdConditionMetEarly && ReleaseCondition())
            {
                uiTransform.DOKill();
                enabled = false;
                return;
            }
        }
        
        if (!ReleaseCondition()) return;
        
        Release();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;
        if (!other.CompareTag("Player")) return;

        wasTriggered = true;
        
        TimeHelper.timeScale = 0.01f;
        appearTween.Play();
    }

    protected abstract bool ReleaseCondition();

    private void Release()
    {
        uiTransform.DOKill();
        uiTransform
            .DOScale(Vector3.zero, disappearDuration)
            .From()
            .SetEase(Ease.InExpo)
            .SetUpdate(isIndependentUpdate: true)
            .Pause();
        
        TimeHelper.timeScale = 1f;
        enabled = false;
    }
}
