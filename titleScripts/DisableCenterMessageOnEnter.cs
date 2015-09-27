using UnityEngine;
using System.Collections;

public class DisableCenterMessageOnEnter : MonoBehaviour {

	[Header("Target GameObject for Post-Action Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool targetEnabled = false;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	private CenterMessageController centerMessageController;
	private CenterMessageOnEnter centerMessageOnEnter;

	void Awake(){
		centerMessageController = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponentInChildren<CenterMessageController> ();
		centerMessageOnEnter = GetComponent<CenterMessageOnEnter> ();
	}

	void OnEnable () {
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;

			if(!centerMessageController){
				centerMessageController = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponentInChildren<CenterMessageController> ();
			}

			centerMessageOnEnter.enabled = false;
			if (centerMessageOnEnter.GetComponent<BoxCollider> ()) {
				centerMessageOnEnter.GetComponent<BoxCollider> ().enabled = false;
			}
			else if(centerMessageOnEnter.GetComponent<SphereCollider> ()) {
				centerMessageOnEnter.GetComponent<SphereCollider> ().enabled = false;
			}
			centerMessageOnEnter.hitColliders.Clear ();
			centerMessageController.CloseCenterMessage ();

			//If post-action event exists, execute it...
			if(!targetEnabled && targetGO != null){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
				targetEnabled = true;
			}
		}
	}



}
