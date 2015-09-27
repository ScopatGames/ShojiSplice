using UnityEngine;
using System.Collections;

public class randomVolumePitch : MonoBehaviour {
	private AudioSource source;
	public float pitchLow=1.0f;
	public float pitchHigh=1.0f;
	public float volumeLow=1.0f;
	public float volumeHigh=1.0f;

	void Awake () {
		source = GetComponent<AudioSource>();
		source.pitch = Random.Range (pitchLow, pitchHigh);
		source.volume = Random.Range (volumeLow, volumeHigh);

	}

}
