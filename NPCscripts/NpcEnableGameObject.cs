using UnityEngine;
using System.Collections;

public class NpcEnableGameObject : MonoBehaviour {

	public GameObject enableGO;
	[Header("Target GameObject for Post-Script Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	public bool targetEnabled=false;
	public bool destroyGO = false;
	[HideInInspector] public bool alreadyEnabledOnce = false;
	
	//This script enables a GameObject when called by an NPC...
	
	void OnEnable () {
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;
			enableGO.SetActive(true);
			
			//if there is a chained action...
			if(targetGO){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
			}
		}
	}
}
