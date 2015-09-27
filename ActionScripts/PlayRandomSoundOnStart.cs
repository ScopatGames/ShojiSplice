using UnityEngine;
using System.Collections;

public class PlayRandomSoundOnStart : MonoBehaviour {

	[Header("Bypass play on start...")]
	public bool doNotPlay;
	public AudioClip[] audioClips;
	public float pitchMin; 
	public float pitchMax;
	public float volumeMin; 
	public float volumeMax;

	private AudioSource audioSource;

	void Awake(){
		audioSource = GetComponent<AudioSource> ();
		audioSource.pitch = Random.Range (pitchMin, pitchMax);
		audioSource.volume = Random.Range (volumeMin, volumeMax);
		int clipCount = audioClips.Length;
		audioSource.clip = audioClips [Random.Range (0, clipCount)];

		if(!doNotPlay){
			PlayClip ();
		}
	}

	void PlayClip(){
		audioSource.Play ();
	}
}

