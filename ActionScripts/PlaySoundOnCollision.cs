using UnityEngine;
using System.Collections;

public class PlaySoundOnCollision : MonoBehaviour {

	public AudioClip[] audioClips;
	public float pitchMin; 
	public float pitchMax;
	public float volumeMin; 
	public float volumeMax;
	public float minimumVelSqrd = 0.1f;
	public float minimumTimerLimit = 0.05f;

	private AudioSource audioSource;
	private float audioTimer=100f;
	private Rigidbody rb;
	private float maxVelocitySqrd;

	void Start(){
		audioSource = GetComponent<AudioSource> ();
		rb = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter () {
		if(audioTimer > minimumTimerLimit && rb.velocity.sqrMagnitude > minimumVelSqrd){
			int clipCount = audioClips.Length;
			audioSource.clip = audioClips [Random.Range (0, clipCount)];
			float pitchFrame = (rb.velocity.sqrMagnitude/maxVelocitySqrd)*(pitchMax-pitchMin)+pitchMin;
			if (pitchFrame > pitchMax){
				audioSource.pitch = pitchMax;
			}
			else{
				audioSource.pitch = pitchFrame;
			}
			float volumeFrame = (rb.velocity.sqrMagnitude/maxVelocitySqrd)*(volumeMax-volumeMin)+volumeMin;
			if (volumeFrame > volumeMax){
				audioSource.volume = volumeMax;
			}
			else{
				audioSource.volume = volumeFrame;
			}
			audioSource.Play ();

			//Restart audio timer...
			audioTimer = 0f;
		}
	}

	void Update(){
		//set max velocitysquared
		if(maxVelocitySqrd == 0f){
			maxVelocitySqrd = rb.velocity.sqrMagnitude;
		}

		//increment timer...
		audioTimer += Time.deltaTime;
		
	}

}
