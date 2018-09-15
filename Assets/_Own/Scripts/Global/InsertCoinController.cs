using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class InsertCoinController : MyBehaviour, IEventReceiver<OnInsertCoinScreen>
{
    [SerializeField] PlayerController controller;

    private bool canListenToControls = false;
    private float initialTimeScale = 1.0f;

    public void Start()
    {
        Assert.IsNotNull(controller);
    }

    public void Update()
    {
        if (!canListenToControls) return;

        if(Input.GetButtonDown("Respawn"))
        {
            canListenToControls = false;
            StopAllCoroutines();

            new OnPlayerRespawn(controller.previousPlatform)
                .SetDeliveryType(MessageDeliveryType.Immediate)
                .PostEvent();
        }
    }

    public void On(OnInsertCoinScreen message)
    {
        TimeHelper.timeScale = 0.0f;
        canListenToControls = true;
        StartCoroutine(OnTimeExpire(10.0f));
    }

    private IEnumerator OnTimeExpire(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        TimeHelper.timeScale = initialTimeScale;
        new OnGameOver().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
    }
}