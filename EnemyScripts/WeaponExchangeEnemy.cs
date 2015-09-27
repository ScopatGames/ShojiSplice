using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponExchangeEnemy : MonoBehaviour {
	//THIS SCRIPT IS OBSOLETE! reference only

	//This script controls the ENEMY weapon exchange...
	
	
	public Transform originStart;
	public bool hasPrimaryWeap = false;
	public bool hasSecondaryWeap = false;
	public float dropForce = 10.0f;
	public Transform primarySpawnpoint;
	public Transform secondarySpawnpoint;
	public Transform dropPoint;
	public string onStartWeaponName;
	public bool randomPrimaryWeaponAtStart;
	public Transform primaryEquipped, secondaryEquipped;

	private Vector3 clearDropPoint;
	
	//private Dictionary<string, Rigidbody> worldPrimaryWeaponDictionary = new Dictionary<string, Rigidbody>(); 
	//private Dictionary<string, Rigidbody> worldSecondaryWeaponDictionary = new Dictionary<string, Rigidbody> ();
	private Dictionary<string, Transform> enemyPrimaryWeaponDictionary = new Dictionary<string, Transform> ();
	//private Dictionary<string, Transform> equippedSecondaryWeaponDictionary = new Dictionary<string, Transform> ();
	//private bool interactable = false;
	private GameObject Pickup;
	
	private string weaponPickup;
	
	
	
	void Start(){ 
		if (GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WeaponTypes> ().equippedSecondaryWeaponDictionary == null){
			//... delay this script.
			StartCoroutine(DelayStart());
		}
		//... otherwise, continue.
		else {
			//worldPrimaryWeaponDictionary = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WeaponTypes> ().worldPrimaryWeaponDictionary; 
			enemyPrimaryWeaponDictionary = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WeaponTypes> ().enemyPrimaryWeaponDictionary;
			//worldSecondaryWeaponDictionary = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WeaponTypes> ().worldSecondaryWeaponDictionary; 
			//equippedSecondaryWeaponDictionary = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WeaponTypes> ().equippedSecondaryWeaponDictionary;
		}

		if(onStartWeaponName != ""){
			Transform pickupob = Instantiate (enemyPrimaryWeaponDictionary[onStartWeaponName], primarySpawnpoint.position, primarySpawnpoint.rotation) as Transform;
			pickupob.parent = transform;
			primaryEquipped = pickupob;
			hasPrimaryWeap = true;
			//pickupob.GetComponent<ShootBullet>().enabled = false; //Use this line while testing to out to prevent enemies from to shooting.
			pickupob.parent.GetComponentInChildren<EnemySight>().equippedWeaponAttackRange = pickupob.GetComponent<weaponIndex>().weaponRange;
			pickupob.parent.GetComponent<NavMeshAgent>().stoppingDistance = pickupob.GetComponent<weaponIndex>().weaponRange;
		}
		// If the gameObject needs a random primary weapon at start...
		if (randomPrimaryWeaponAtStart){ 
			//Pick a random weapon from the primary weapon list
			string randomWeaponString = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WeaponTypes> ().primaryWeaponKey[Random.Range(1,3)]; //enemyPrimaryWeaponDictionary.Count)];
			//Instantiate random weapon and prepare enemy properties
			Transform pickupob = Instantiate (enemyPrimaryWeaponDictionary[randomWeaponString], primarySpawnpoint.position, primarySpawnpoint.rotation) as Transform;
			pickupob.parent = transform;
			primaryEquipped = pickupob;
			hasPrimaryWeap = true;
			//pickupob.GetComponent<ShootBullet>().enabled = false; //Use this line while testing to out to prevent enemies from to shooting.
			pickupob.parent.GetComponentInChildren<EnemySight>().equippedWeaponAttackRange = pickupob.GetComponent<weaponIndex>().weaponRange;
			pickupob.parent.GetComponent<NavMeshAgent>().stoppingDistance = pickupob.GetComponent<weaponIndex>().weaponRange;
		}
	}
	

	
	IEnumerator DelayStart(){
		//Debug.Log ("waiting...");
		yield return new WaitForSeconds(0.1f);
	}


}


