using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

	private SceneFadeInOut sceneFadeInOut;
	private PersistentData persistentData;

	public void LoadScene(int level)
	{
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
		persistentData.Save ();
		sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<SceneFadeInOut>();
		sceneFadeInOut.LoadScene (level);


	}

	public void QuitGame(){
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
		persistentData.Save ();
		Application.Quit();
	}


}
