using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MyBehaviour
{
    public bool isPaused { get; private set; }
    
    private float initialTimeScale = 1f;
    
    void Update()
    {
        if (!Input.GetButtonDown("Pause")) return;

        if (isPaused) Unpause();
        else Pause();
    }

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