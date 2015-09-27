using UnityEngine;
using System.Collections;

public class PlayerEnableScript : MonoBehaviour {

	[Header("Script to enable:")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	public bool DestroyGO;


	public void EnableScript () {
		//if there is a chained action...
		if(targetGO){
			(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
		}
		if (DestroyGO) {
			Destroy (gameObject);
		}
	}
	

}
