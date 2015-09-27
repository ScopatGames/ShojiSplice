using UnityEngine;
using System.Collections;

public class UpdateNpcTag : MonoBehaviour {

	public string newTag;

	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	void OnEnable(){
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;
			transform.parent.tag = newTag;
		
			if(targetGO){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
			}
		}
	}
}
