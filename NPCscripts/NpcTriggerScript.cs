using UnityEngine;
using System.Collections;

public class NpcTriggerScript : MonoBehaviour {

	public GameObject targetGO;
	public string scriptNameToEnable;
	public bool targetEnabled=false;
	public bool destroyGO = false;

	[Header("Check this box if only the player should activate this trigger:")]
	public bool playerActivated = false;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == Tags.enemy || other.gameObject.tag == Tags.npc || other.gameObject.tag == Tags.neutral) {
			if(!targetEnabled && targetGO != null && !playerActivated){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
				targetEnabled = true;

				//destroying GameObject used for making NPCs disappear going through doors
				if(destroyGO){
					Destroy (gameObject);
				}

			}
		}
		else if(playerActivated && other.gameObject.tag == Tags.player){
			if(!targetEnabled && targetGO != null){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
				targetEnabled = true;
			}
		}
	}
}
