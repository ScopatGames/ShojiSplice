using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySight : MonoBehaviour {

	public bool ableToAttack = true; 
	public bool insideScreenBuffer = false; //Turn this on for an enemy once inside screen buffer
	public float fieldOfViewAngle = 240f;
	public float sightlessScreenBufferPercentageMax = 15f;
	public float sightlessScreenBufferPercentageMin = 8f;
	public bool targetInSight;
	public bool targetInRange;
	public float equippedWeaponAttackRange=0f;
	public Vector3 personalLastSighting;
	public LayerMask sightMask;
	public LayerMask colliderMask;
	public float patrolStoppingDistance=2f;
	public Vector3 direction;
	public Vector3 direction2;
	public bool chasesNonLineOfSight = true;
	public Vector3 targetResetPosition = new Vector3(1000f, 1000f, 1000f);
	public float sightRadius=10f;
	public float hearingRadius = 2.05f;

	private NavMeshAgent nav;
	private Collider[] hitColliders;	// all colliders within range and within colliderMask
	public List<Collider> targetColliders = new List<Collider>(); // targetable colliders based on NPC tag and LOS colliders and alert colliders if NPC chasesNonLineOfSight
	public List<float> targetDistances = new List<float>();  //target distances from this NPC


	//private SphereCollider col;
	private EnemyAI enemyAI;
	private LookAtGameObject lookAtGameObject;
	private Transform target;
	private PlayerStats playerStats; 	// Contains last player sighting info (position, resetPosition) and player health bool (playerIsAlive)
	//private Transform playerTransform;

	private float weaponRange;
	private float sightlessScreenBufferPercentage;

	[HideInInspector] public bool targetLostThisFrame;
	private bool hadTargetLastFrame;
	private Vector3 previousSighting;




	void Start(){
		nav = GetComponentInParent<NavMeshAgent> ();
		//col = transform.GetComponent<SphereCollider> ();
		enemyAI = GetComponentInParent<EnemyAI> ();

		// Set up target info if there is an initial target...
		lookAtGameObject = GetComponentInParent<LookAtGameObject> ();

		personalLastSighting = targetResetPosition;
		playerStats = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<PlayerStats> ();


		//Determine Random sightlessScreenBufferPercentage
		sightlessScreenBufferPercentage = Random.Range (sightlessScreenBufferPercentageMin, sightlessScreenBufferPercentageMax)/100f;

	}


	void Update(){
		//Test to see if the enemy is within the screen buffer.  If so, turn on sight collider.
		if(!insideScreenBuffer){
			Vector3 viewPos = Camera.main.WorldToViewportPoint (transform.position);
			if (viewPos.x > sightlessScreenBufferPercentage && viewPos.x < (1f-sightlessScreenBufferPercentage) && viewPos.y > sightlessScreenBufferPercentage && viewPos.y < (1f - sightlessScreenBufferPercentage))
			{
				insideScreenBuffer = true;
			}
		}

		//By default the target is not in sight or in range.
		targetInSight = false;
		targetInRange = false;
		nav.stoppingDistance = patrolStoppingDistance;

		//Clear lists...
		targetColliders.Clear ();
		targetDistances.Clear ();

		if(ableToAttack && insideScreenBuffer){
			hitColliders = Physics.OverlapSphere(transform.position, sightRadius, colliderMask);
			foreach (Collider other in hitColliders){

				//Check to see if the collider is a viable target by tag comparison...
				if(transform.parent.tag == Tags.enemy && other.gameObject.tag == Tags.player && playerStats.playerIsAlive){

					//Create a vector from the enemy to the target and store the angle between it and forward.
					direction = other.transform.position - transform.position;

					//test if the collider is within the hearing radius...
					if(direction.magnitude < hearingRadius){
						RaycastHit hit;
						
						//... and if a raycast towards the target hits something...
						if(Physics.Raycast (transform.position, direction.normalized, out hit, hearingRadius, sightMask)){

							// ... and if the raycast hits the target, save it as a viable target...
							if(hit.transform.tag == Tags.player){
								targetColliders.Add (other);
								// ... and its distance from this NPC...
								targetDistances.Add(hit.distance);
							}
						}
					}
					// else, check to see if it is in the cone of sight...
					else{
						float angle = Vector3.Angle(direction, transform.forward);

						//If the angle between forward and where the target is, is less than half the angle of view...
						if(angle<fieldOfViewAngle*0.5f)
						{
							RaycastHit hit;
							
							//... and if a raycast towards the target hits something...
							if(Physics.Raycast (transform.position, direction.normalized, out hit, sightRadius, sightMask)){
								
								// ... and if the raycast hits the target, save it as a viable target...
								if(hit.transform.tag == Tags.player){
									targetColliders.Add (other);
									// ... and its distance from this NPC...
									targetDistances.Add(hit.distance);
								}
							}
						}
					}
						
				}
				else if(transform.parent.tag == Tags.npc && other.gameObject.tag == Tags.enemy && other.transform.GetComponent<EnemyStats>().enemyIsAlive){

					//Create a vector from the enemy to the target and store the angle between it and forward.
					direction = other.transform.position - transform.position;

					//test if the collider is within the hearing radius...
					if(direction.magnitude < hearingRadius){
						RaycastHit hit;
						
						//... and if a raycast towards the target hits something...
						if(Physics.Raycast (transform.position, direction.normalized, out hit, hearingRadius, sightMask)){
							
							// ... and if the raycast hits the target, save it as a viable target...
							if(hit.transform.tag == Tags.enemy){	
								targetColliders.Add (other);
								// ... and its distance from this NPC...
								targetDistances.Add(hit.distance);
							}
						}
					}
					// else, check to see if it is in the cone of sight...
					else{
						float angle = Vector3.Angle(direction, transform.forward);
						
						//If the angle between forward and where the target is, is less than half the angle of view...
						if(angle<fieldOfViewAngle*0.5f)
						{
							RaycastHit hit;
							
							//... and if a raycast towards the target hits something...
							if(Physics.Raycast (transform.position, direction.normalized, out hit, sightRadius, sightMask)){
								
								// ... and if the raycast hits the target, save it as a viable target...
								if(hit.transform.tag == Tags.enemy){	
									targetColliders.Add (other);
									// ... and its distance from this NPC...
									targetDistances.Add(hit.distance);
								}
							}
						}
					}
				}
				else if(transform.parent.tag == Tags.enemy && other.gameObject.tag == Tags.npc && other.transform.GetComponent<EnemyStats>().enemyIsAlive){

					//Create a vector from the enemy to the target and store the angle between it and forward.
					direction = other.transform.position - transform.position;

					//test if the collider is within the hearing radius...
					if(direction.magnitude < hearingRadius){
						RaycastHit hit;
						
						//... and if a raycast towards the target hits something...
						if(Physics.Raycast (transform.position, direction.normalized, out hit, hearingRadius, sightMask)){
							
							// ... and if the raycast hits the target, save it as a viable target...
							if(hit.transform.tag == Tags.npc){
								targetColliders.Add (other);
								// ... and its distance from this NPC...
								targetDistances.Add(hit.distance);
							}
						}
					}
					// else, check to see if it is in the cone of sight...
					else{
						float angle = Vector3.Angle(direction, transform.forward);
						
						//If the angle between forward and where the target is, is less than half the angle of view...
						if(angle<fieldOfViewAngle*0.5f)
						{
							RaycastHit hit;
							
							//... and if a raycast towards the target hits something...
							if(Physics.Raycast (transform.position, direction.normalized, out hit, sightRadius, sightMask)){
								
								// ... and if the raycast hits the target, save it as a viable target...
								if(hit.transform.tag == Tags.npc){
									targetColliders.Add (other);
									// ... and its distance from this NPC...
									targetDistances.Add(hit.distance);
								}
							}
						}
					}
				}
				else if (other.gameObject.tag == Tags.strike && transform.parent.tag == Tags.enemy){
					//See if the strike is in direct line of sight...

					RaycastHit hit;
					if(Physics.Raycast (transform.position, other.transform.position - transform.position, out hit, hearingRadius, sightMask)){
						//if the hit distance is greater than the distance to the alert...

						if(hit.distance > ((other.transform.position - transform.parent.position).magnitude-0.1f)){
							//the strike is between this NPC and the obstruction... save as viable target...
							targetColliders.Add (other);
							//... and its distance from this NPC...
							targetDistances.Add (hit.distance);

						}
						// else, don't save this target.
					}
				}
				else if(other.gameObject.tag == Tags.alert && transform.parent.tag == Tags.enemy){

					//See if the alert is in direct line of sight...
					RaycastHit hit;
					if(Physics.Raycast (transform.position, other.transform.position - transform.position, out hit, sightRadius, sightMask) && !chasesNonLineOfSight){
						//if the hit distance is greater than the distance to the alert...
						if(hit.distance > ((other.transform.position - transform.position).magnitude-0.1f)){
							//the alert is between this NPC and the obstruction... save as viable target...
							targetColliders.Add (other);
							//... and its distance from this NPC...
							targetDistances.Add (hit.distance);

						}
						// else, don't save this target.
					}
					//else this NPC chases Non Line Of Sight alerts.  Test to see if the path length is within range...
					else if(CalculatePathLength(other.gameObject.transform.position)<= sightRadius){
						//the alert is within range.  Save as a viable target...
						targetColliders.Add (other);
						//... and its distance from this NPC...
						targetDistances.Add (CalculatePathLength(other.gameObject.transform.position));

					}
				}
			}

			//Now we have a list of viable targets (targetColliders) and a list of their distances (targetDistances) from this NPC
			//Need to determine the closest viable target and set it as the real target...

			if(targetColliders.Count> 0){
				float minDistance = sightRadius;
				int minIndex = 0;
				for (int i = 0; i < targetDistances.Count; i++ ) {
					if(targetDistances[i] < minDistance){
						minDistance = targetDistances[i];
						minIndex = i;
					}
				}

				if(targetColliders[minIndex].tag == Tags.alert){
					personalLastSighting = targetColliders[minIndex].transform.parent.position;
					if(targetColliders[minIndex].GetComponent<BoxCollider>()){
						targetColliders[minIndex].GetComponent<BoxCollider>().enabled = false;
					}
				}
				else if(targetColliders[minIndex].tag == Tags.strike){
					personalLastSighting = GameObject.FindGameObjectWithTag (Tags.player).transform.position;
					if(targetColliders[minIndex].GetComponent<BoxCollider>()){
						targetColliders[minIndex].GetComponent<BoxCollider>().enabled = false;
					}
				}
				else{
					//Set target to minimum distance one...
					lookAtGameObject.target = targetColliders[minIndex].transform;

					//Create a vector from the enemy to the target and store the angle between it and forward.
					direction2 = targetColliders[minIndex].transform.position - transform.position;
								
					//If the magnitude of the direction vector is less than the range, then the target is within attack range.
					if(direction2.sqrMagnitude <= equippedWeaponAttackRange*equippedWeaponAttackRange){
						targetInRange = true;
					}
					//... the target is in sight.
					targetInSight = true;
					//test if enemy is in melee mode
					if(enemyAI.isMelee){
						equippedWeaponAttackRange = patrolStoppingDistance;
					}
					
					nav.stoppingDistance = equippedWeaponAttackRange;
					enemyAI.UpdateTarget();

					//Set the last global sighting to the target's current position.
					personalLastSighting = lookAtGameObject.target.position;
					previousSighting = personalLastSighting;
					hadTargetLastFrame = true;
				}
			}
			else{
				//Set target to null.
				lookAtGameObject.target = null;

				if(hadTargetLastFrame){
					targetLostThisFrame = true;
					personalLastSighting = previousSighting;
				}
				else{
					targetLostThisFrame = false;
				}

				hadTargetLastFrame = false;
			}
		}
	}

	float CalculatePathLength (Vector3 targetPosition){
		//Create a path and set it based on a target position.
		NavMeshPath path = new NavMeshPath ();

		if(nav.enabled)
			nav.CalculatePath(targetPosition, path);

		//Create an array of points which is the length of the number of corners in the path + 2.
		Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

		//The first point is the enemy's position.
		allWayPoints [0] = transform.position;

		//The last point is the target position.
		allWayPoints [allWayPoints.Length - 1] = targetPosition;

		//The points in between are the corners of the path.
		for(int i = 0; i < path.corners.Length; i++)
		{
			allWayPoints[i+1] = path.corners[i];
		}

		//Create a float to store the path length that is by default 0.
		float pathLength = 0;

		//Increment the path length by an amount equal to the distance between each waypoint and the next.
		for(int i = 0; i<allWayPoints.Length - 1; i++)
		{
			pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i+1]);
		}

		return pathLength;
	}



}
