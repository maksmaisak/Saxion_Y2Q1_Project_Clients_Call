using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerRespawner : MyBehaviour, IEventReceiver<OnPlayerRespawn>
{
    [SerializeField] float respawnDuration = 5.0f;
    private PlayerController controller;

    public void Start()
    {
        controller = GetComponent<PlayerController>();
        Assert.IsNotNull(controller);
    }

    public void On(OnPlayerRespawn message)
    {
        ObjectRepresentation platform   = message.previousPlatform;
        Vector3 targetPos               = platform.gameObject.transform.position;
        targetPos.y                     = 0.75f;
        targetPos.z                     = platform.location.bounds.min;
        transform.position              = targetPos;

        /// TEMP: Make a new event for this instead of using directly
        controller.UpdatePlatformAndLaneAfterRespawn();

        StartCoroutine(EnableController());
    }

    IEnumerator EnableController()
    {
        yield return new WaitForSecondsRealtime(respawnDuration);
        controller.enabled = true;
        TimeHelper.timeScale = 1.0f;
    }
}

