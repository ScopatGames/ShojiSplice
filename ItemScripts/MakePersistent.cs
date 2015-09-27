using UnityEngine;
using System.Collections;

public class MakePersistent : MonoBehaviour {

	void Start () {
		if (GameObject.FindGameObjectWithTag (Tags.localScene)) {
			if(transform.parent == null){
				GameObject localSceneGO = GameObject.FindGameObjectWithTag (Tags.localScene);
				transform.parent = localSceneGO.transform;

				//Get AudioSources in object...
				PlayRandomSoundOnStart audioExtension = GetComponent<PlayRandomSoundOnStart>();
				if(audioExtension != null){
					//Set play on awake to false so the sound does not play when respawned...
					audioExtension.doNotPlay = true;
				}
				PlayRandomSoundOnStart[] audioExtensionChildren = GetComponentsInChildren<PlayRandomSoundOnStart>();
				if(audioExtensionChildren.Length > 0){
					for(int i = 0; i < audioExtensionChildren.Length; i++){
						//Set play on awake to false so the sound does not play when respawned...
						audioExtensionChildren[i].doNotPlay = true;
					}
				}
				AudioSource audioSource = GetComponent<AudioSource>();
				if(audioSource != null){
					//Set play on awake to false so the sound does not play when respawned...
					audioSource.playOnAwake = false;
				}
				AudioSource[] audioSourceChildren = GetComponentsInChildren<AudioSource>();
				if(audioSourceChildren.Length > 0){
					for(int i = 0; i < audioSourceChildren.Length; i++){
						//Set play on awake to false so the sound does not play when respawned...
						audioSourceChildren[i].playOnAwake = false;
					}
				}
			}

		}
	}
}
