using UnityEngine;
using System.Collections;

public class ShootBullet : MonoBehaviour {


	public float fireRate;
	public Transform shotSpawn;
	public GameObject bullet;
	public GameObject weaponEffects;
	public bool isEquippedByPlayer;
	public LayerMask wallTestLayerMask;
	public float inaccuracyTime = 3.0f;
	private Vector3 collisionTest;
	private float lastFireTime;
	private float thisFireTime;
	private float inaccuracyScale;
	private SmoothCamera2D shakeCam;
	private float shakeMagnitude;
	private PlayerInfoPanel playerInfoPanel;


	Animator anim;
	int sightedHash = Animator.StringToHash("sighted");
	int sightedmeleeHash = Animator.StringToHash("sightedmelee");
	int attackHash = Animator.StringToHash("attack");
	int equipStateHash = Animator.StringToHash("Base Layer.Equip");


	[HideInInspector] public float nextFire;

	void Start(){
		if(gameObject.transform.parent.name == "PlayerEmpty"){
			isEquippedByPlayer = true;
			playerInfoPanel = GameObject.FindGameObjectWithTag(Tags.canvas).GetComponentInChildren<PlayerInfoPanel>();
		}
		else{
			isEquippedByPlayer = false;
		}
		anim = GetComponentInChildren<Animator> ();
		shakeCam = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<SmoothCamera2D> ();
		shakeMagnitude = gameObject.GetComponent <weaponIndex>().cameraShakeMagnitude;


	}

