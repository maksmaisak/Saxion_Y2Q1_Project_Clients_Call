using UnityEngine;

public class OnPlayerDeath : BroadcastEvent<OnPlayerDeath>
{
    public Vector3 deathPosition = Vector3.zero;
    public string deathSceneName = "";

    public OnPlayerDeath(Vector3 deathPosition, string sceneName)
    {
        this.deathPosition = deathPosition;
        this.deathSceneName = sceneName;
    }
}