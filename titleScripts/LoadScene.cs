using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

	public int sceneToLoad;
	public bool resetDefaults;

	private SceneFadeInOut sceneFadeInOut;

	public void OnEnable () {
		if (GameObject.FindGameObjectWithTag(Tags.gameController)){
			sceneFadeInOut = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<SceneFadeInOut> ();
			if(resetDefaults){
				GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<PersistentData>().ResetDefaults();
			}
			sceneFadeInOut.LoadScene(sceneToLoad);
		}
		else{
			Application.LoadLevel (sceneToLoad);
		}
	}
	
}
