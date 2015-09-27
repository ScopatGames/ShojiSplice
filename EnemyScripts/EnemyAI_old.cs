using UnityEngine;
using System.Collections;

public class EnemyAI_old : MonoBehaviour {
	/*
	public float patrolSpeed = 1.5f;
	public float chaseSpeed = 4.5f;
	public float chaseWaitTime = 5f;
	public float patrolWaitTime = 2f;
	public float searchIncrement = 1f;
	public Transform[] patrolWayPoints;
	public bool isAttacking;
	public bool isMelee;
	public bool opensDoorsOnPatrol = true;
	public bool autoOpensDoors = true;
	public Transform initialPosition;
	public Vector3 navPosition =  new Vector3(0f, 0f, 0f);
	
	
	
	
	private EnemySight enemySight;
	private NavMeshAgent nav;
	private PlayerStats playerStats; 	// Contains last player sighting info (position, resetPosition) and player health bool (playerIsAlive)
	private float chaseTimer;
	private float patrolTimer;
	private float searchIncrementTimer;
	private int wayPointIndex;
	private bool wasPatrolling;
	private float animSpeed;
	
	Animator anim=null;
	int sightedHash = Animator.StringToHash("sighted");
	int sightedmeleeHash = Animator.StringToHash("sightedmelee");
	int lostSightHash = Animator.StringToHash("lostSight");
	
	
	void Start(){
		// Set up references
		enemySight = GetComponentInChildren<EnemySight> ();
		nav = GetComponent<NavMeshAgent> ();
		
		if(initialPosition){
			navPosition = initialPosition.position;
		}
		
		
		//player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		playerStats = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<PlayerStats> ();
		isAttacking = false;
		wasPatrolling = true;
		
		//This needs to be initialized in Start() because the Animator is instantiated in Awake() in NonPlayerCharacter()
		anim = GetComponentInChildren<Animator> ();
		if(gameObject.GetComponentInChildren<weaponIndex>().ammoCount == 0){
			isMelee = true; 
		}
		
		
	}
	
	void Update(){
		//If the player is in sight, in range, and is alive...
		if(enemySight.playerInSight && enemySight.playerInRange && playerStats.playerIsAlive){
			// ... attack!
			Attacking();
			
			//Check to see if previous frame was patrolling...
			if(wasPatrolling){
				anim.SetTrigger (sightedHash);
				//Debug.Log ("sighted!");
				
			}
			wasPatrolling = false;
		}
		
		//If the player has been sighted and isn't dead...
		else if(enemySight.personalLastSighting != playerStats.resetPosition && playerStats.playerIsAlive){
			// ... chase.
			Chasing();
			//Debug.Log ("Chasing");
			if(wasPatrolling && isMelee){
				anim.SetTrigger (sightedmeleeHash);
				wasPatrolling = false;
			}
		}
		
		//Otherwise...
		else {
			// ... patrol.
			Patrolling();
			//Debug.Log ("Patrolling");
			
			//Check to see if previous frame was NOT patrolling...
			if(!wasPatrolling){
				anim.SetTrigger (lostSightHash);
				//Debug.Log ("lost sight!");
			}
			wasPatrolling = true;
		}
	}
	
	void Attacking(){
		nav.Stop ();
		
		if (!nav.updateRotation) {
			nav.updateRotation = true;		
		}
		isAttacking = true;
		if(gameObject.GetComponentInChildren<weaponIndex>().ammoCount == 0){
			isMelee = true; 
		}
	}
	
	void Chasing(){
		isAttacking = false;
		autoOpensDoors = true;
		if (!nav.updateRotation) {
			nav.updateRotation = true;		
		}
		//Create a vector from the enemy to the last sighting of the player.
		Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
		
		//If the last personal sighting of the player is not close...
		if(sightingDeltaPos.sqrMagnitude > 4f){
			//...set the destination for the NavMeshAgent to the last personal sighting of the player.
			nav.destination = enemySight.personalLastSighting+ enemySight.direction.normalized*2f;
		}
		//Set the appropriate speed for the NavMeshAgent.
		nav.speed = chaseSpeed;
		
		//If near the last personal sighting...
		if(nav.remainingDistance <= nav.stoppingDistance)
		{
			//... increment the chase timer.
			chaseTimer += Time.deltaTime;
			
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
			if(chaseTimer>=chaseWaitTime)
			{
				// ... reset last global sighting, the last personal sighting and the timer.
				//playerStats.position = playerStats.resetPosition;
				enemySight.personalLastSighting = playerStats.resetPosition;
				chaseTimer = 0f;
				searchIncrementTimer = 0f;
				
			}
		}
		else{
			//If not near the last personal sighting of the player, reset the timer.
			chaseTimer = 0f;
			searchIncrementTimer = 0f;
		}
	}
	
	void Patrolling(){
		//Set an appropriate speed for the NavMeshAgent.
		isAttacking = false;
		nav.speed = patrolSpeed;
		nav.updateRotation = true;
		
		
		
		//Send enemy velocity to the animation parameter...
		animSpeed = nav.velocity.magnitude/patrolSpeed;
		anim.SetFloat ("speed", animSpeed);
		
		//If near the next waypoint or there is no destination...
		if(nav.destination == playerStats.resetPosition || nav.remainingDistance<nav.stoppingDistance)
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
	
	
*/	
}
