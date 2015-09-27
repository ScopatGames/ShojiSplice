using UnityEngine;
using System.Collections;

public class EnableInteraction : MonoBehaviour {

	[Header("Target GameObject for Post-Action Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool targetEnabled = false;
	[HideInInspector] public bool alreadyEnabledOnce = false;



	void OnEnable () {
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;

			InteractionController interactionController = GetComponent<InteractionController> ();
			interactionController.InitiateTalking ();

			//If post-action event exists, execute it...
			if(!targetEnabled && targetGO != null){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
				targetEnabled = true;
			}
		}	
	}

}
