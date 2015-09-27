using UnityEngine;
using System.Collections;

public class NpcActivateGate : MonoBehaviour {

	private GateController gateController;
	
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool alreadyEnabledOnce = false;
	
	//This script unlocks a door when called by an NPC...
	
	void OnEnable () {
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;
			gateController = GetComponent<GateController> ();
			gateController.ActivateGate ();
			
			//if there is a chained action...
			if(targetGO){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
			}
		}
	}
}
