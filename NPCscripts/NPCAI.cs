using UnityEngine;
using System.Collections;

public class NPCAI : MonoBehaviour {

	public float patrolSpeed = 1.5f;
	public float patrolWaitTime = 2f;
	public float searchIncrement = 1f;
	public Transform[] patrolWayPoints;
	public bool opensDoorsOnPatrol = true;
	public bool autoOpensDoors = true;
	public Transform initialPosition;
	public Vector3 navPosition =  new Vector3(0f, 0f, 0f);
	public bool isPatrolling = true;
	
	private NavMeshAgent nav;
	private float patrolTimer;
	private float searchIncrementTimer;
	private int wayPointIndex;
	private bool wasPatrolling;
	private float animSpeed;
	//private LookAtGameObject lookAtGO;
	
	Animator anim=null;

	
	void Start(){
		// Set up references
		nav = GetComponent<NavMeshAgent> ();
		//lookAtGO = GetComponent<LookAtGameObject> ();
		if(initialPosition){
			navPosition = initialPosition.position;
			//lookAtGO.target = initialPosition;
			//lookAtGO.enabled = true;

		}
		else {
			navPosition = transform.position;
		}

		//This needs to be initialized in Start() because the Animator is instantiated in Awake() in NonPlayerCharacter()
		anim = GetComponentInChildren<Animator> ();
	}
	
	void Update(){
		if(isPatrolling){

			Patrolling();
		}
		else {
			nav.destination = navPosition;
		}
	}

	void Patrolling(){
		//Set an appropriate speed for the NavMeshAgent.
		nav.speed = patrolSpeed;
		nav.updateRotation = true;

		//Send enemy velocity to the animation parameter...
		animSpeed = nav.velocity.magnitude/patrolSpeed;
		anim.SetFloat ("speed", animSpeed);
		
		//If near the next waypoint or there is no destination...
		if(nav.remainingDistance<nav.stoppingDistance)
		{
			autoOpensDoors = false;
			if(nav.updateRotation){
				nav.updateRotation = false;
			}
			
			//... increment the timer.
			patrolTimer += Time.deltaTime;
			
			//... increment the search timer.
			searchIncrementTimer += Time.deltaTime;
			
			if(searchIncrementTimer >= searchIncrement)
			{
				//update the rotation of the enemy to a random search value...
				Vector2 randomRotation = Random.insideUnitCircle;
				Quaternion newRotation = Quaternion.LookRotation(new Vector3(randomRotation.x, 0.0f, randomRotation.y), Vector3.up);
				transform.rotation = newRotation;
				searchIncrementTimer = 0;
			}
			
			//If the timer exceeds the wait time...
			if (patrolTimer>=patrolWaitTime)
			{
				//... increment the wayPointIndex.
				if(wayPointIndex == patrolWayPoints.Length -1)
					wayPointIndex = 0;
				else
					wayPointIndex++;
				
				//Reset the timer.
				patrolTimer =0;
				//nav.updateRotation = true;
			}
		}
		else{
			//If not near a destination, reset the timer.
			patrolTimer = 0;
			searchIncrementTimer = 0;
		}
		//Set the destination to the patrolWayPoint.
		nav.destination = patrolWayPoints [wayPointIndex].position;
	}

	

	

}
