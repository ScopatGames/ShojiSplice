using UnityEngine;
using System.Collections;

public class DestroyEnemyOnCollision : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if ((other.tag == Tags.enemy || other.tag == Tags.npc || other.tag == Tags.neutral) && !other.isTrigger) {
			Destroy(other.gameObject);		
		}

	
	}
}
