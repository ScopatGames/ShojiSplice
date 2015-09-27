using UnityEngine;
using System.Collections;

public class AssignAudioClip : MonoBehaviour {

	public AudioClip audioClip;


	// Use this for initialization
	void Start () {
		gameObject.GetComponent<AudioSource> ().clip = audioClip;
		GetComponent<AudioSource>().Play();
	}

}
