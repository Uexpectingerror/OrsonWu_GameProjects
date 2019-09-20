using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicSnapshotSwitch : MonoBehaviour {

	public AudioMixerSnapshot mySnapshot;
	public float fadeTime = 3.0f;
	public float delayTime = 0.0f;

	
	void OnTriggerEnter () {
		mySnapshot.TransitionTo (fadeTime);


	}

}
