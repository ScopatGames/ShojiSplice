using UnityEngine;
using System.Collections;

public class CenterMessageOpenOnEnable : MonoBehaviour {

	[Header("Message:")]
	public string message;

	[Header("Time Delay to Open Message")]
	public float timeDelay;
	public bool isFlashing;

	[Header("Target GameObject for Post-enabled Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool targetEnabled = false;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	private CenterMessageController centerMessageController;

	void OnEnable(){
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;

			if(!centerMessageController){
				centerMessageController = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponentInChildren<CenterMessageController> ();
			}
			centerMessageController.UpdateCenterMessage (message);
			if(isFlashing){
				centerMessageController.FlashCenterMessageAfterSeconds(timeDelay);
			}
			else{
				centerMessageController.OpenCenterMessageAfterSeconds(timeDelay);
			}

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
