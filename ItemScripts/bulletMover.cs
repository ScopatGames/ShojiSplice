using UnityEngine;
using System.Collections;

public class bulletMover : MonoBehaviour {
	public float force;
	public LayerMask layerMask;
	public float minRandomAngle;
	public float maxRandomAngle;
	public float maxDamage;
	public float minDamage;
	public string damageType;
	public int floorMask;
	public GameObject characterStrike;
	public Vector3 characterStrikeRotation = Vector3.zero;
	public GameObject wallStrike;
	public float bulletHitForce;
	public bool hasParticleSystem = false;
	public float inaccuracyScale = 0.0f;
	public float initialAngleOffset = 0.0f;

	private float angleLimit = 2.0f;
	private Vector3 previousPosition;
	private bool destroyTrigger = false;
	private float randomAngleRange;
	private Transform PS; //Particle System Transform
	private bool firstFrame = true;


	// Use this for initialization
	void Start () {
		if(hasParticleSystem){
			PS = transform.Find ("ParticleSystem");
		}
		//floorMask = LayerMask.GetMask ("Floor");
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		cursorPosition.y = GameObject.FindGameObjectWithTag (Tags.player).transform.position.y;

		// OBSOLETE: Figure out if the player is targeting an elevated location.  If so, adjust the y-component of the direction vector to account for it.
		/*Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;
		if (Physics.Raycast (camRay, out floorHit, 100f, floorMask)) {
			cursorPosition.y = floorHit.point.y + 0.5f;
		}
		else {
			cursorPosition.y = 0.5f;
		}END OBSOLETE*/



		Vector3 direction = new Vector3 (cursorPosition.x - transform.position.x, cursorPosition.y - transform.position.y, cursorPosition.z-transform.position.z);
		//"direction" is the vector from the bullet spawn point to the cursor
		direction = Vector3.Normalize (direction);

		previousPosition = GameObject.FindGameObjectWithTag (Tags.player).GetComponentInChildren<ShootBullet> ().transform.position;

		// theta is the angle between "direction" and transform.up of the bullet.  If theta is too large, the player will awkwardly shoot sideways, so...
		//Use vector projection onto a plane (directionProjection) to figure out theta...
		Vector3 directionProjection = new Vector3 ();
		directionProjection = direction - Vector3.Dot (direction, transform.forward)*transform.forward;
		float theta = Vector3.Angle (directionProjection, transform.up);

		// If the "direction" is too great an angle...
		if(theta > angleLimit){
			//Set the angleLimit as the limiting angle on the "direction"
			// phi is the angle between 0 and direction...
			float phi = Mathf.Atan2 (transform.up.z,transform.up.x);
			direction = new Vector3(Mathf.Cos (phi+angleLimit*Mathf.Deg2Rad), direction.y, Mathf.Sin (phi+angleLimit*Mathf.Deg2Rad));
		}		
		//Determine a random angle on the direction...
		randomAngleRange = minRandomAngle + inaccuracyScale*(maxRandomAngle-minRandomAngle);
		float deltaAngle = Random.Range (-randomAngleRange, randomAngleRange);
		//Calculate new direction with random angle...
		//theta reassigned to angle between direction and zero...
		theta = Mathf.Atan2 (direction.z,direction.x);

		direction = new Vector3(Mathf.Cos (theta+(deltaAngle+initialAngleOffset)*Mathf.Deg2Rad), direction.y, Mathf.Sin (theta+(deltaAngle+initialAngleOffset)*Mathf.Deg2Rad));
		GetComponent<Rigidbody>().AddForce(direction*force);
	}

	void FixedUpdate(){
		//Test to see if the bullet has passed through a wall without contact...
		RaycastHit hit;
		//Test forward...
		if (Physics.Raycast (transform.position, (transform.position - previousPosition), out hit, (transform.position - previousPosition).magnitude, layerMask)) {

			if (hit.transform.tag == Tags.enemy || hit.transform.tag == Tags.npc || hit.transform.tag == Tags.neutral || hit.transform.tag == Tags.player)
			{
				if(hit.transform.tag == Tags.player && firstFrame){
					previousPosition = transform.position;
				}
				else{
					destroyTrigger = true;
					Instantiate (characterStrike, hit.point, transform.rotation);
				}
			}
			else {
				destroyTrigger = true;
				Instantiate (wallStrike, hit.point, transform.rotation);
			}
			if(hit.rigidbody){
				hit.rigidbody.AddForce (GetComponent<Rigidbody>().velocity*bulletHitForce);
			}
		}
		//Test backwards...
		else if(Physics.Raycast (transform.position, (previousPosition - transform.position), out hit, (previousPosition - transform.position).magnitude, layerMask)){

			if (hit.transform.tag == Tags.enemy || hit.transform.tag == Tags.npc || hit.transform.tag == Tags.neutral || hit.transform.tag == Tags.player)
			{
				if(hit.transform.tag == Tags.player && firstFrame){
					previousPosition = transform.position;
				}
				else{
					destroyTrigger = true;
					Instantiate (characterStrike, hit.point, transform.rotation);
				}
			}
			else {
				destroyTrigger = true;
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

		//If it hit an environment object or enemy, destroy the bullet and/or apply damage...
		if (destroyTrigger == true) {

			if (hasParticleSystem){

				PS.GetComponent<ParticleSystem>().Stop ();
				PS.parent = null;
			}
			Destroy (gameObject);

			if(hit.collider.tag == Tags.enemy || hit.collider.tag == Tags.npc || hit.collider.tag == Tags.neutral){
				float damage = Random.Range (minDamage,maxDamage);
				EnemyStats enemyStats = hit.collider.gameObject.GetComponent<EnemyStats>();
				enemyStats.BloodEffects(damageType, hit.point, Quaternion.LookRotation(transform.position-previousPosition)*Quaternion.Euler(characterStrikeRotation)); 
				enemyStats.enemyHealth -= damage; 
				enemyStats.damageForce = gameObject.GetComponent<Rigidbody>().velocity*bulletHitForce;
			}
			else if(hit.collider.tag == Tags.player){
				float damage = Random.Range (minDamage,maxDamage);
				PlayerStats playerStats = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerStats>();
				playerStats.BloodEffects(damageType, hit.point, Quaternion.LookRotation(transform.position-previousPosition)*Quaternion.Euler(characterStrikeRotation)); 
				playerStats.playerHealth -= damage; 
				playerStats.damageForce = gameObject.GetComponent<Rigidbody>().velocity*bulletHitForce;
			}
		}
		if(firstFrame){
			firstFrame = false;
		}
	}
}
