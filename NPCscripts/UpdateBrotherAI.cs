using UnityEngine;
using System.Collections;

public class UpdateBrotherAI : MonoBehaviour {

	public float newPatrolStoppingDistance;
	public bool isOnPatrol;
	public bool ableToAttack;
	[Header("Target GameObject for Post-Script Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool alreadyEnabledOnce = false;
	
	void OnEnable(){
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;
			transform.parent.GetComponent<LookAtGameObject> ().target = null;
			transform.parent.GetComponentInChildren<EnemySight>().patrolStoppingDistance = newPatrolStoppingDistance;
			transform.parent.GetComponent<EnemyAI> ().isOnPatrol = isOnPatrol;
			transform.parent.GetComponentInChildren<EnemySight> ().ableToAttack = ableToAttack;
			
			if(targetGO){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
			}
		}
	}
}
