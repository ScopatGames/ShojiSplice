using UnityEngine;
using System.Collections;

public class ChangeMusicOnEnable : MonoBehaviour {

	public AudioClip newAudioClip;
	public float newMusicVolume;

	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	private InitializeBackgroundMusic initBkgdMusic;


	void Awake () {
		Transform t = GameObject.FindGameObjectWithTag (Tags.gameController).transform;
		initBkgdMusic = t.parent.GetComponentInChildren<InitializeBackgroundMusic> ();


	}
	
	void OnEnable () {
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;
			initBkgdMusic.ChangeMusicPublic (newAudioClip, newMusicVolume);

			if(targetGO){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
			}
		}
	}
}
