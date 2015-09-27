using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

	public int sceneToLoad;

	private SceneFadeInOut sceneFadeInOut;

	public void OnEnable () {
		if (GameObject.FindGameObjectWithTag(Tags.gameController)){
			sceneFadeInOut = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<SceneFadeInOut> ();
			sceneFadeInOut.LoadScene(sceneToLoad);
		}
		else{
			Application.LoadLevel (sceneToLoad);
		}
	}
	
}
