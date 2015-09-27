using UnityEngine;
using System.Collections;

public class SetLookAtTarget : MonoBehaviour {

	public Transform target;

	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	void OnEnable(){
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;
			GetComponent<LookAtGameObject> ().target = target;

			if(targetGO){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
			}
		}
	}
}
