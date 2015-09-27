using UnityEngine;
using System.Collections;

public class DestroyByExplosion : MonoBehaviour {
	private Transform originStart;
	private Vector3 direction;
	private float colRadius;
	public float maxDamage = 200f;
	public float minDamage = 75f;
	//public GameObject blood;
	public LayerMask obstacleLayerMask;
	public LayerMask colliderLayerMask;
	public float explosionForce;

	private PlayerStats playerStats;
	private EnemyStats enemyStats;
	private Collider[] hitColliders;

	public void ExplodeAndDamage(){
		colRadius = GetComponent<SphereCollider> ().radius;
	
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, colRadius, colliderLayerMask);

		foreach(Collider other in hitColliders){
			if ((other.gameObject.transform.parent.tag == Tags.enemy || other.gameObject.transform.parent.tag == Tags.npc || other.gameObject.transform.parent.tag == Tags.neutral) && !other.isTrigger) {
				enemyStats = other.gameObject.transform.GetComponentInParent<EnemyStats>();

				RaycastHit hit;
				originStart = gameObject.transform;
				//Debug.Log (originStart);
				direction = other.transform.position - transform.position;

				//Debug.Log (direction);
				if(Physics.Raycast (originStart.position, direction.normalized, out hit, direction.magnitude, obstacleLayerMask)){
					//Debug.Log ("Hit environment!");
					if(hit.distance > direction.magnitude){
						//Debug.Log ("Damage Enemy");
						//Calculate Damage:
						float damage = (minDamage-maxDamage)/colRadius*direction.magnitude + maxDamage;
						enemyStats.enemyHealth -= damage;
						enemyStats.explosionForce = explosionForce*(damage-minDamage)/(maxDamage-minDamage);
						enemyStats.explosionLocation = transform.position;
						//enemyStats.explosionRadius = colRadius;


						Quaternion newRotation = Quaternion.LookRotation(new Vector3(other.transform.position.x - transform.position.x, 0.0f, other.transform.position.z-transform.position.z), Vector3.up);
						newRotation *= Quaternion.Euler(90,0,0);
						enemyStats.BloodEffects("highVelocityBlunt", other.transform.position, newRotation);
						//Instantiate (blood, other.transform.position, newRotation);
						//Destroy (other.gameObject);
					}

				}
				else {
					//Debug.Log ("Damage Enemy");
					float damage = (minDamage-maxDamage)/colRadius*direction.magnitude + maxDamage;
					enemyStats.enemyHealth -= damage;
					enemyStats.explosionForce = explosionForce*(damage-minDamage)/(maxDamage-minDamage);
					enemyStats.explosionLocation = transform.position;
					//enemyStats.explosionRadius = colRadius;
					Quaternion newRotation = Quaternion.LookRotation(new Vector3(other.transform.position.x - transform.position.x, 0.0f, other.transform.position.z-transform.position.z), Vector3.up);
					newRotation *= Quaternion.Euler(90,0,0);
					enemyStats.BloodEffects("highVelocityBlunt", other.transform.position, newRotation);
					//Instantiate (blood, other.transform.position, newRotation);
					//Destroy (other.gameObject);
				}
			}

			if (other.gameObject.transform.parent.tag == Tags.player && !other.isTrigger) {
				playerStats = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerStats>();
				//Debug.Log (other.gameObject.name);
				RaycastHit hit;
				originStart = gameObject.transform;
				//Debug.Log (originStart);
				direction = other.transform.position - transform.position;
				//Debug.Log (direction);
				if(Physics.Raycast (originStart.position, direction.normalized, out hit, direction.magnitude, obstacleLayerMask)){
					//Debug.Log ("Hit environment!");
					if(hit.distance > direction.magnitude){
						//Debug.Log ("But it was behind the explosion!");
						float damage = (minDamage-maxDamage)/colRadius*direction.magnitude + maxDamage;
						//Debug.Log ("Damage Player");
						playerStats.playerHealth -= damage;
						playerStats.explosionForce = explosionForce*(damage-minDamage)/(maxDamage-minDamage);
						playerStats.explosionLocation = transform.position;
						//playerStats.explosionRadius = colRadius;
						Quaternion newRotation = Quaternion.LookRotation(new Vector3(other.transform.position.x - transform.position.x, 0.0f, other.transform.position.z-transform.position.z), Vector3.up);
						newRotation *= Quaternion.Euler(90,0,0);
						playerStats.BloodEffects("highVelocityBlunt", other.transform.position, newRotation);
						//Instantiate (blood, other.transform.position, newRotation);
						//Destroy (other.gameObject);
					}

				}
				else {

					float damage = (minDamage-maxDamage)/colRadius*direction.magnitude + maxDamage;

					playerStats.playerHealth -= damage;
					playerStats.explosionForce = explosionForce*(damage-minDamage)/(maxDamage-minDamage);
					playerStats.explosionLocation = transform.position;
					//playerStats.explosionRadius = colRadius;
					Quaternion newRotation = Quaternion.LookRotation(new Vector3(other.transform.position.x - transform.position.x, 0.0f, other.transform.position.z-transform.position.z), Vector3.up);
					newRotation *= Quaternion.Euler(90,0,0);
					playerStats.BloodEffects("highVelocityBlunt", other.transform.position, newRotation);
					//Instantiate (blood, other.transform.position, newRotation);

				}
			}
			// Explosion affecting other Physics objects...
			if (other.gameObject.transform.parent.tag == Tags.physicsInteractable && !other.isTrigger) {

				RaycastHit hit;
				originStart = gameObject.transform;
				//Determine vector to explosion interactable collider parent
				direction = other.transform.parent.position - transform.position;

				//Test to see if an obstacle is within range...
				if(Physics.Raycast (originStart.position, direction.normalized, out hit, direction.magnitude, obstacleLayerMask)){

					//There is an obstacle.  Test to see if the obstacle is a PhysicsInteractable obstacle...
					if (hit.transform.tag != Tags.physicsInteractable){
						//If it is not PhysicsInteractable, it is a solid barrier.  Test to see if the obstacle is behind the object...
						if(hit.distance > direction.magnitude){
							//The obstacle is behind the object, so it is unprotected and an explosion force is added...
							other.gameObject.transform.parent.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, colRadius); 
						}
					}
					else {
						//The obstacle is PhysicsInteractable, so an explosion force is added...
						other.gameObject.transform.parent.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, colRadius); 
					}
				}
				else {
					//There is no obstacle in the way, and an explosion force is added...
					other.gameObject.transform.parent.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, colRadius); 
				}
			}
		}
	}
}
