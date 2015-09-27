using UnityEngine;
using System.Collections;

public class SceneClearedOnEnable : MonoBehaviour {
	
	
	[Header("Target GameObject for Post-enabled Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool targetEnabled = false;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	private SceneFadeInOut sceneFadeInOut;
	private weaponIndex[] weapons;
	
	public void OnEnable(){
		//If scene clear hasn't already been called and if the player is still alive...
		if(!alreadyEnabledOnce && GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerStats>().playerIsAlive){
			alreadyEnabledOnce = true;
									
			//If post-action event exists, execute it...
			if(!targetEnabled && targetGO != null){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
				targetEnabled = true;
			}

			//Do this step last!
			sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<SceneFadeInOut>();
			sceneFadeInOut.SceneClear();

		}
	}
}
