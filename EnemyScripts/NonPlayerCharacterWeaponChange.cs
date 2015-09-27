using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NonPlayerCharacterWeaponChange : MonoBehaviour {

	//This script controls the Non Player Character weapon exchange...
	
	
	public Transform originStart;
	public bool hasPrimaryWeap = false;
	public bool hasSecondaryWeap = false;
	public float dropForce = 10.0f;
	public Transform primarySpawnpoint;
	public Transform secondarySpawnpoint;
	public Transform dropPoint;
	public string onStartWeaponName;
	public Transform primaryEquipped, secondaryEquipped;
	
	private Vector3 clearDropPoint;
	private GameObject Pickup;
	private string weaponPickupName;
	private UsableWeapons usableWeapons;
	private int primaryWeaponIndex;
	//private int secondaryWeaponIndex;
	
	
	
	void Awake(){ 
		usableWeapons = gameObject.GetComponent<UsableWeapons> ();

		if(onStartWeaponName != ""){
			primaryWeaponIndex = usableWeapons.primaryUsableWeapons.IndexOf(onStartWeaponName);
			Transform pickupob = Instantiate (usableWeapons.equippedPrimaryUsableWeapons[primaryWeaponIndex], primarySpawnpoint.position, primarySpawnpoint.rotation) as Transform;
			pickupob.parent = transform;
			primaryEquipped = pickupob;
			hasPrimaryWeap = true;
			//pickupob.GetComponent<ShootBullet>().enabled = false; //Use this line while testing to out to prevent enemies from to shooting.
			pickupob.parent.GetComponentInChildren<EnemySight>().equippedWeaponAttackRange = pickupob.GetComponent<weaponIndex>().weaponRange;

		}

		// Else the gameObject needs a random primary weapon at start...
		else{ 
			//Pick a random weapon from the primary weapon list
			float randomNumber = Random.value*100.0f;
			int usableWeaponCount = usableWeapons.primaryUsableWeapons.Count;
			float minCheck = 0.0f;
			float maxCheck = 0.0f;
			if(randomNumber == 0.0f){
				primaryWeaponIndex = 0;
			}
			else if(randomNumber == 1.0f){
				primaryWeaponIndex = usableWeapons.primaryUsableWeapons.Count - 1;
			}
			else{
				for (int i=0; i < usableWeaponCount; i++){
					maxCheck = maxCheck + usableWeapons.percentChanceWeaponSpawn[i];
					if (randomNumber >= minCheck && randomNumber < maxCheck){
						primaryWeaponIndex = i;
					}
					minCheck = maxCheck;
				}
			}

			//Instantiate random weapon and prepare enemy properties
			Transform pickupob = Instantiate (usableWeapons.equippedPrimaryUsableWeapons[primaryWeaponIndex], primarySpawnpoint.position, primarySpawnpoint.rotation) as Transform;
			pickupob.parent = transform;
			primaryEquipped = pickupob;
			hasPrimaryWeap = true;
			//pickupob.GetComponent<ShootBullet>().enabled = false; //Use this line while testing to out to prevent enemies from to shooting.
			pickupob.parent.GetComponentInChildren<EnemySight>().equippedWeaponAttackRange = pickupob.GetComponent<weaponIndex>().weaponRange;

		}
	}

	public void EquipPrimaryWeapon(string weaponPickupName, int ammoCount, bool dropCurrentWeapon){
		//This script equips a weapon with or without (discardCurrentWeapon bool) dropping the held weapon...
		
		if(dropCurrentWeapon && hasPrimaryWeap){
			//Test to see if the dropPoint is being blocked by an obstacle...
			RaycastHit wallHit;
			if(Physics.Raycast (transform.position, (dropPoint.position - transform.position), out wallHit, (dropPoint.position - transform.position).magnitude, 1 << LayerMask.NameToLayer("Environment_Collision"))){
				clearDropPoint = transform.position+(wallHit.point-transform.position)*0.8f;
			}
			else{
				clearDropPoint = dropPoint.position;
			}
			
			//Instantiate dropped primary weapon
			Rigidbody dropob = Instantiate (usableWeapons.worldPrimaryUsableWeapons[primaryWeaponIndex], clearDropPoint, primarySpawnpoint.rotation) as Rigidbody;
			
			//Set properties of dropped primary weapon
			dropob.AddForce (transform.up * dropForce);
			dropob.AddTorque (transform.up*dropForce*Random.Range(-0.5f, 0.5f));
			dropob.GetComponent<weaponIndex>().ammoCount = primaryEquipped.GetComponent<weaponIndex>().ammoCount;
			
		}
		
		
		if(hasPrimaryWeap){
			//Destroy old equipped instance
			Destroy (primaryEquipped.gameObject);
		}
		
		//Determine weapon index of pickup
		primaryWeaponIndex = usableWeapons.primaryUsableWeapons.IndexOf (weaponPickupName);
		//Instantiate equipped primary weapon
		Transform pickupob = Instantiate (usableWeapons.equippedPrimaryUsableWeapons [primaryWeaponIndex], primarySpawnpoint.position, primarySpawnpoint.rotation) as Transform;
		
		//Set properties of picked up weapon
		pickupob.parent = transform;
		pickupob.GetComponent<weaponIndex>().ammoCount = ammoCount;
		if(pickupob.GetComponent<weaponIndex>().ammoCount == 0){
			Animator a = pickupob.GetComponentInChildren<Animator>();
			if(a){
				AnimatorControllerParameter[] acp = a.parameters;
				foreach(AnimatorControllerParameter b in acp){
					if(b.name == "noAmmoBool"){
						a.SetBool ("noAmmoBool", true);
					}
				}
			}
		}
		primaryEquipped = pickupob;

		
	}

}
