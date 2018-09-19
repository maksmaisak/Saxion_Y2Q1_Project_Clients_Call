using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

public class Intro : MonoBehaviour
{
	[SerializeField] PlayableDirector introPlayableDirector;

	void Awake()
	{
		if (!introPlayableDirector) introPlayableDirector = GetComponent<PlayableDirector>();
		Assert.IsNotNull(introPlayableDirector);

		introPlayableDirector.played += director =>
		{
			Audio.instance.SetMusicVolume(0f);
			this.Delay((float)director.duration, () => Audio.instance.SetMusicVolume(1f));
		};
		
		introPlayableDirector.Play();
	}
}
