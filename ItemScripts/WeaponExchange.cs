using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponExchange : MonoBehaviour {
	//This script controls the PLAYER weapon exchange...


	public Transform originStart;
	public bool hasPrimaryWeap = false;
	public bool hasSecondaryWeap = false;
	public float dropForce = 10.0f;
	public Transform primarySpawnpoint;
	public Transform secondarySpawnpoint;
	public Transform dropPoint;
	public string onStartWeaponName;
	//public bool randomPrimaryWeaponAtStart;
	public Transform primaryEquipped, secondaryEquipped;

	private Dictionary<string, Rigidbody> worldPrimaryWeaponDictionary = new Dictionary<string, Rigidbody>(); 
	private Dictionary<string, Rigidbody> worldSecondaryWeaponDictionary = new Dictionary<string, Rigidbody> ();
	private Dictionary<string, Transform> equippedPrimaryWeaponDictionary = new Dictionary<string, Transform> ();
	private Dictionary<string, Transform> equippedSecondaryWeaponDictionary = new Dictionary<string, Transform> ();
	private bool interactable = false;
	private GameObject Pickup;

	private string weaponPickup;



	void Start(){ 
		if (GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WeaponTypes> ().equippedSecondaryWeaponDictionary == null){
			//... delay this script.
			StartCoroutine(DelayStart());
		}
		//... otherwise, continue.
		else {
			worldPrimaryWeaponDictionary = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WeaponTypes> ().worldPrimaryWeaponDictionary; 
			equippedPrimaryWeaponDictionary = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WeaponTypes> ().equippedPrimaryWeaponDictionary;
			worldSecondaryWeaponDictionary = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WeaponTypes> ().worldSecondaryWeaponDictionary; 
			equippedSecondaryWeaponDictionary = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WeaponTypes> ().equippedSecondaryWeaponDictionary;
		}
		if(onStartWeaponName != null){
			Transform pickupob = Instantiate (equippedPrimaryWeaponDictionary[onStartWeaponName], primarySpawnpoint.position, primarySpawnpoint.rotation) as Transform;
			pickupob.parent = transform;
			primaryEquipped = pickupob;
			hasPrimaryWeap = true;
		}
		// If the gameObject needs a random primary weapon at start...
		/*if (randomPrimaryWeaponAtStart){ //Comment out for player
			//Pick a random weapon from the primary weapon list
			string randomWeaponString = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WeaponTypes> ().primaryWeaponKey[Random.Range(0,equippedPrimaryWeaponDictionary.Count)];
			//Instantiate random weapon and prepare enemy properties
			Transform pickupob = Instantiate (equippedPrimaryWeaponDictionary[randomWeaponString], primarySpawnpoint.position, primarySpawnpoint.rotation) as Transform;
			pickupob.parent = transform;
			primaryEquipped = pickupob;
			hasPrimaryWeap = true;
			//pickupob.GetComponent<ShootBullet>().enabled = false; //Comment out to allow enemies to shoot
			pickupob.parent.GetComponentInChildren<EnemySight>().equippedWeaponAttackRange = pickupob.GetComponent<weaponIndex>().weaponRange;
			pickupob.parent.GetComponent<NavMeshAgent>().stoppingDistance = pickupob.GetComponent<weaponIndex>().weaponRange;
		}*/
	}

	void Update () {

			if (Input.GetButtonDown ("Interact")) {

				//Get cursor position and player position to find direction of Raycast...
				Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
				cursorPosition.y = 0f;
				Vector3 direction = new Vector3 (cursorPosition.x - transform.position.x, cursorPosition.y - transform.position.y, cursorPosition.z-transform.position.z);
				direction.y = 0f;
				direction = direction.normalized;
				
				//Cast ray to see if a pickup is in range...
				RaycastHit hit;
				Physics.Raycast (originStart.position, direction, out hit, 1.5f, 1 << LayerMask.NameToLayer("Pickup"));
				Debug.DrawRay(originStart.position, direction*1.5f, Color.green);
				if (hit.collider != null) {
					RaycastHit blockage; // Check for environment blockage (walls)
					if(Physics.Raycast (originStart.position, direction, out blockage, 1.5f, 1 << LayerMask.NameToLayer("Environment_Collision"))){
						if(blockage.distance > hit.distance){
							interactable = true;
						}
						else{
							interactable = false;
						}
					}
					else {
						interactable = true;
					}
				}
				else{
					interactable = false;
				}

				if(interactable){
					Pickup = hit.collider.gameObject;
					weaponPickup = hit.collider.gameObject.GetComponent<weaponIndex> ().weaponName;

					if (hit.collider.gameObject.GetComponent<weaponIndex> ().primaryWeapon == true) { //Test to see if the raycast weapon is a primary weapon

						Rigidbody dropob = Instantiate (worldPrimaryWeaponDictionary [primaryEquipped.gameObject.GetComponent<weaponIndex> ().weaponName], dropPoint.position, primarySpawnpoint.rotation) as Rigidbody;
						dropob.AddForce (transform.up * dropForce);
						dropob.GetComponent<weaponIndex>().ammoCount = primaryEquipped.GetComponent<weaponIndex>().ammoCount;
						Destroy (primaryEquipped.gameObject);
						Transform pickupob = Instantiate (equippedPrimaryWeaponDictionary [weaponPickup], primarySpawnpoint.position, primarySpawnpoint.rotation) as Transform;
						pickupob.parent = transform;
						pickupob.GetComponent<weaponIndex>().ammoCount = hit.collider.gameObject.GetComponent<weaponIndex>().ammoCount;
						if(pickupob.GetComponent<weaponIndex>().ammoCount == 0){
							pickupob.GetComponentInChildren<Animator>().SetBool ("noAmmoBool", true);
						}
						Destroy (Pickup);
						primaryEquipped = pickupob;

					} // End Primary Weapon pick up

					else{// Direct logic to pick up a Secondary Weapon

						if (hasSecondaryWeap == false) {
							Transform pickupob = Instantiate (equippedSecondaryWeaponDictionary [weaponPickup], secondarySpawnpoint.position, secondarySpawnpoint.rotation) as Transform;
							pickupob.parent = transform;
							pickupob.GetComponent<weaponIndex>().ammoCount = hit.collider.gameObject.GetComponent<weaponIndex>().ammoCount;
							Destroy (Pickup);
							secondaryEquipped = pickupob;
							hasSecondaryWeap = true;
						} else {
							Rigidbody dropob = Instantiate (worldSecondaryWeaponDictionary [secondaryEquipped.gameObject.GetComponent<weaponIndex> ().weaponName], dropPoint.position, secondarySpawnpoint.rotation) as Rigidbody;
							dropob.AddForce (transform.up * dropForce);
							dropob.GetComponent<weaponIndex>().ammoCount = secondaryEquipped.GetComponent<weaponIndex>().ammoCount;
							Destroy (secondaryEquipped.gameObject);
							Transform pickupob = Instantiate (equippedSecondaryWeaponDictionary [weaponPickup], secondarySpawnpoint.position, secondarySpawnpoint.rotation) as Transform;
							pickupob.parent = transform;
							pickupob.GetComponent<weaponIndex>().ammoCount = hit.collider.gameObject.GetComponent<weaponIndex>().ammoCount;
							Destroy (Pickup);
							secondaryEquipped = pickupob;
						}
					}// End Secondary Weapon pick up
				}
			}
			/* Comment out ability to drop primary weapon...
			if (Input.GetButtonDown ("Interact") && interactable == false && hasPrimaryWeap == true) {
					Rigidbody dropob = Instantiate (worldWeaponList [primaryEquipped.gameObject.GetComponent<weaponIndex> ().weaponNum], dropPoint.position, primarySpawnpoint.rotation) as Rigidbody;
					dropob.AddForce (transform.up * dropForce);
					Destroy (primaryEquipped.gameObject);
					hasPrimaryWeap = false;
					primaryEquipped = null;
			}
			*/
			
			//Commented out ability to drop Secondary Weapon --- need to edit code for different KeyCode.###
			/*if (Input.GetButtonDown ("Fire2") && interactable == false && hasSecondaryWeap == true) {
					Rigidbody2D dropob = Instantiate (worldWeaponList [secondaryEquipped.gameObject.GetComponent<weaponIndex> ().weaponNum], secondarySpawnpoint.position, secondarySpawnpoint.rotation) as Rigidbody2D;
					dropob.AddForce (transform.up * dropForce);
					Destroy (secondaryEquipped.gameObject);
					hasSecondaryWeap = false;
					secondaryEquipped = null;
			}*/  

	}//End Update()

	IEnumerator DelayStart(){
		//Debug.Log ("waiting...");
		yield return new WaitForSeconds(0.1f);
	}
}
