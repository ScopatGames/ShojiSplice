using UnityEngine;
using System.Collections;

public class ActivateSphereCollider : MonoBehaviour {

	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool alreadyEnabledOnce = false;
	
	void OnEnable(){
		if(!alreadyEnabledOnce){
			//alreadyEnabledOnce = true;
			GetComponent<SphereCollider> ().enabled = true;
		
			if(targetGO != null){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = !(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled;
			}
		}
	}
}
