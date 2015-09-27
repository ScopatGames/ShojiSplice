using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerWeaponChange : MonoBehaviour {
	
	//This script controls the PLAYER weapon exchange...
	
	
	public Transform rayStart;
	
	public bool hasPrimaryWeap = false;
	public bool hasSecondaryWeap = false;
	public float dropForce = 10.0f;
	public Transform primarySpawnpoint;
	public Transform secondarySpawnpoint;
	public Transform dropPoint;
	public string onStartWeaponName = "unarmed";
	public Transform primaryEquipped, secondaryEquipped;
	public LayerMask dropLayerMask; 
	
	private Vector3 clearDropPoint;
	private GameObject gameController;
	private bool interactable = false;
	private GameObject Pickup;
	private string weaponPickupName;
	private UsableWeapons usableWeapons;
	private int primaryWeaponIndex;
	private int secondaryWeaponIndex;
	private PlayerInfoPanel playerInfoPanel;
	private PersistentData persistentData;
	
	
	
	void Start(){ 
		
		
		//Define UsableWeapons link:
		gameController = GameObject.FindGameObjectWithTag (Tags.gameController);
		usableWeapons = gameController.GetComponent<UsableWeapons> ();
		playerInfoPanel = GameObject.FindGameObjectWithTag(Tags.canvas).GetComponentInChildren<PlayerInfoPanel> ();
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
		
		
		
		if(persistentData.primaryWeapon != ""){
			EquipPrimaryWeapon(persistentData.primaryWeapon, persistentData.primaryWeaponAmmoCount, false);
		}
		else{
			if(onStartWeaponName != ""){
				//Determine index of onStartWeaponName
				primaryWeaponIndex = usableWeapons.primaryUsableWeapons.IndexOf(onStartWeaponName);
				Transform pickupob = Instantiate (usableWeapons.equippedPrimaryUsableWeapons[primaryWeaponIndex], primarySpawnpoint.position, primarySpawnpoint.rotation) as Transform;
				pickupob.parent = transform;
				primaryEquipped = pickupob;
				hasPrimaryWeap = true;
				playerInfoPanel.UpdatePrimaryWeaponInfo();
				
			}
			else{
				Debug.Log ("ERROR: Need to set onStartWeaponName.");
			}
		}
		if(persistentData.secondaryWeapon != ""){
			EquipSecondaryWeapon(persistentData.secondaryWeapon, persistentData.secondaryWeaponAmmoCount, false);
		}
	}
	
	void Update () {
		if (Input.GetButtonDown ("Pickup") && !Input.GetButton ("Shift")) {
			
			//Get cursor position and player position to find direction of Raycast...
			Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
			Vector3 direction = new Vector3 (cursorPosition.x - transform.position.x, 0.0f, cursorPosition.z-transform.position.z);
			
			if(direction.magnitude > 1.0f){
				direction = Vector3.Normalize(direction);
			}
			direction.y = -0.5f;
			
			//Cast ray to see if a pickup is in range...
			RaycastHit hit;
			Physics.Raycast (rayStart.position, direction, out hit, 1.5f, 1 << LayerMask.NameToLayer("Pickup"));
			Debug.DrawRay(rayStart.position, direction*1.5f, Color.green);
			if (hit.collider != null && hit.collider.GetComponent<weaponIndex>().weaponName != "dead") {
				RaycastHit blockage; // Check for environment blockage (walls)
				if(Physics.Raycast (rayStart.position, direction, out blockage, 1.5f, dropLayerMask)){
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
				weaponPickupName = Pickup.GetComponent<weaponIndex> ().weaponName;
				
				//Test to see if the dropPoint is being blocked by an obstacle...
				RaycastHit wallHit;
				if(Physics.Raycast (transform.position, (dropPoint.position - transform.position), out wallHit, (dropPoint.position - transform.position).magnitude, dropLayerMask)){
					clearDropPoint = transform.position+(wallHit.point-transform.position)*0.8f;
				}
				else{
					clearDropPoint = dropPoint.position;
				}
				
				if (hit.collider.gameObject.GetComponent<weaponIndex> ().primaryWeapon == true) { //Test to see if the raycast weapon is a primary weapon
					
					//Instantiate dropped primary weapon
					Rigidbody dropob = Instantiate (usableWeapons.worldPrimaryUsableWeapons[primaryWeaponIndex], clearDropPoint, primarySpawnpoint.rotation) as Rigidbody;
					
					//Set properties of dropped primary weapon
					dropob.AddForce (transform.up * dropForce);
					dropob.AddTorque (transform.up*dropForce*Random.Range(-0.5f, 0.5f));
					dropob.GetComponent<weaponIndex>().ammoCount = primaryEquipped.GetComponent<weaponIndex>().ammoCount;
					
					//Destroy old equipped instance
					Destroy (primaryEquipped.gameObject);
					
					//Determine weapon index of pickup
					primaryWeaponIndex = usableWeapons.primaryUsableWeapons.IndexOf (weaponPickupName);
					
					//Instantiate equipped primary weapon
					Transform pickupob = Instantiate (usableWeapons.equippedPrimaryUsableWeapons [primaryWeaponIndex], primarySpawnpoint.position, primarySpawnpoint.rotation) as Transform;
					
					//Set properties of picked up weapon
					pickupob.parent = transform;
					pickupob.GetComponent<weaponIndex>().ammoCount = hit.collider.gameObject.GetComponent<weaponIndex>().ammoCount;
					if(pickupob.GetComponent<weaponIndex>().ammoCount == 0){
						pickupob.GetComponentInChildren<Animator>().SetBool ("noAmmoBool", true);
					}
					else{
						playerInfoPanel.DisplayNewWeaponInfo();
					}
					primaryEquipped = pickupob;
					playerInfoPanel.UpdatePrimaryWeaponInfo();
					
					//Destroy old world instance
					Destroy (Pickup);
					
					
					
				} // End Primary Weapon pick up
				
				else{// Direct logic to pick up a Secondary Weapon
					
					if (hasSecondaryWeap == false) {
						
						//Determine weapon index of pickup
						secondaryWeaponIndex = usableWeapons.secondaryUsableWeapons.IndexOf (weaponPickupName);
						
						//Instantiate equipped secondary weapon
						Transform pickupob = Instantiate (usableWeapons.equippedSecondaryUsableWeapons[secondaryWeaponIndex], secondarySpawnpoint.position, secondarySpawnpoint.rotation) as Transform;
						
						//Set properties of picked up weapon
						pickupob.parent = transform;
						pickupob.GetComponent<weaponIndex>().ammoCount = hit.collider.gameObject.GetComponent<weaponIndex>().ammoCount;
						secondaryEquipped = pickupob;
						hasSecondaryWeap = true;
						
						
						//Destroy old world instance
						Destroy (Pickup);
						
					} else {
						
						//Instantiate dropped weapon instance
						Rigidbody dropob = Instantiate (usableWeapons.worldSecondaryUsableWeapons[secondaryWeaponIndex], clearDropPoint, secondarySpawnpoint.rotation) as Rigidbody;
						
						//Set dropped weapon properties
						dropob.AddForce (transform.up * dropForce);
						dropob.AddTorque (transform.up*dropForce*Random.Range(-0.5f, 0.5f));
						dropob.GetComponent<weaponIndex>().ammoCount = secondaryEquipped.GetComponent<weaponIndex>().ammoCount;
						
						//Destroy old equipped weapon instance
						Destroy (secondaryEquipped.gameObject);
						
						//Determine weapon index of pickup
						secondaryWeaponIndex = usableWeapons.secondaryUsableWeapons.IndexOf (weaponPickupName);
						
						//Instantiate equipped secondary weapon
						Transform pickupob = Instantiate (usableWeapons.equippedSecondaryUsableWeapons[secondaryWeaponIndex], secondarySpawnpoint.position, secondarySpawnpoint.rotation) as Transform;
						
						//Set properties of picked up weapon
						pickupob.parent = transform;
						pickupob.GetComponent<weaponIndex>().ammoCount = hit.collider.gameObject.GetComponent<weaponIndex>().ammoCount;
						secondaryEquipped = pickupob;
						
						//Destroy old world instance
						Destroy (Pickup);
						
					}
				}// End Secondary Weapon pick up
			}
		}
		
		
		if(Input.GetButton ("Shift") && Input.GetButtonDown ("Pickup")){
			DropPrimaryWeapon();
		}
		
		
	}//End Update()
	
	public void DropPrimaryWeapon(){
		//Test to see if the dropPoint is being blocked by an obstacle...
		RaycastHit wallHit;
		if(Physics.Raycast (transform.position, (dropPoint.position - transform.position), out wallHit, (dropPoint.position - transform.position).magnitude, dropLayerMask)){
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
		
		//Destroy old equipped instance
		Destroy (primaryEquipped.gameObject);
		
		//Determine weapon index of pickup
		primaryWeaponIndex = usableWeapons.primaryUsableWeapons.IndexOf ("unarmed");
		
		//Instantiate equipped primary weapon
		Transform pickupob = Instantiate (usableWeapons.equippedPrimaryUsableWeapons [primaryWeaponIndex], primarySpawnpoint.position, primarySpawnpoint.rotation) as Transform;
		
		//Set properties of picked up weapon
		pickupob.parent = transform;
		primaryEquipped = pickupob;
		playerInfoPanel.UpdatePrimaryWeaponInfo();
	}
	
	
	public void EquipPrimaryWeapon(string weaponPickupName, int ammoCount, bool dropCurrentWeapon){
		//This script equips a weapon with or without (dropCurrentWeapon bool) dropping the held weapon...
		
		if(dropCurrentWeapon && hasPrimaryWeap){
			//Test to see if the dropPoint is being blocked by an obstacle...
			RaycastHit wallHit;
			if(Physics.Raycast (transform.position, (dropPoint.position - transform.position), out wallHit, (dropPoint.position - transform.position).magnitude, dropLayerMask)){
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
			pickupob.GetComponentInChildren<Animator>().SetBool ("noAmmoBool", true);
		}
		else{
			playerInfoPanel.DisplayNewWeaponInfo();
		}
		primaryEquipped = pickupob;
		playerInfoPanel.UpdatePrimaryWeaponInfo();
		
	}
	
	public void EquipSecondaryWeapon(string weaponPickupName, int ammoCount, bool dropCurrentWeapon){
		
		if(dropCurrentWeapon && hasSecondaryWeap){
			//Test to see if the dropPoint is being blocked by an obstacle...
			RaycastHit wallHit;
			if(Physics.Raycast (transform.position, (dropPoint.position - transform.position), out wallHit, (dropPoint.position - transform.position).magnitude, dropLayerMask)){
				clearDropPoint = transform.position+(wallHit.point-transform.position)*0.8f;
			}
			else{
				clearDropPoint = dropPoint.position;
			}
			//Instantiate dropped weapon instance
			Rigidbody dropob = Instantiate (usableWeapons.worldSecondaryUsableWeapons[secondaryWeaponIndex], clearDropPoint, secondarySpawnpoint.rotation) as Rigidbody;
			
			//Set dropped weapon properties
			dropob.AddForce (transform.up * dropForce);
			dropob.AddTorque (transform.up*dropForce*Random.Range(-0.5f, 0.5f));
			dropob.GetComponent<weaponIndex>().ammoCount = secondaryEquipped.GetComponent<weaponIndex>().ammoCount;
		}
		
		if(hasSecondaryWeap){
			//Destroy old equipped weapon instance
			Destroy (secondaryEquipped.gameObject);
		}
		
		if(weaponPickupName != ""){
			//Determine weapon index of pickup
			secondaryWeaponIndex = usableWeapons.secondaryUsableWeapons.IndexOf (weaponPickupName);
			
			//Instantiate equipped secondary weapon
			Transform pickupob = Instantiate (usableWeapons.equippedSecondaryUsableWeapons[secondaryWeaponIndex], secondarySpawnpoint.position, secondarySpawnpoint.rotation) as Transform;
			
			//Set properties of picked up weapon
			pickupob.parent = transform;
			pickupob.GetComponent<weaponIndex>().ammoCount = ammoCount;
			secondaryEquipped = pickupob;
			hasSecondaryWeap = true;
		}
		else{
			secondaryEquipped = null;
			hasSecondaryWeap = false;
		}
	}
}
