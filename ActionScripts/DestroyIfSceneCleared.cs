using UnityEngine;
using System.Collections;

public class DestroyIfSceneCleared : MonoBehaviour {

	private SceneFadeInOut sceneFadeInOut;

	void Start () {
		sceneFadeInOut = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<SceneFadeInOut> ();
	}

	void Update (){

		if(sceneFadeInOut.localPersistentSet){
			GameObject localScene = GameObject.FindGameObjectWithTag (Tags.localScene);
			if (localScene != null) {
				if(localScene.GetComponent <LocalScenePersistentGameObjects>().isCleared){
					Destroy (gameObject);		
				}
			}
		}
	}

}
