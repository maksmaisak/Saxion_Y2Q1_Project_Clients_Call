using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;

public class ResolutionProfilerView : MyBehaviour, IEventReceiver<OnResolutionScreen>
{
    [SerializeField] List<TMP_Text> profileTexts = new List<TMP_Text>();
    [SerializeField] List<Image> profileImages = new List<Image>();

    // Use this for initialization
    private void Start()
    {
        Assert.IsTrue(profileTexts.Count == 4);
        Assert.IsTrue(profileImages.Count == 4);
    }

    public void On(OnResolutionScreen resolution)
    {
        var profiles = resolution.profiles;

        float totalAmount = 0;
        foreach (var profile in profiles)
            totalAmount += profile.Value;

        // Set fill Amounts for images and text for percentages
        for (int i = 0; i < 4; i++)
        {
            PlayerProfile playerProfile = (PlayerProfile)i;
            float profilePercent = totalAmount > 0 ? (float)profiles[playerProfile] / totalAmount : 0.0f;
            profileImages[i].fillAmount = Mathf.Clamp(profilePercent, 0.01f, 1f);
            profileTexts[i].text = (Math.Round(profilePercent * 100.0f, 1)) + "%";
        }
    }
}
