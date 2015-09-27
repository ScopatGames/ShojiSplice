using UnityEngine;
using System.Collections;

public class PlayerTriggerScript : MonoBehaviour {

	public GameObject targetGO;
	public string scriptNameToEnable;
	public bool targetEnabled=false;
	public bool destroyGO = false;
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == Tags.player) {
			if(!targetEnabled && targetGO != null){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
				targetEnabled = true;
				if(destroyGO){
					Destroy (gameObject);
				}
				
			}
		}
	}
}
