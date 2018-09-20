using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MyBehaviour,
    IEventReceiver<OnPlayerDeath>,
    IEventReceiver<OnPlayerRespawned>,
    IEventReceiver<OnLevelBeganSwitching>,
    IEventReceiver<OnLevelFinishedSwitching>
{
    public bool isPaused { get; private set; }
    
    private float initialTimeScale = 1f;
    
    void Update()
    {
        if (!Input.GetButtonDown("Pause")) return;

        if (isPaused) Unpause();
        else Pause();
    }

    public void On(OnPlayerDeath         message) => enabled = false;
    public void On(OnPlayerRespawned     message) => enabled = true;
    
    public void On(OnLevelBeganSwitching  message) => enabled = false;
    public void On(OnLevelFinishedSwitching message) => enabled = true;
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Unpause();
    }

    public void Pause()
    {
        if (isPaused) return;
        
        initialTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        isPaused = true;
        
        new OnPause().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
    }

    public void Unpause()
    {
        if (!isPaused) return;
        
        Time.timeScale = initialTimeScale;
        
        isPaused = false;
        
        new OnUnpause().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
    }
}