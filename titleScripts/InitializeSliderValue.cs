using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class InitializeSliderValue : MonoBehaviour {

	public AudioMixer masterMixer;
	public string stringSliderAudio;

	private float value;

	// Use this for initialization
	void Start () {
		masterMixer.GetFloat (stringSliderAudio, out value);
		GetComponent<Slider> ().value = value;
	}

}
