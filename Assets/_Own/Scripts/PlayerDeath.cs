using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private Vector3 _spawnPoint = Vector3.zero;
    [SerializeField] private float _minimumPositionY = -15f;

    private Rigidbody _rb = null;
    private Player _player = null;
    private PlayerController _playerController = null;

    public delegate void GameOverEvent();
    public event GameOverEvent HandleGameOver;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
        _playerController = GetComponent<PlayerController>();
        StartCoroutine(CheckYBoundary());
    }

    void GameOver()
    {
        // Die
        // Play Sound
        // Play Animation
        // Display Resolution Screen
        //_rb.constraints = _rb.constraints |= RigidbodyConstraints.FreezeAll;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        //ResetPlayerDefaults();

        //HandleGameOver?.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_player.IsGodMode() && other.CompareTag("Obstacle"))
            GameOver();
    }

    private void ResetPlayerDefaults()
    {
        _playerController.enabled = true;
        _rb.useGravity = false;
        _rb.isKinematic = true;
        transform.position = _spawnPoint;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    IEnumerator CheckYBoundary()
    {
        while(this.enabled)
        {
            if(transform.position.y < _minimumPositionY)
                GameOver();

            yield return new WaitForSeconds(0.3f);
        }
    }
}
