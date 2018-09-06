using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] float _slowTimeScale = 0.6f;
    [SerializeField] private TMP_Text _timerText;

    private PlayerController _playerController;
    private bool _isGodMode = false;
    private bool _canShowTimer = false;
    private float _timerDuration;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(_canShowTimer)
        {
            _timerDuration -= Time.deltaTime;
            _timerText.text = "Timer: " + System.Math.Round(_timerDuration, 2);

            if(_timerDuration <= 0)
            {
                _canShowTimer = false;
                _timerDuration = 0.0f;
                _timerText.enabled = false;
            }
        }
    }

    public void CollectPowerUp(PowerUpInfo powerUp)
    {
        if (_isGodMode)
            return;

        switch(powerUp.type)
        {
            case PowerUpType.Slow:
                Time.timeScale = _slowTimeScale;
                Time.fixedDeltaTime = 0.02f * _slowTimeScale;
                Invoke("ResetTimeScale", powerUp.duration);
                break;

            case PowerUpType.Fast:
                _isGodMode = true;
                _playerController.SetSpeed(powerUp.power);
                _playerController.SetFall(false);
                Invoke("ResetGodMode", powerUp.duration);
                break;

            default: break;
        }

        StartTimer(powerUp.duration);
    }

    public bool IsGodMode()
    {
        return _isGodMode;
    }

    private void ResetTimeScale()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
    }

    private void ResetGodMode()
    {
        _isGodMode = false;
        _playerController.SetSpeed(_playerController.GetOldSpeed());
        _playerController.SetFall(true);
    }

    private void StartTimer(float duration)
    {
        _timerDuration = duration;
        _canShowTimer = true;
        _timerText.enabled = true;
    }
}
