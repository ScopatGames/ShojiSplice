using UnityEngine;
using System.Collections;

public class UpdateNPCAI : MonoBehaviour {

	public bool isOnPatrol = false;
	public Transform targetPosition;

	public GameObject targetGO;
	public string scriptNameToEnable;



	// Use this for initialization
	void OnEnable () {

		if(isOnPatrol){
			GetComponent<EnemyAI>().isOnPatrol = true;
		}
		else if(targetPosition){

			GetComponent<EnemyAI>().isOnPatrol = false;
			GetComponent<EnemyAI>().navPosition = targetPosition.position;
		}

		//if there is a chained action...
		if(targetGO){
			(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
		}

	}
	

}
