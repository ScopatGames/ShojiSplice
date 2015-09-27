using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public float avgReactionTime = 0.25f;
	public float avgAttackMomentumTime = 0.5f;
	public bool isOnPatrol= true;
	public bool wasPatrolling;
	public float patrolSpeed = 1.5f;
	public float chaseSpeed = 4.5f;
	public float chaseWaitTime = 5f;
	public float patrolWaitTime = 2f;
	public float searchIncrement = 1f;
	public Transform[] patrolWayPoints;
	public bool isAttacking;
	public bool isMelee;
	public bool opensDoorsOnPatrol = true;
	[HideInInspector] public bool autoOpensDoors = false;
	public Transform initialPosition;
	public Vector3 navPosition =  new Vector3(0f, 0f, 0f);
	[HideInInspector] public Vector3 targetResetPosition = new Vector3(1000f, 1000f, 1000f);

	private LookAtGameObject lookAtGameObject;

	private EnemySight enemySight;
	private NavMeshAgent nav;
	private PlayerStats playerStats; 	// Contains last player sighting info (position, resetPosition) and player health bool (playerIsAlive)
	private EnemyStats enemyStats;
	private float chaseTimer;
	private float patrolTimer;
	private float searchIncrementTimer;
	private float reactionTime;
	private float reactionTimer;
	private float attackMomentumTimer;
	public float attackMomentumTime;
	private int wayPointIndex;
	private float animSpeed;
	private bool onWayBackToPatrol = false;

	AnimatorControllerParameter[] acp;
	Animator[] anims;
	public Animator anim=null;
	int sightedHash = Animator.StringToHash("sighted");
	int sightedmeleeHash = Animator.StringToHash("sightedmelee");
	int lostSightHash = Animator.StringToHash("lostSight");


	void Start(){
	
		// Set up references
		enemySight = GetComponentInChildren<EnemySight> ();
		nav = GetComponent<NavMeshAgent> ();
		attackMomentumTimer = 0f;
		chaseTimer = 0f;
		reactionTime = Random.Range (avgReactionTime * 0.9f, avgReactionTime * 1.1f);
		ResetAttackMomentumTime ();

		playerStats = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<PlayerStats> ();

		if(initialPosition){
			navPosition = initialPosition.position;
		}
		else{
			navPosition = transform.position;
		}

		// Set up target info if there is an initial target...
		lookAtGameObject = GetComponent<LookAtGameObject> ();
		if(lookAtGameObject.target != null){
			UpdateTarget ();
		}

		isAttacking = false;
		wasPatrolling = true;


		if(gameObject.GetComponentInChildren<weaponIndex>().ammoCount == 0){
			isMelee = true; 
		}


	}

	void Update(){

		if (!anim) {
			anims = GetComponentsInChildren<Animator> ();	
			foreach(Animator a in anims){
				acp = a.parameters;
				foreach(AnimatorControllerParameter b in acp){
					if(b.name == "sighted" || b.name == "sightedmelee"){
						anim = a;
					}
				}
			}
		}

		// If enemy is patrolling and has a target...
		if(isOnPatrol && lookAtGameObject.target != null){ 
			// If the target is the player...
			if(lookAtGameObject.target.tag == Tags.player){ 
				//If the player is in sight, in range, and is alive...
				if(enemySight.targetInSight && enemySight.targetInRange && playerStats.playerIsAlive){ 
					// ... attack!

					wasPatrolling = false;
					Attacking();
				}

				//If the player has been sighted and isn't dead...
				else if(enemySight.personalLastSighting != targetResetPosition && playerStats.playerIsAlive){
					// ... chase.

					//If the last frame was patrolling, change animation to combat ready...
					if(wasPatrolling){
						//determine if the NPC is melee or ranged attack...
						if(isMelee){
							// Set the animation "sightedmelee" trigger...
							anim.SetTrigger(sightedmeleeHash);
						}
						else{
							//Set the animation "sighted" trigger...
							anim.SetTrigger (sightedHash);
						}
					}
					wasPatrolling = false;
					Chasing();
				}

				//Otherwise...
				else {
					// ... patrol.


					//Check to see if previous frame was NOT patrolling...
					if(!wasPatrolling){
						//Set the animation "lostSight" trigger to change to peace mode animation...
						anim.SetTrigger (lostSightHash);
						chaseTimer = 0f;
						reactionTimer = 0f;
						attackMomentumTimer = 0f;
					}
					wasPatrolling = true;
					Patrolling();
				}
			}
			//Else if the target is not the player...
			else if(lookAtGameObject.target.tag != Tags.player){
				//If the target is in sight, in range, and is alive...
				if(enemySight.targetInSight && enemySight.targetInRange && enemyStats.enemyIsAlive){
					// ... attack!

					
					//Check to see if previous frame was patrolling...
					if(wasPatrolling){
						anim.SetTrigger (sightedHash);
					}
					wasPatrolling = false;
					Attacking();
				}
				
				//If the target has been sighted and isn't dead...
				else if(enemySight.personalLastSighting != targetResetPosition && enemyStats.enemyIsAlive){
					// ... chase.

					if(wasPatrolling && isMelee){
						anim.SetTrigger (sightedmeleeHash);
					}
					wasPatrolling = false;
					Chasing();
				}
				
				//Otherwise...
				else {
					// ... patrol.


					//Check to see if previous frame was NOT patrolling...
					if(!wasPatrolling){
						anim.SetTrigger (lostSightHash);
						chaseTimer = 0f;
						reactionTimer = 0f;
						attackMomentumTimer = 0f;
					}
					wasPatrolling = true;
					Patrolling();
				}
			}
		}
		// Else if the enemy is on patrol and does not have a target...  I think this is for when the target is dead, or sight lost, or for alerts...
		else if(isOnPatrol && lookAtGameObject.target == null){
			//If the enemy has a sighting, but the target was not JUST lost this frame...
			if(enemySight.personalLastSighting != targetResetPosition){
				// ... chase.

				if(wasPatrolling && !enemySight.targetLostThisFrame){
					//determine if the NPC is melee or ranged attack...

					if(isMelee){
						// Set the animation "sightedmelee" trigger...
						anim.SetTrigger(sightedmeleeHash);
					}
					else{
						//Set the animation "sighted" trigger...
						anim.SetTrigger (sightedHash);
					}
				} 
				wasPatrolling = false;
				Chasing ();
			}
			else{


				//Check to see if previous frame was NOT patrolling...
				if(!wasPatrolling){
					if(anim){
						anim.SetTrigger (lostSightHash);
						reactionTimer = 0f;
						attackMomentumTimer = 0f;
					}
					nav.stoppingDistance = enemySight.patrolStoppingDistance;
				}
				wasPatrolling = true;
				Patrolling();
			}
		}
		else if(!isOnPatrol){
			nav.Resume ();
			nav.destination = navPosition;
		}
	}

	void Attacking(){
		if(reactionTimer<reactionTime && !isMelee){
			//Hesitate...
			reactionTimer += Time.deltaTime;
		}
		else{ //Attack!...
			nav.Stop ();
		
			if (!nav.updateRotation) {
				nav.updateRotation = true;		
			}
			isAttacking = true;
			if(gameObject.GetComponentInChildren<weaponIndex>().ammoCount == 0){
				isMelee = true; 
			}
		}
	}

	void ResetAttackMomentumTime(){
		attackMomentumTime = Random.Range (avgAttackMomentumTime * 0.85f, avgAttackMomentumTime * 2f);
	}

	void Chasing(){
		if (isAttacking && !isMelee && attackMomentumTimer < attackMomentumTime) {
			//Keep Attacking...
			Attacking();
			attackMomentumTimer += Time.deltaTime;
		}
		else{ //Chase...
			nav.Resume ();
			isAttacking = false;
			ResetAttackMomentumTime();
			attackMomentumTimer = 0f;
			autoOpensDoors = true;
			if (!nav.updateRotation) {
				nav.updateRotation = true;		
			}
			//Create a vector from the enemy to the last sighting of the target.
			Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;


			//If the last personal sighting of the target is not close...
			if(sightingDeltaPos.sqrMagnitude > nav.stoppingDistance*nav.stoppingDistance){
				//...set the destination for the NavMeshAgent to 2 units beyond the last personal sighting of the target if no blockages.
				RaycastHit testBlockages;
				if(Physics.Raycast(enemySight.personalLastSighting, enemySight.direction2.normalized, out testBlockages, 2f, enemySight.sightMask)){
					if(testBlockages.transform.tag == Tags.wall || testBlockages.transform.tag == Tags.door){
						nav.destination = enemySight.personalLastSighting;
					}
					else{
						nav.destination = enemySight.personalLastSighting+ enemySight.direction2.normalized*2f;			
					}
				}
				else{
					nav.destination = enemySight.personalLastSighting+ enemySight.direction2.normalized*2f;			
				}
			}
			
			//Set the appropriate speed for the NavMeshAgent.
			nav.speed = chaseSpeed;

			//If near the last personal sighting...
			if(nav.remainingDistance <= nav.stoppingDistance)
			{
				reactionTimer = 0f;

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
					enemySight.personalLastSighting = targetResetPosition;
					chaseTimer = 0f;
					searchIncrementTimer = 0f;
					onWayBackToPatrol = true;

				}
			}
			else{
				//If not near the last personal sighting of the player, reset the timer.
				chaseTimer = 0f;
				searchIncrementTimer = 0f;
			}
		}
	}

	void Patrolling(){
		if (isAttacking && !isMelee && attackMomentumTimer < attackMomentumTime) {
			//Keep Attacking...
			Attacking();
			attackMomentumTimer += Time.deltaTime;
		}
		else{
			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
			int chaseStateHash = Animator.StringToHash("Base Layer.Chase");
			int chaseMeleeStateHash = Animator.StringToHash ("Base Layer.ChaseMelee");
			
			if (stateInfo.fullPathHash == chaseStateHash || stateInfo.fullPathHash == chaseMeleeStateHash) {
				anim.SetTrigger(lostSightHash);
				chaseTimer = 0f;
				reactionTimer = 0f;
				attackMomentumTimer = 0f;
			}

			nav.Resume ();

			//Set an appropriate speed for the NavMeshAgent.
			isAttacking = false;
			ResetAttackMomentumTime();
			attackMomentumTimer = 0f;
			nav.speed = patrolSpeed;
			nav.updateRotation = true;

			//Send enemy velocity to the animation parameter...
			animSpeed = nav.desiredVelocity.sqrMagnitude/(patrolSpeed*patrolSpeed);
			if(anim){
				anim.SetFloat ("speed", animSpeed);
			}

			//If near the next waypoint or there is no destination...
			if(nav.destination == targetResetPosition || nav.remainingDistance<nav.stoppingDistance)
			{
				if(onWayBackToPatrol){
					autoOpensDoors = true;
				}
				else{
					autoOpensDoors = false;
				}

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
					onWayBackToPatrol = false;
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

	public void UpdateTarget(){
		if(lookAtGameObject.target.tag != Tags.player){
		
			enemyStats = lookAtGameObject.target.GetComponent<EnemyStats>();
			
		}
	}
	
}
