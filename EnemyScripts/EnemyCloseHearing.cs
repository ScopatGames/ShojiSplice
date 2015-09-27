using UnityEngine;
using System.Collections;

public class EnemyCloseHearing : MonoBehaviour {

	public LayerMask layerMask;
	private LookAtGameObject lookAtGameObject;

	private SphereCollider col;

	void Awake(){
		col = GetComponent<SphereCollider> ();
		lookAtGameObject = GetComponentInParent<LookAtGameObject> ();


	}
	
 
	
	void OnTriggerStay(Collider other){
		if(lookAtGameObject.target == null){
			//If the player or a weapon strike has entered the trigger sphere...
			if(other.gameObject.tag == Tags.player || other.gameObject.tag == Tags.strike || other.gameObject.tag == Tags.npc || other.gameObject.tag == Tags.enemy)
			{

				//Create a vector from the enemy to the player and store the angle between it and forward.
				Vector3 direction = other.transform.position - transform.position;


				RaycastHit hit;
				
				//... and if a raycast towards the player or strike hits something...
				if(Physics.Raycast (transform.position, direction.normalized, out hit, col.radius, layerMask))
				{
					// ... and if the raycast hits the player...
					if(transform.parent.tag == Tags.enemy && hit.collider.tag == Tags.player && GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerStats>().playerIsAlive && hit.collider.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
					{
						//... the player is in range.
						//update target to player...
						lookAtGameObject.target = other.transform;
						//lookAtGameObject.enabled = true;


					}
					else if (hit.collider.tag == Tags.strike && GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerStats>().playerIsAlive)
					{
						//GameObject theParent = transform.parent.gameObject;
						//theParent.GetComponentInChildren<EnemySight>().personalLastSighting = GameObject.FindGameObjectWithTag(Tags.player).transform.position;
						transform.parent.GetComponentInChildren<EnemySight>().personalLastSighting = GameObject.FindGameObjectWithTag(Tags.player).transform.position;

					}
					else if(transform.parent.tag == Tags.enemy && hit.collider.tag == Tags.npc && other.GetComponent<EnemyStats>().enemyIsAlive){
						//target the npc...
						lookAtGameObject.target = other.transform;
						//lookAtGameObject.enabled = true;

					}
					else if(transform.parent.tag == Tags.npc && hit.collider.tag == Tags.enemy && other.GetComponent<EnemyStats>().enemyIsAlive){
						//target the enemy...
						lookAtGameObject.target = other.transform;
						//lookAtGameObject.enabled = true;

					}
				}
			}
		}
	}
}