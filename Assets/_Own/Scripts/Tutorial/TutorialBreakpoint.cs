using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class TutorialBreakpoint : MonoBehaviour
{
    [SerializeField] Transform uiTransform;
    [SerializeField] float appearDuration = 0.2f;
    [SerializeField] float disappearDuration = 0.2f;

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
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        wasTriggered = true;
        
        TimeHelper.timeScale = 0.01f;
        appearTween.Play();
    }

    private void Free()
    {
        
    }
}
