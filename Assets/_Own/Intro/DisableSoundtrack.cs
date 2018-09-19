using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

public class DisableSoundtrack : MonoBehaviour
{
	void OnEnable() => this.DoNextFrame(() => Audio.instance.SetMusicVolume(0f, immediate: true));
	void OnDisable() => Audio.instance.SetMusicVolume(1f);
	void OnDestroy() => Audio.instance.SetMusicVolume(1f);
}
