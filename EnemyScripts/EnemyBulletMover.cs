using UnityEngine;
using System.Collections;

public class EnemyBulletMover : MonoBehaviour {

	public float force;
	public LayerMask layerMask;
	public float minRandomAngle;
	public float maxRandomAngle;
	public float maxDamage;
	public float minDamage;
	public string damageType;
	public GameObject characterStrike;
	public Vector3 characterStrikeRotation = Vector3.zero;
	public GameObject wallStrike;
	public bool hasParticleSystem = false;
	public float bulletHitForce;
	public float inaccuracyScale = 0.0f;
	public float initialAngleOffset = 0.0f;

	private float randomAngleRange;
	private Vector3 previousPosition;
	private bool destroyTrigger = false;
	private Transform PS; //particle system transform
		
	
	// Use this for initialization
	void Start () {
		if (hasParticleSystem) {
			PS = transform.Find ("ParticleSystem");	
		}

		Vector3 trueDirection = transform.up; 
		trueDirection = Vector3.Normalize (trueDirection);
		previousPosition = transform.position;
		//Determine a random angle on the direction...
		randomAngleRange = minRandomAngle + inaccuracyScale*(maxRandomAngle-minRandomAngle);
		float deltaAngle = Random.Range (-randomAngleRange, randomAngleRange);

		// (Apparently this method works okay. See if this can be used in bulletMover and thrownMover scripts) Calculate global direction of bullet, theta...
		float theta = transform.eulerAngles.y;

		//Calculate new direction with random angle...
		Vector3 direction = new Vector3(Mathf.Sin ((theta + deltaAngle+initialAngleOffset)*Mathf.Deg2Rad), trueDirection.y, Mathf.Cos ((theta + deltaAngle+initialAngleOffset)*Mathf.Deg2Rad));

		GetComponent<Rigidbody>().AddForce(direction.normalized*force);
	}
	
	void FixedUpdate(){
		//Test to see if the bullet has passed through a wall without contact...
		RaycastHit hit;
		//Test forward...
		if (Physics.Raycast (transform.position, (transform.position - previousPosition), out hit, (transform.position - previousPosition).magnitude, layerMask)) {
			destroyTrigger = true;
			if (hit.transform.tag == Tags.player || hit.transform.tag == Tags.enemy || hit.transform.tag == Tags.npc || hit.transform.tag == Tags.neutral)
			{
				Instantiate (characterStrike, hit.point, transform.rotation);
			}
			else {
				Instantiate (wallStrike, hit.point, transform.rotation);
			}
			if(hit.rigidbody){
				hit.rigidbody.AddForce (GetComponent<Rigidbody>().velocity*bulletHitForce);
			}
		}
		//Test backwards...
		else if(Physics.Raycast (transform.position, (previousPosition - transform.position), out hit, (previousPosition - transform.position).magnitude, layerMask)){
			destroyTrigger = true;
			if (hit.transform.tag == Tags.player || hit.transform.tag == Tags.enemy || hit.transform.tag == Tags.npc || hit.transform.tag == Tags.neutral)
			{
				Instantiate (characterStrike, hit.point, transform.rotation);
			}
			else {
				Instantiate (wallStrike, hit.point, transform.rotation);
			}
			if(hit.rigidbody){
				hit.rigidbody.AddForce (GetComponent<Rigidbody>().velocity*bulletHitForce);
			}
		}
		//else, keep moving...
		else {
			previousPosition = transform.position;
		}
		//If it hit an environment object or player, destroy the bullet and/or apply damage...
		if (destroyTrigger == true) {

			if (hasParticleSystem){

				PS.GetComponent<ParticleSystem>().Stop ();
				PS.parent = null;
			}

			Destroy (gameObject);
			if(hit.collider.tag == Tags.player){
				float damage = Random.Range (minDamage,maxDamage);
				PlayerStats playerStats = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerStats>();
				playerStats.playerHealth -= damage;
				playerStats.damageForce = GetComponent<Rigidbody>().velocity*bulletHitForce; 
				playerStats.BloodEffects(damageType, hit.point, Quaternion.LookRotation(transform.position-previousPosition)*Quaternion.Euler(characterStrikeRotation));
			}
			else if(hit.transform.tag == Tags.enemy || hit.transform.tag == Tags.npc || hit.transform.tag == Tags.neutral){
				float damage = Random.Range (minDamage,maxDamage);
				EnemyStats enemyStats = hit.transform.GetComponent<EnemyStats>();
				enemyStats.enemyHealth -= damage;
				enemyStats.damageForce = GetComponent<Rigidbody>().velocity*bulletHitForce;
				enemyStats.BloodEffects(damageType, hit.point, Quaternion.LookRotation(transform.position-previousPosition)*Quaternion.Euler(characterStrikeRotation));
			}
		}
	}
}
