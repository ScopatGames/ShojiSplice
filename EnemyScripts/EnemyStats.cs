using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStats : MonoBehaviour {

	public float enemyHealth= 100.0f;

	public bool enemyIsAlive = true;

	[Header("Should this enemy be accounted for Scene Clearing?")]
	public bool ignoreEnemyForSceneClearing = false;
	
	[Header("Toggle cannotDropWeapons for enemies that don't carry weapons:")]
	public bool cannotDropWeapons;

	[Header("Supply the list of available damage types:")]
	public List<string> damageType = new List<string>();
	[Header("Supply the corresponding list of blood effects")]
	public List<GameObject> bloodEffect = new List<GameObject> ();
	[Header("Supply the corresponding list of death animations:")]
	public List<Rigidbody> deadEnemy = new List<Rigidbody>();

	[Header("If provided, this GameObject will spawn on death of enemy at the percentage chance specified")]
	public List<GameObject> spawnOnDeath = new List<GameObject>();
	public List<float> percentSpawnOnDeath = new List<float>();

	[Header("If provided, target GameObject for OnDeath Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;

	[Header("Should the dead body spawn at a random rotation?")]
	public bool deadBodyRandomRotation;

	[HideInInspector] public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);
	[HideInInspector] public Vector3 damageForce = Vector3.zero;
	[HideInInspector] public float explosionForce = 0.0f;
	[HideInInspector] public Vector3 explosionLocation = Vector3.zero;

	private int damageTypeIndex;
	private Rigidbody dropWeapon;
	private Rigidbody deadBody;
	private Vector3 dropPoint;
	private NonPlayerCharacterWeaponChange npcWeaponChange;
	private UsableWeapons usableWeapons;
	private Quaternion deadBodyRotation;
	private SceneFadeInOut sceneFadeInOut;
	private LocalScenePersistentGameObjects localScenePersistentGameObjects;
	private bool localSceneObjectsExist = false;

	void Start(){
		sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<SceneFadeInOut>();		
		if(GameObject.FindGameObjectWithTag(Tags.localScene)){
			localScenePersistentGameObjects = GameObject.FindGameObjectWithTag(Tags.localScene).GetComponent<LocalScenePersistentGameObjects>();
			localSceneObjectsExist = true;
		}

		if(localSceneObjectsExist){
			if(!ignoreEnemyForSceneClearing && gameObject.tag == Tags.enemy && !localScenePersistentGameObjects.isCleared){
				//If this gameObject is tagged "enemy", then add it to the enemy list
				StartCoroutine(DelayedListAdd());
			}
		}
	}

	void Update(){
		//If the enemy health is <= 0, destroy the enemy
		if(enemyHealth <= 0.0f){
			if(enemyIsAlive){
				if(!cannotDropWeapons){
					npcWeaponChange = gameObject.GetComponent<NonPlayerCharacterWeaponChange>();

					dropPoint = npcWeaponChange.dropPoint.position;

					//Test to see if the dropPoint is being blocked by an obstacle...
					RaycastHit wallHit;
					if(Physics.Raycast (transform.position, (dropPoint - transform.position), out wallHit, (dropPoint - transform.position).magnitude, 1 << LayerMask.NameToLayer("Environment_Collision"))){
						dropPoint = transform.position+(wallHit.point-transform.position)*0.8f;
					}


					usableWeapons = gameObject.GetComponent<UsableWeapons>();
					if(npcWeaponChange.hasPrimaryWeap == true){
						string equippedWeaponName = npcWeaponChange.primaryEquipped.gameObject.GetComponent<weaponIndex> ().weaponName;
						int equippedWeaponIndex = usableWeapons.primaryUsableWeapons.IndexOf (equippedWeaponName);
						dropWeapon = Instantiate(usableWeapons.worldPrimaryUsableWeapons [equippedWeaponIndex], dropPoint, npcWeaponChange.primarySpawnpoint.rotation) as Rigidbody;
						dropWeapon.AddForce(GetComponent<NavMeshAgent>().velocity*npcWeaponChange.dropForce);
						dropWeapon.AddTorque (transform.up*npcWeaponChange.dropForce*Random.Range(-0.5f, 0.5f));
						dropWeapon.GetComponent<weaponIndex>().ammoCount = npcWeaponChange.primaryEquipped.gameObject.GetComponent<weaponIndex> ().ammoCount;
					}
					if(npcWeaponChange.hasSecondaryWeap == true){
						string equippedWeaponName = npcWeaponChange.secondaryEquipped.gameObject.GetComponent<weaponIndex> ().weaponName;
						int equippedWeaponIndex = usableWeapons.secondaryUsableWeapons.IndexOf (equippedWeaponName);
						dropWeapon = Instantiate(usableWeapons.worldSecondaryUsableWeapons [equippedWeaponIndex], dropPoint, npcWeaponChange.secondarySpawnpoint.rotation) as Rigidbody;
						dropWeapon.AddForce(GetComponent<NavMeshAgent>().velocity*npcWeaponChange.dropForce);
						dropWeapon.AddTorque (transform.up*npcWeaponChange.dropForce*Random.Range(-0.5f, 0.5f));
						dropWeapon.GetComponent<weaponIndex>().ammoCount = npcWeaponChange.secondaryEquipped.gameObject.GetComponent<weaponIndex> ().ammoCount;
					}
				}
				if(deadBodyRandomRotation){	
					deadBodyRotation = Quaternion.AngleAxis (Random.Range (0f, 360f),Vector3.up);
				}
				else{
					deadBodyRotation = transform.rotation;
				}
				deadBody = Instantiate (deadEnemy[damageTypeIndex], transform.position, deadBodyRotation ) as Rigidbody;

				deadBody.transform.Rotate (Vector3.right, 90f);
				if(damageForce!= Vector3.zero){
					deadBody.AddForce (damageForce);
				}
				if(explosionForce!= 0.0f){
					//deadBody.AddExplosionForce (explosionForce,explosionLocation, explosionRadius);
					deadBody.AddForce (explosionForce*((transform.position - explosionLocation).normalized));
				}

				//Pick a random spawnOnDeath from the list
				if(spawnOnDeath.Count > 0){
					int spawnIndex = 0;
					float randomNumber = Random.value*100.0f;
					int spawnOptionCount = spawnOnDeath.Count;
					float minCheck = 0.0f;
					float maxCheck = 0.0f;
					if(randomNumber == 0.0f){
						spawnIndex = 0;
					}
					else if(randomNumber == 1.0f){
						spawnIndex = spawnOnDeath.Count - 1;
					}
					else{
						for (int i=0; i < spawnOptionCount; i++){
							maxCheck = maxCheck + percentSpawnOnDeath[i];
							if (randomNumber >= minCheck && randomNumber < maxCheck){
								spawnIndex = i;
							}
							minCheck = maxCheck;
						}
					}
					Instantiate(spawnOnDeath[spawnIndex], transform.position, spawnOnDeath[spawnIndex].transform.rotation);
				}
				if(targetGO != null){
					(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
				}
			}
			enemyIsAlive = false;
			if(localSceneObjectsExist){
				if(!ignoreEnemyForSceneClearing && gameObject.tag == Tags.enemy && !localScenePersistentGameObjects.isCleared){
					sceneFadeInOut.remainingEnemiesList.Remove(gameObject);
					sceneFadeInOut.SceneStatusCheckDelay(true);
				}
			}
			Destroy(gameObject);

		}
	}

	public void BloodEffects(string damType, Vector3 damPosition, Quaternion damRotation){
		damageTypeIndex = damageType.IndexOf(damType);

		Instantiate (bloodEffect [damageTypeIndex], damPosition, damRotation);

	}

	IEnumerator DelayedListAdd(){
		yield return new WaitForSeconds(0.1f);
		sceneFadeInOut.remainingEnemiesList.Add(gameObject);
		sceneFadeInOut.enemiesComplete = false;
	}



}
