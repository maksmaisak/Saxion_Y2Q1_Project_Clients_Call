using System.Collections;
using UnityEngine;

public class InsertCoinController : MyBehaviour, IEventReceiver<OnInsertCoinScreen>
{
    private bool canListenToControls = false;
    private float initialTimeScale = 1.0f;

    public void Update()
    {
        if (!canListenToControls) return;

        if(Input.GetButtonDown("Jump"))
        {
            canListenToControls = false;
            StopAllCoroutines();
        }
    }

    public void On(OnInsertCoinScreen message)
    {
        TimeHelper.timeScale = 0f;
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