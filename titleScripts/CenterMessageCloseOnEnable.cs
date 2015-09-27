using UnityEngine;
using System.Collections;

public class CenterMessageCloseOnEnable : MonoBehaviour {
	[Header("Time Delay to Close Message")]
	public float timeDelay;
	private CenterMessageController centerMessageController;

	[Header("Target GameObject for Post-enabled Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool targetEnabled = false;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	void OnEnable(){
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;

			if(!centerMessageController){
				centerMessageController = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponentInChildren<CenterMessageController> ();
			}
			centerMessageController.CloseCenterMessageAfterSeconds (timeDelay);

			//If post-action event exists, execute it...
			if(!targetEnabled && targetGO != null){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
				targetEnabled = true;
			}
		}
	}

	// Use this for initialization
	void Awake () {
		centerMessageController = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponentInChildren<CenterMessageController> ();
	}
	

}
