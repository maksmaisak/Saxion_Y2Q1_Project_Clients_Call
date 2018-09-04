using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    private int _score = 0;
    private int _scoreIncreaseAmount = 3;
    private float _updateScoreRate = 0.5f;
    private float _startTime = 0;
    private float _lastUpdate = 0;

    private void Start()
    {
        _startTime = Time.time;
    }

    public void Update()
    {
        if(_scoreText != null)
        {
            _updateScoreRate = 0.5f;

            if (Input.GetKey(KeyCode.V))
                _updateScoreRate /= 10.0f;

            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        if(Time.time - _lastUpdate >= _updateScoreRate)
        {
            _lastUpdate = Time.time;
            _score += _scoreIncreaseAmount;
            _scoreText.text = "Score: " + _score;
        }
    }
}
