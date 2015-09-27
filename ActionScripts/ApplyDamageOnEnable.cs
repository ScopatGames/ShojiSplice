using UnityEngine;
using System.Collections;

public class ApplyDamageOnEnable : MonoBehaviour {

	public GameObject damageTarget;
	public float damage;
	public Vector3 damageForce;
	public string damageType;
	[Header("Target GameObject for Post-Script Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;

	[HideInInspector] public bool alreadyEnabledOnce = false;
	private EnemyStats enemyStats;



	void OnEnable () {
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;

			enemyStats = damageTarget.GetComponent<EnemyStats> ();
			enemyStats.enemyHealth -= damage;
			enemyStats.damageForce = damageForce;
			enemyStats.BloodEffects (damageType, damageTarget.transform.position, damageTarget.transform.rotation);
		
			//if there is a chained action...
			if(targetGO){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
			}
		}
	
	}

}
