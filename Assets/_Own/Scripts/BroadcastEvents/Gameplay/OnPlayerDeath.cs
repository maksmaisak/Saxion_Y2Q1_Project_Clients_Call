using UnityEngine;

public class OnPlayerDeath : BroadcastEvent<OnPlayerDeath>
{
    public Vector3 position = Vector3.zero;

    public OnPlayerDeath(Vector3 deathPosition)
    {
        position = deathPosition;
    }
}