using UnityEngine;
using System.Collections;

public class FiringRangeDoor : MonoBehaviour {

	[Header("Tutorial messages:")]
	public string stringPickup;
	public string stringAttack;
	public string stringShiftAttack;
	public string stringInfo;
	public string stringShiftPickup;


	[Header("Other variables:")]
	public GameObject player;
	public LayerMask layerMask;

	public GameObject targetGO;
	public string scriptNameToEnable;


	public Collider[] hitColliders;
	public bool hasSecondary = false;
	public weaponIndex[] weapIndex;
	public bool noGrenades = false;
	public bool noAmmo = false;
	public bool pickupsDisabled = false;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	private CenterMessageController centerMessageController;
	public bool boolPickup;

	void OnEnable(){
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;
			centerMessageController = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponentInChildren<CenterMessageController> ();
			centerMessageController.UpdateCenterMessage (stringPickup);
			centerMessageController.OpenCenterMessage ();
	
		}
	}
	
	// This is listening to see if all the weapons have been destroyed...
	void Update () {
		if(player){	

			weapIndex = player.GetComponentsInChildren<weaponIndex> ();
			hasSecondary = player.GetComponent<PlayerWeaponChange> ().hasSecondaryWeap;

			hitColliders = Physics.OverlapSphere(transform.position, 3.1f, layerMask);

			foreach (weaponIndex wi in weapIndex){
				if(wi.weaponName == "unarmed" && hitColliders.Length == 2){
					noGrenades = true;
				}
				if(wi.weaponName == "laserPistol"){
					if(wi.ammoCount == 0){
						centerMessageController.UpdateCenterMessage(stringPickup + "\n" + stringAttack + "\n" + stringShiftAttack + "\n" + stringInfo + "\n" + stringShiftPickup);
						noAmmo = true;
					}
					else if(wi.ammoCount == 17){
						centerMessageController.UpdateCenterMessage(stringPickup + "\n" + stringAttack + "\n" + stringShiftAttack);
					}
					else if(wi.ammoCount == 10){
						centerMessageController.UpdateCenterMessage(stringPickup + "\n" + stringAttack + "\n" + stringShiftAttack + "\n" + stringInfo);
					}
				}

			
			
				if(noAmmo && wi.weaponName == "unarmed" && noGrenades && !pickupsDisabled){
					Rigidbody rb = hitColliders[0].transform.GetComponent<Rigidbody>() as Rigidbody;
					rb.isKinematic = true;
					//rb.velocity = Vector3.zero;

					hitColliders[0].enabled = false;
					hitColliders[1].enabled = false;

					pickupsDisabled = true;
				}


				if(noAmmo && noGrenades && pickupsDisabled && !hasSecondary && wi.weaponName == "unarmed"){
					centerMessageController.CloseCenterMessage();
					GetComponent<DoorController>().UnlockDoor();
					if(targetGO != null){
						(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
					}
				}

			}
		}
	}
}
