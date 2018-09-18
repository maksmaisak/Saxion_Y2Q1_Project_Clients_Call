using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerRespawner : MyBehaviour, IEventReceiver<OnPlayerWillRespawn>
{
    [SerializeField] float respawnDuration = 5.0f;
    private PlayerController controller;

    public void Start()
    {
        controller = GetComponent<PlayerController>();
        Assert.IsNotNull(controller);
    }

    public void On(OnPlayerWillRespawn message)
    {
        ObjectRepresentation platform   = message.previousPlatform;
        Vector3 targetPos               = platform.gameObject.transform.position;
        targetPos.y                     = 0.75f;
        targetPos.z                     = platform.location.bounds.min;
        transform.position              = targetPos;

        /// TEMP: Make a new event for this instead of using directly
        controller.ResetControllerAfterRespawn();

        StartCoroutine(EnableController());
    }

    IEnumerator EnableController()
    {
        yield return new WaitForSecondsRealtime(respawnDuration);
        
        TimeHelper.timeScale = 1.0f;
        new OnPlayerRespawned().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
    }
}

