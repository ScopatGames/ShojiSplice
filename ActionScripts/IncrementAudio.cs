using UnityEngine;
using System.Collections;

public class IncrementAudio : MonoBehaviour {

	public GameObject audioTargetGO;
	public float pitchIncrement;
	public float volumeIncrement;
	public float smoothTime;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	private AudioSource audioSource;
	private float targetPitch;
	private float tVol;
	private float tPit;
	private float targetVolume;
	private bool lerpValues;



	void OnEnable () {
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;
			audioSource = audioTargetGO.GetComponent<AudioSource> ();
			targetVolume = audioSource.volume + volumeIncrement;
			targetPitch = audioSource.pitch + pitchIncrement;
			lerpValues = true;
		}
	
	}

	void Update(){
		if (lerpValues && tVol<= 1.0f) {
			tVol += smoothTime*Time.deltaTime;
			float newVolume = Mathf.Lerp(audioSource.volume, targetVolume, tVol);
			audioSource.volume = newVolume;
		}
		if (lerpValues && tPit <= 1.0f) {
			tPit += smoothTime*Time.deltaTime;
			float newPitch = Mathf.Lerp (audioSource.pitch, targetPitch, tPit);
			audioSource.pitch = newPitch;
		}
	}

}
