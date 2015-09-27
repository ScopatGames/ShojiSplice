using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CenterMessageOnEnter : MonoBehaviour {

	[Header("Message string:")]
	public string message;
	[Header("Should the message flash?")]
	public bool flashMessage;
	public List<Collider> hitColliders = new List<Collider>();

	[Header("Target GameObject for Post-enabled Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool targetEnabled = false;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	private CenterMessageController centerMessageController;
	private bool noHitsLastFrame = true;


	void OnEnable(){
		hitColliders.Clear ();
		noHitsLastFrame = true;
		centerMessageController = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponentInChildren<CenterMessageController> ();

		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;

			//If post-action event exists, execute it...
			if(!targetEnabled && targetGO != null){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
				targetEnabled = true;
			}
		}
	}

	// Use this for initialization
	/*void Awake () {
		centerMessageController = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponentInChildren<CenterMessageController> ();
		hitColliders.Clear ();
	}*/

	void FixedUpdate(){
		if(!centerMessageController){
			centerMessageController = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponentInChildren<CenterMessageController> ();
		}

		//Check to see if touching colliders have been destroyed.  If so, remove from list...
		if(hitColliders.Count > 0 ){
			foreach (Collider col in hitColliders){
				if(col == null){
					hitColliders.Remove (col);
					return;
				}
			}
		}


		if(hitColliders.Count == 0 && !noHitsLastFrame){
			centerMessageController.CloseCenterMessage();
			noHitsLastFrame = true;
		}
		else if(hitColliders.Count != 0 && noHitsLastFrame){
			centerMessageController.UpdateCenterMessage(message);
			if(flashMessage){
				centerMessageController.FlashCenterMessage();
				noHitsLastFrame = false;
			}
			else{
				centerMessageController.OpenCenterMessage();
				noHitsLastFrame = false;
			}
		}
		
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == Tags.player){ 
			hitColliders.Add (other);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == Tags.player){ 
			hitColliders.Remove (other);
		}
	}
}