	void Update () {
		if(isEquippedByPlayer){
			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
			if (Input.GetButton ("Attack") && !Input.GetButton ("Shift") && Time.time > nextFire && gameObject.GetComponent<weaponIndex>().ammoCount > 0 && stateInfo.fullPathHash != equipStateHash) {
				nextFire = Time.time + fireRate;

				//Test to see if collision is within first frame of movement; if so, instantiate bullet just before the obstacle
				RaycastHit hit;
				collisionTest = shotSpawn.position-transform.position;
				if(Physics.Raycast(transform.position,collisionTest,out hit,collisionTest.magnitude, wallTestLayerMask)){
					//Instantiate new bullet just before obstacle...
					GameObject newBullet = Instantiate (bullet, (hit.point-transform.position)*0.9f+transform.position, shotSpawn.rotation) as GameObject; 
					//Set newBullet velocity as the current gameobject's velocity... had to cut the velocity in half to stay on target... not entirely sure why...
					if(newBullet.GetComponent<Rigidbody>()){
						newBullet.GetComponent<Rigidbody>().velocity = transform.parent.GetComponent<Rigidbody>().velocity*0.5f;
					}
					//Determine the inaccuracy of the bullet based on rapidity of firing...
					thisFireTime = Time.time;
					if((thisFireTime-lastFireTime)>(fireRate*inaccuracyTime)){
						inaccuracyScale = 0.0f;
					}
					else {
						inaccuracyScale += 0.2f;
					}
					if(inaccuracyScale >1.0f){
						inaccuracyScale = 1.0f;
					}
					if(newBullet.GetComponent<bulletMover>()){
						newBullet.GetComponent<bulletMover>().inaccuracyScale = inaccuracyScale;
					}
					lastFireTime = thisFireTime;

					//Camera shake...
					shakeCam.shakeCamera(0.33f *shakeMagnitude + inaccuracyScale * 0.66f *shakeMagnitude, shotSpawn.position);


				}
				else {
					//Instantiate bullet at bullet spawn point...
					GameObject newBullet = Instantiate (bullet, shotSpawn.position, shotSpawn.rotation) as GameObject;
					//Set newBullet velocity as the current gameobject's velocity... had to cut the velocity in half to stay on target... not entirely sure why...
					if(newBullet.GetComponent<Rigidbody>()){
						newBullet.GetComponent<Rigidbody>().velocity = transform.parent.GetComponent<Rigidbody>().velocity*0.5f;
					}
					else{
						Rigidbody[] rbs = newBullet.GetComponentsInChildren<Rigidbody>();
						foreach(Rigidbody rb in rbs){
							rb.velocity = transform.parent.GetComponent<Rigidbody>().velocity*0.5f;
						}
					}
					//Determine the inaccuracy of the bullet based on rapidity of firing...
					thisFireTime = Time.time;
					if((thisFireTime-lastFireTime)>(fireRate*inaccuracyTime)){
						inaccuracyScale = 0.0f;
					}
					else {
						inaccuracyScale += 0.2f;
					}
					if(inaccuracyScale >1.0f){
						inaccuracyScale = 1.0f;
					}
					if(newBullet.GetComponent<bulletMover>()){
						newBullet.GetComponent<bulletMover>().inaccuracyScale = inaccuracyScale;
					}
					else{
						bulletMover[] bms = newBullet.GetComponentsInChildren<bulletMover>();
						foreach(bulletMover bm in bms){
							bm.inaccuracyScale = inaccuracyScale;
						}
					}
					lastFireTime = thisFireTime;

					//Camera shake...
					shakeCam.shakeCamera(0.33f*shakeMagnitude + inaccuracyScale * 0.66f*shakeMagnitude, shotSpawn.position);
				}

				//Instantiate weapon effects, if they exist...
				if(weaponEffects != null){
					Instantiate (weaponEffects, shotSpawn.position, shotSpawn.rotation);
				}

				//Reduce ammo by shot amount...
				gameObject.GetComponent<weaponIndex>().ammoCount--;
				playerInfoPanel.UpdatePrimaryWeaponInfo();

				//Perform attack animation...
				anim.SetTrigger(attackHash);
			}
		}
		else if(isEquippedByPlayer == false && gameObject.GetComponentInParent<EnemyAI>().isAttacking == true ){
			EnemyAI enemyAI = gameObject.GetComponentInParent<EnemyAI>();
			if(Time.time > nextFire && gameObject.GetComponent<weaponIndex>().ammoCount > 0){
				AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
				int chaseStateHash = Animator.StringToHash("Base Layer.Chase");

				if(stateInfo.fullPathHash == chaseStateHash){
					nextFire = Time.time + fireRate;
					//Test to see if enemy is firing directly into an obstacle.  
					RaycastHit hit;
					collisionTest = shotSpawn.position-transform.position;
					if(Physics.Raycast(transform.position,collisionTest,out hit,collisionTest.magnitude, wallTestLayerMask)){
						//Instantiate bullet just before obstacle.
						GameObject newBullet = Instantiate (bullet, (hit.point-transform.position)*0.9f+transform.position, shotSpawn.rotation) as GameObject;
						//Determine the inaccuracy of the bullet based on rapidity of firing...
						thisFireTime = Time.time;
						if((thisFireTime-lastFireTime)>(fireRate*inaccuracyTime)){
							inaccuracyScale = 0.0f;
						}
						else {
							inaccuracyScale += 0.2f;
						}
						if(inaccuracyScale >1.0f){
							inaccuracyScale = 1.0f;
						}
						if(newBullet.GetComponent<EnemyBulletMover>()){
							newBullet.GetComponent<EnemyBulletMover>().inaccuracyScale = inaccuracyScale;
						}
						lastFireTime = thisFireTime;
					}
					else {
						//Instantiate bullet at the bullet spawn location
						GameObject newBullet = Instantiate (bullet, shotSpawn.position, shotSpawn.rotation) as GameObject;
						//Determine the inaccuracy of the bullet based on rapidity of firing...
						thisFireTime = Time.time;
						if((thisFireTime-lastFireTime)>(fireRate*inaccuracyTime)){
							inaccuracyScale = 0.0f;
						}
						else {
							inaccuracyScale += 0.2f;
						}
						if(inaccuracyScale >1.0f){
							inaccuracyScale = 1.0f;
						}
						if(newBullet.GetComponent<EnemyBulletMover>()){
							newBullet.GetComponent<EnemyBulletMover>().inaccuracyScale = inaccuracyScale;
						}
						else{
							EnemyBulletMover[] bms = newBullet.GetComponentsInChildren<EnemyBulletMover>();
							foreach(EnemyBulletMover bm in bms){
								bm.inaccuracyScale = inaccuracyScale;
							}
						}

						lastFireTime = thisFireTime;
					}
					if(weaponEffects != null){
						Instantiate (weaponEffects, shotSpawn.position, shotSpawn.rotation);
					}
					
					//Instantiate (bulletAudio, shotSpawn.position, shotSpawn.rotation);
					gameObject.GetComponent<weaponIndex>().ammoCount--;
					anim.SetTrigger (attackHash);

				}
				else{//Send the chase trigger before attack...

					if(enemyAI.isMelee){
						// Set the animation "sightedmelee" trigger...
						enemyAI.anim.SetTrigger(sightedmeleeHash);
					}
					else{
						//Set the animation "sighted" trigger...
						enemyAI.anim.SetTrigger (sightedHash);
					}
					enemyAI.wasPatrolling = false;

				}
			}
		}
	}
}
