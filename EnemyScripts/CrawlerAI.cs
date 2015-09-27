using UnityEngine;
using System.Collections;

public class CrawlerAI : MonoBehaviour {

	public bool isAggressive;
	public float fuseTime = 0.5f;

	public GameObject explodeAnimation;
	public GameObject explodeAudio;
	public float cameraShakeMagnitude = 0.5f;
	public float maxCameraShakeDistance = 10.0f;
	
	SmoothCamera2D shakeCamera;
	float distanceFromCamera;
	float cameraShakeFactor;


	public float courseChangeTimeIncrementMax = 10f;
	public float courseMaxRadius = 3f;
	public float sightRadius;
	public LayerMask colliderMask;
	public LayerMask obstacleMask;
	public float crawlSpeed;
	public float fleeSpeed;
	public float chaseStoppingDistance = 1.0f;

	private Animator anim;
	private NavMeshAgent nav;
	private bool isTicking;
	private float courseTimer;
	private float fuseTimer;
	private Vector3 targetPosition;
	private float courseChangeTimeIncrement;
	private Collider[] hitColliders;
	private bool targetInSight;
	private bool targetWasInSight;
	private EnemyStats enemyStats;

	// Use this for initialization
	void Start () {
		enemyStats = GetComponent<EnemyStats> ();
		nav = GetComponent<NavMeshAgent> ();
		anim = GetComponentInChildren<Animator> ();
		courseChangeTimeIncrement = Random.Range (3f, courseChangeTimeIncrementMax);
		courseTimer = courseChangeTimeIncrement;
		fuseTimer = 0f;
		nav.speed = crawlSpeed;
		shakeCamera = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<SmoothCamera2D> ();

	}
	
	// Update is called once per frame
	void Update () {

		targetInSight = false;
		hitColliders = Physics.OverlapSphere(transform.position, sightRadius, colliderMask);

		foreach(Collider other in hitColliders){
			//Test to see if an obstacle is between the Crawler and the target...
			RaycastHit hit;
			if (!Physics.Raycast (transform.position, other.transform.position - transform.position, out hit, (other.transform.position - transform.position).magnitude, obstacleMask)) {
				//There is no obstacle...
				targetInSight = true;
				targetPosition = other.transform.position;
			}
		}

		if(!targetInSight){
			if(targetWasInSight){
				nav.destination = transform.position;
				courseTimer = courseChangeTimeIncrement;
				targetWasInSight = false;
			}
			nav.speed = crawlSpeed;
			nav.stoppingDistance = 0f;
			courseTimer += Time.deltaTime;
			if(courseTimer > courseChangeTimeIncrement){
				//update course
				//Get a random target within the maximum radius
				Vector2 targetPosition2D = Random.insideUnitCircle*courseMaxRadius;
				targetPosition = new Vector3(targetPosition2D.x, 0.0f, targetPosition2D.y);
				targetPosition = transform.position + targetPosition;


				//Create a path and set it based on a target position.
				NavMeshPath path = new NavMeshPath ();
				
				if(nav.enabled){
					if(nav.CalculatePath(targetPosition, path)){
						RaycastHit hit;
						if (!Physics.Raycast (transform.position, targetPosition-transform.position, out hit, (targetPosition - transform.position).magnitude, obstacleMask)) {
							//Debug.Log (transform.position);
							//Debug.Log (targetPosition);
							nav.destination = targetPosition;
							courseTimer = 0f;
							courseChangeTimeIncrement = Random.Range (3f, courseChangeTimeIncrementMax);	
						}
						else{
							//Debug.Log("Hit Wall");
						}
					}
				}
			}
		}
		else{
			targetWasInSight = true;
			nav.speed = fleeSpeed;
			if(isAggressive){

				nav.stoppingDistance = chaseStoppingDistance;

				if(nav.remainingDistance <= chaseStoppingDistance){
					if(!isTicking){
						anim.SetTrigger ("triggerShake");
						isTicking= true;
					}
					fuseTimer += Time.deltaTime;
					if(fuseTimer >= fuseTime){
						crawlerExplode();

						Invoke ("destroyCrawler", 0.1f);
						fuseTimer = 0f;
					}
				}
				else{
					if(isTicking){
						anim.SetTrigger ("triggerStopShake");
						isTicking =false;
					}
					fuseTimer = 0f;
				}
			}
			else{
				targetPosition = (targetPosition - transform.position)*-0.3f; //The 0.3 provides an offset so it does not run into the wall
				targetPosition = transform.position + targetPosition;
			}
			//Create a path and set it based on a target position.
			NavMeshPath path = new NavMeshPath ();
			
			if(nav.enabled){
				if(nav.CalculatePath(targetPosition, path)){
					nav.destination = targetPosition;
					courseTimer = 0f;
					courseChangeTimeIncrement = Random.Range (3f, courseChangeTimeIncrementMax);
				}
			}
		}
	}

	public void crawlerExplode(){
		GetComponentInChildren<SphereCollider> ().enabled = true;
		GetComponentInChildren<DestroyByExplosion> ().ExplodeAndDamage ();
		GetComponentInChildren<SpriteRenderer> ().enabled = false;
		Instantiate (explodeAudio, transform.position, transform.rotation);
		Instantiate (explodeAnimation, transform.position, Quaternion.AngleAxis (Random.Range (0f, 360f),Vector3.up));
				
		//Camera Shake...
		distanceFromCamera = new Vector2 (shakeCamera.transform.position.x - transform.position.x, shakeCamera.transform.position.z - transform.position.z ).magnitude;
		if(distanceFromCamera <= 1.0f){
			cameraShakeFactor = 1.0f;
		}
		else if (distanceFromCamera < maxCameraShakeDistance) {
			cameraShakeFactor = 1.0f/(distanceFromCamera*distanceFromCamera)-1.0f/(maxCameraShakeDistance*maxCameraShakeDistance);		
		}
		else {
			cameraShakeFactor = 0f;
		}
		shakeCamera.shakeCamera (cameraShakeFactor*cameraShakeMagnitude, transform.position);

	
	}

	public void destroyCrawler(){
		enemyStats.enemyHealth = -100f;

	}

}
