using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour {

	public bool playerIsAlive = true;
	public bool playerHelmet = false;
	public Vector3 position = new Vector3(1000f, 1000f, 1000f);
	[HideInInspector] public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);
	public float playerHealth = 100.0f;
	[Header("Supply the list of available damage types:")]
	public List<string> damageType = new List<string>();
	[Header("Supply the corresponding list of blood effects")]
	public List<GameObject> bloodEffect = new List<GameObject> ();
	[Header("Supply the corresponding list of death animations:")]
	public List<Rigidbody> deadPlayer = new List<Rigidbody>();
	//public Rigidbody deadPlayer;
	private Rigidbody dropWeapon;
	private PlayerWeaponChange playerWeaponChange;
	private UsableWeapons usableWeapons;
	[HideInInspector] public Vector3 damageForce = Vector3.zero;
	private GameObject playerGO;
	private Vector3 dropPoint;
	private Rigidbody deadBody;
	[HideInInspector] public float explosionForce = 0.0f;
	[HideInInspector] public Vector3 explosionLocation = Vector3.zero;
	private int damageTypeIndex;

	private CenterMessageController centerMessageController;

	void Awake(){
		playerGO = GameObject.FindGameObjectWithTag(Tags.player);
		centerMessageController = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponentInChildren<CenterMessageController> ();
	}

	void Update(){
		//If the player health is <= 0, destroy the player
		if(playerHealth <= 0.0f){
			if(playerIsAlive){ //Test to see if the player health has just crossed below 0.0

				centerMessageController.UpdateCenterMessage("Press Tab to Restart");
				centerMessageController.FlashCenterMessageAfterSeconds(0.5f);

				playerWeaponChange = playerGO.GetComponent<PlayerWeaponChange>();

				dropPoint = playerWeaponChange.dropPoint.position;
				//Test to see if the dropPoint is being blocked by an obstacle...
				RaycastHit wallHit;
				if(Physics.Raycast (playerWeaponChange.transform.position, (dropPoint - playerWeaponChange.transform.position), out wallHit, (dropPoint - playerWeaponChange.transform.position).magnitude, 1 << LayerMask.NameToLayer("Environment_Collision"))){
					dropPoint = playerWeaponChange.transform.position+(wallHit.point-playerWeaponChange.transform.position)*0.8f;
				}

				usableWeapons = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<UsableWeapons>();

				if(playerWeaponChange.hasPrimaryWeap == true){
					string equippedWeaponName = playerWeaponChange.primaryEquipped.gameObject.GetComponent<weaponIndex> ().weaponName;
					int equippedWeaponIndex = usableWeapons.primaryUsableWeapons.IndexOf (equippedWeaponName);
					dropWeapon = Instantiate(usableWeapons.worldPrimaryUsableWeapons [equippedWeaponIndex], dropPoint, playerWeaponChange.primarySpawnpoint.rotation) as Rigidbody;
					dropWeapon.AddForce(playerGO.GetComponent<Rigidbody>().velocity*playerWeaponChange.dropForce);
					dropWeapon.AddTorque (transform.up*playerWeaponChange.dropForce*Random.Range(-0.5f, 0.5f));
					dropWeapon.GetComponent<weaponIndex>().ammoCount = playerWeaponChange.primaryEquipped.gameObject.GetComponent<weaponIndex> ().ammoCount;
				}
				if(playerWeaponChange.hasSecondaryWeap == true){
					string equippedWeaponName = playerWeaponChange.secondaryEquipped.gameObject.GetComponent<weaponIndex> ().weaponName;
					int equippedWeaponIndex = usableWeapons.secondaryUsableWeapons.IndexOf (equippedWeaponName);
					dropWeapon = Instantiate(usableWeapons.worldSecondaryUsableWeapons [equippedWeaponIndex], dropPoint, playerWeaponChange.secondarySpawnpoint.rotation) as Rigidbody;
					dropWeapon.AddForce(playerGO.GetComponent<Rigidbody>().velocity*playerWeaponChange.dropForce);
					dropWeapon.AddTorque (transform.up*playerWeaponChange.dropForce*Random.Range(-0.5f, 0.5f));
					dropWeapon.GetComponent<weaponIndex>().ammoCount = playerWeaponChange.secondaryEquipped.gameObject.GetComponent<weaponIndex> ().ammoCount;
				}
				deadBody = Instantiate (deadPlayer[damageTypeIndex], playerGO.transform.position, playerWeaponChange.primarySpawnpoint.rotation) as Rigidbody;

				//Check for helmet...
				if(deadBody.GetComponent<Animator>()){
					Animator[] anims = playerGO.GetComponentsInChildren<Animator> ();
					Animator deadBodyAnim = deadBody.GetComponent<Animator>();
					foreach (Animator a in anims) {
						AnimatorControllerParameter[] acps = a.parameters;
						foreach (AnimatorControllerParameter acp in acps){
							if(acp.name == "helmetEquipped"){
								bool currentValue = a.GetBool ("helmetEquipped");
								deadBodyAnim.SetBool ("helmetEquipped", currentValue);
								break;
							}
						}
					}
					AnimatorControllerParameter[] deadBodyAcp = deadBodyAnim.parameters;
					AnimatorControllerParameterType paramType = AnimatorControllerParameterType.Bool;
					int randomIndex = 0;
					while (paramType != AnimatorControllerParameterType.Trigger){
						randomIndex = Random.Range (0, deadBodyAcp.Length); //Random.Range is exclusive for the 2nd value
						paramType = deadBodyAcp[randomIndex].type;
					}
					deadBodyAnim.SetTrigger (deadBodyAcp [randomIndex].name);
				}
				//End check for helmet
			

				if(damageForce != Vector3.zero){
					deadBody.AddForce (damageForce);
				}
				if(explosionForce != 0.0f){
					//deadBody.AddExplosionForce(explosionForce, explosionLocation, explosionRadius);
					deadBody.AddForce (explosionForce*((playerGO.transform.position - explosionLocation).normalized));
				}
			}
			playerIsAlive = false;
			Destroy(playerGO);

		}


	}

	public void BloodEffects(string damType, Vector3 damPosition, Quaternion damRotation){
		damageTypeIndex = damageType.IndexOf(damType);
		Instantiate (bloodEffect [damageTypeIndex], damPosition, damRotation);
		
	}



}
