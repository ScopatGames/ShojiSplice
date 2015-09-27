using UnityEngine;
using System.Collections;

public class FiringRangeDoorTrigger : MonoBehaviour {

	public GameObject targetGO;
	public string scriptNameToEnable;
	public bool targetEnabled=false;
	public bool destroyGO = false;
	public DoorController doorController;
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == Tags.player) {
			if(!targetEnabled && targetGO != null){
				doorController = targetGO.GetComponent<DoorController>();
				doorController.LockDoor();

				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
				targetEnabled = true;
				if(destroyGO){
					Destroy (gameObject);
				}
				
			}
		}
	}
}
