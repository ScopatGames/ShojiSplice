using UnityEngine;
using System.Collections;

public class ActivateBoxCollider : MonoBehaviour {

	public GameObject targetGO;
	public string scriptNameToEnable;

	[HideInInspector] public bool alreadyEnabledOnce = false;

	void OnEnable(){
		if(!alreadyEnabledOnce){
			//alreadyEnabledOnce = true;

			GetComponent<BoxCollider> ().enabled = true;

			if(targetGO != null){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = !(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled;
			}
		}
	}
}
