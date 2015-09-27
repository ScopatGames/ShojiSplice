using UnityEngine;
using System.Collections;

public class SceneChangeOnPlayerDeath : MonoBehaviour {

	private SceneFadeInOut sceneFadeInOut;
	private PlayerStats playerStats;
	private CenterMessageController centerMessageController;
	public float waitTime;
	//[Header("If Next Scene = 0, then automatically loads next scene")]
	[Header("Next relative scene, e.g. 1 for thisScene+1")]
	public int nextScene=0;
	public int spawnLocation = 0;
	private PersistentData persistentData;


	void Start () {
		playerStats = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<PlayerStats>();
		sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<SceneFadeInOut>();
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
		centerMessageController = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponentInChildren<CenterMessageController> ();
	}
	

	void Update () {
		if(!playerStats.playerIsAlive){
			if(!sceneFadeInOut.disableRespawn){
				sceneFadeInOut.disableRespawn = true;
			}
			if(centerMessageController.messageBox.text != ""){
				centerMessageController.UpdateCenterMessage("");
			}
			Invoke("ChangeScene", waitTime);
		}
	}

	void ChangeScene(){

		//if(nextScene==0){
		persistentData.nextSpawnPoint = spawnLocation;
		sceneFadeInOut.LoadScene (sceneFadeInOut.thisScene+nextScene);
		//}
		//else{
		//	persistentData.nextSpawnPoint = spawnLocation;
		//	sceneFadeInOut.LoadScene (nextScene);
		//}
	}

}
