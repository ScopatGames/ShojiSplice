using UnityEngine;
using System.Collections;

public class EnemySight_old : MonoBehaviour {
	/*
	public float fieldOfViewAngle = 240f;
	public float sightlessScreenBufferPercentageMax = 15f;
	public float sightlessScreenBufferPercentageMin = 8f;
	public bool playerInSight;
	public bool playerInRange;
	public float equippedWeaponAttackRange=0f;
	public Vector3 personalLastSighting;
	public LayerMask sightMask;
	public float patrolStoppingDistance=2f;
	public Vector3 direction;
	public bool chasesNonLineOfSight = true;
	
	
	private NavMeshAgent nav;
	private SphereCollider col;
	private PlayerStats playerStats; 	// Contains last player sighting info (position, resetPosition) and player health bool (playerIsAlive)
	private GameObject player;
	private float weaponRange;
	private float sightlessScreenBufferPercentage;
	private bool ableToAttack = false; //Turn this on for an enemy once inside screen buffer
	
	
	
	
	void Start(){
		nav = GetComponentInParent<NavMeshAgent> ();
		col = transform.GetComponent<SphereCollider> ();
		
		playerStats = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<PlayerStats> ();
		player = GameObject.FindGameObjectWithTag (Tags.player);
		
		//Determine Random sightlessScreenBufferPercentage
		sightlessScreenBufferPercentage = Random.Range (sightlessScreenBufferPercentageMin, sightlessScreenBufferPercentageMax)/100f;
		
		
		// Set the personal sighting and the previous sighting to the reset position.
		personalLastSighting = playerStats.resetPosition;
		
	}
	
	void Update(){
		//Test to see if the enemy is within the screen buffer.  If so, turn on sight collider.
		if(!ableToAttack){
			Vector3 viewPos = Camera.main.WorldToViewportPoint (transform.position);
			if (viewPos.x > sightlessScreenBufferPercentage && viewPos.x < (1f-sightlessScreenBufferPercentage) && viewPos.y > sightlessScreenBufferPercentage && viewPos.y < (1f - sightlessScreenBufferPercentage))
			{
				ableToAttack = true;
			}
		}
	}
	
	void OnTriggerStay(Collider other){
		if(other.gameObject){
			//If the player has entered the trigger sphere...
			if(other.gameObject == player && ableToAttack==true)
			{
				//By default the player is not in sight or in range.
				playerInSight = false;
				playerInRange = false;
				nav.stoppingDistance = patrolStoppingDistance;
				gameObject.GetComponentInParent<LookAtGameObject>().enabled = false;
				
				//Create a vector from the enemy to the player and store the angle between it and forward.
				direction = other.transform.position - transform.position;
				float angle = Vector3.Angle(direction, transform.forward);
				
				//If the magnitude of the direction vector is less than the range, then the player is within attack range.
				if(direction.magnitude <= equippedWeaponAttackRange){
					//Debug.Log("Distance: " + direction.magnitude);
					playerInRange = true;
				}
				
				//If the angle between forward and where the player is, is less than half the angle of view...
				if(angle<fieldOfViewAngle*0.5f)
				{
					RaycastHit hit;
					
					//... and if a raycast towards the player hits something...
					Debug.DrawRay(transform.position, direction.normalized, Color.green);
					if(Physics.Raycast (transform.position, direction.normalized, out hit, col.radius, sightMask))
					{
						// ... and if the raycast hits the player...
						if(hit.collider.gameObject == player)
						{
							//... the player is in sight.
							playerInSight = true;
							if(gameObject.GetComponentInParent<EnemyAI>().isMelee){
								equippedWeaponAttackRange = patrolStoppingDistance;
							}
							
							nav.stoppingDistance = equippedWeaponAttackRange;
							
							gameObject.GetComponentInParent<LookAtGameObject>().enabled = true;
							
							//Set the last global sighting to the player's current position.
							personalLastSighting = player.transform.position;
						}
					}
				}
				
			}
			
			//else if the object is an alert from a player weapon, send the enemy to the player position...
			else if(other.gameObject.tag == Tags.alert)
			{
				//See if the alert is in direct line of sight...
				RaycastHit hit;
				if(Physics.Raycast (transform.position, other.transform.position - transform.position, out hit, col.radius, sightMask) && !chasesNonLineOfSight){
					if(hit.distance > (other.transform.position - transform.position).magnitude){
						personalLastSighting = other.gameObject.transform.position;
					}
				}
				
				else if(CalculatePathLength(other.gameObject.transform.position)<= col.radius){
					personalLastSighting = other.gameObject.transform.position;
				}
				if(other.GetComponent<BoxCollider>()){
					other.GetComponent<BoxCollider>().enabled = false;
				}
			}
		}
	}
	
	void OnTriggerExit(Collider other){
		//If the player leaves the trigger zone...
		if (other.gameObject == player){
			// ... the player is not in sight.
			playerInSight = false;
			playerInRange = false;
			nav.stoppingDistance = patrolStoppingDistance;
			gameObject.GetComponentInParent<LookAtGameObject>().enabled = false;
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
*/
}
