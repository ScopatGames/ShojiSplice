using UnityEngine;
using System.Collections;

public class AudioSourceExtension : MonoBehaviour {

	public bool playOnStart;
	public bool doNotPlayOnStartWhenSceneCleared;

	private AudioSource audioSource;
	private SceneFadeInOut sceneFadeInOut;
	private bool played = false;

	// Use this for initialization
	void Start () {
		//Establish audioSource...
		audioSource = GetComponent<AudioSource> ();
		//Establish sceneFadeInOut...
		sceneFadeInOut = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<SceneFadeInOut> ();

		//Play audio clip on start...
		if (playOnStart && !doNotPlayOnStartWhenSceneCleared) {
			audioSource.Play ();
			played = true;
		}
	}

	void Update (){
		
		if(sceneFadeInOut.localPersistentSet && !played){
			GameObject localScene = GameObject.FindGameObjectWithTag (Tags.localScene);
			if (localScene != null) {
				if(localScene.GetComponent <LocalScenePersistentGameObjects>().isCleared && doNotPlayOnStartWhenSceneCleared){
					Destroy (gameObject);		
				}
				else if (playOnStart){
					audioSource.Play ();
					played = true;
				}
			}
		}
	}
}
