using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorTrigger : MonoBehaviour {

	public List<Collider> hitColliders = new List<Collider>();


	private DoorController doorController;




	void Awake(){
		doorController = GetComponentInParent<DoorController> ();

	}

	void FixedUpdate(){
		if(hitColliders.Count > 0 ){
			foreach (Collider col in hitColliders){
				//Debug.Log (col.transform.name);
				if(col == null){
					hitColliders.Remove (col);
					return;
				}
			}
		}

		if(hitColliders.Count == 0){
			doorController.CloseDoor();
		}

	}

	void OnTriggerEnter(Collider other){
		if ((other.gameObject.tag == Tags.enemy || other.gameObject.tag == Tags.npc || other.gameObject.tag == Tags.neutral) && !doorController.isLocked){ 
			if(other.gameObject.GetComponent <EnemyAI>()){	
				if(other.gameObject.GetComponent<EnemyAI>().autoOpensDoors){ 
					hitColliders.Add (other);
					doorController.OpenDoor();
				}
			}
		}
		if((other.gameObject.tag == Tags.enemy  || other.gameObject.tag == Tags.npc || other.gameObject.tag == Tags.neutral) && doorController.opensForPatrol && !doorController.isLocked){
			if(other.gameObject.GetComponent <EnemyAI>()){
				if(other.gameObject.GetComponent<EnemyAI>().opensDoorsOnPatrol && !other.gameObject.GetComponent<EnemyAI>().autoOpensDoors){ 
					hitColliders.Add (other);
					doorController.OpenDoor();
				}
			}
		}
		if(other.gameObject.tag == Tags.player && !doorController.isLocked){
			if(!hitColliders.Contains (other)){
				hitColliders.Add (other);
			}
		}
	}



	void OnTriggerExit(Collider other){
		if ((other.gameObject.tag == Tags.enemy || other.gameObject.tag == Tags.npc || other.gameObject.tag == Tags.neutral || other.gameObject.tag ==  Tags.player) && !doorController.isLocked) {
			hitColliders.Remove (other);
			if(hitColliders.Count == 0){
				doorController.CloseDoor();
			}
		}
	}


}
