using TMPro;
using UnityEngine;

public class ResolutionScoreView : MyBehaviour, IEventReceiver<OnResolutionScreen>
{
    TMP_Text scoreText;

    // Use this for initialization
    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    public void On(OnResolutionScreen resolution)
    {
        scoreText.text = $"Score: {resolution.score}";
    }
}
