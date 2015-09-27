using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class MeleeAttack : MonoBehaviour {
	public Transform meleeAttackOrigin;
	public Transform meleeAttackEndpoint;
	public LayerMask layerMask_enemy;
	public LayerMask layerMask_obstruction;
	public float maxDamage;
	public float minDamage;
	public string damageType;
	public float damageRadius;
	public float damageZoneAngleLeft;//The leftward angle from forward of the damage zone
	public float damageZoneAngleRight;//The rightward angle from forward of the damage zone
	public GameObject meleeCharacterStrike;
	public GameObject meleeWallStrike;
	public float meleeHitForce;
	public float cameraShakeMagnitude;

	private List<int> colliderParentIDs = new List<int>();
	private Collider[] inRangeColliders;
	private bool interactable = false;
	private SmoothCamera2D shakeCamera;


	void OnEnable () {
		//Clear the list of colliderParents
		colliderParentIDs.Clear ();

		//Define the shakeCamera GameObject if it is not already
		if(shakeCamera == null){
			shakeCamera = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<SmoothCamera2D> ();
		}

		//Define forward attack vector...
		Vector3 forwardAttackVector = meleeAttackEndpoint.position - meleeAttackOrigin.position;

		//Get colliders within range that are on the enemy layer...
		inRangeColliders = Physics.OverlapSphere (transform.position, damageRadius, layerMask_enemy);

		//Build list of colliders that are within damage zone...
		foreach(Collider inRangeCollider in inRangeColliders){
			//Check to see if the collider is a part of the owner...
			if(inRangeCollider.transform == transform.parent.transform.parent.transform){
				//Ignore the collider...
				continue;
			}

			//Define target location vector...
			Vector3 targetVector = inRangeCollider.bounds.ClosestPoint(meleeAttackEndpoint.position) - meleeAttackOrigin.position;

			targetVector.y = forwardAttackVector.y;  //ensures the targetVector is in the 2D plane 

			//Define cross product of attack vector and target vector...
			Vector3 crossVector = Vector3.Cross (forwardAttackVector, targetVector);

			//Check against the leftward or rightward damage zone limit angle, depending on sign of the y component of the cross...
			if(crossVector.y < 0f){
				//the target is to the left, so check against leftward angle limit...
				if(Vector3.Angle (forwardAttackVector, targetVector) < damageZoneAngleLeft){
					//check for blockages and apply damage if hit enemy...
					RaycastHit blockage; // Check for environment blockage (walls)
					if( Physics.Raycast (meleeAttackOrigin.position, targetVector, out blockage, damageRadius, layerMask_obstruction)){
						RaycastHit targetPoint; //Obtain target point (more accurate than center of collider)
						if(Physics.Raycast (meleeAttackOrigin.position, targetVector, out targetPoint, damageRadius, layerMask_enemy)){
							if(blockage.distance > targetPoint.distance){
								interactable = true;
							}
						}
					}
					else {
						interactable = true;
					}

				}
			}
			else{
				//the target is to the right, so check against the rightward angle limit...
				if(Vector3.Angle(forwardAttackVector, targetVector) < damageZoneAngleRight){
					//check for blockages and apply damage if hit enemy...
					RaycastHit blockage; // Check for environment blockage (walls)
					if( Physics.Raycast (meleeAttackOrigin.position, targetVector, out blockage, damageRadius, layerMask_obstruction)){
						RaycastHit targetPoint; //Obtain target point (more accurate than center of collider)
						if(Physics.Raycast (meleeAttackOrigin.position, targetVector, out targetPoint, damageRadius, layerMask_enemy)){
							if(blockage.distance > targetPoint.distance){
								interactable = true;
							}
						}
					}
					else {
						interactable = true;
					}
				}
			}

			//If the collider is deemed interactable, then check to see if its parent gameobject has already been accounted for...
			if(interactable){
				interactable = false;

				//Define parent ID...
				int colParentID = inRangeCollider.gameObject.GetInstanceID();

				//Test if the ID is already accounted for...
				if(!colliderParentIDs.Contains (colParentID)){	

					//If not, then add ID to list...
					colliderParentIDs.Add(colParentID);

					//...and apply damage...
					if(inRangeCollider.tag == Tags.enemy || inRangeCollider.tag == Tags.npc || inRangeCollider.tag == Tags.neutral){
						float damage = Random.Range (minDamage,maxDamage);
						EnemyStats enemyStats = inRangeCollider.gameObject.GetComponent<EnemyStats>();
						enemyStats.enemyHealth -= damage; 
						enemyStats.damageForce = forwardAttackVector*meleeHitForce*0.5f;
						enemyStats.BloodEffects(damageType, inRangeCollider.transform.position, transform.rotation);
						Instantiate (meleeCharacterStrike, inRangeCollider.transform.position, transform.rotation);
						shakeCamera.shakeCamera(cameraShakeMagnitude, meleeAttackEndpoint.position);
					}
					else if(inRangeCollider.tag == Tags.player){
						float damage = Random.Range (minDamage,maxDamage);
						PlayerStats playerStats = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerStats>();
						playerStats.playerHealth -= damage;
						playerStats.damageForce = forwardAttackVector*meleeHitForce*0.5f; 
						playerStats.BloodEffects(damageType, inRangeCollider.transform.position, transform.rotation);
						Instantiate (meleeCharacterStrike, inRangeCollider.transform.position, transform.rotation);
						shakeCamera.shakeCamera(cameraShakeMagnitude*2.0f, transform.position);
					}
				}
			}
		}

		//When the enemy testing is complete, test to see if there is a wall strike necessary...
		RaycastHit hit;
		if(Physics.Raycast (meleeAttackOrigin.position, forwardAttackVector, out hit, damageRadius, layerMask_obstruction)){
			Instantiate (meleeWallStrike, hit.point, transform.rotation);
			if(hit.rigidbody){
				hit.rigidbody.AddForce (forwardAttackVector*meleeHitForce);
			}

			shakeCamera.shakeCamera(cameraShakeMagnitude, meleeAttackEndpoint.position);
		}	

	}
}