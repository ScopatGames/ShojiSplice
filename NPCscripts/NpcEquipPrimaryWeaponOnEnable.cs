using UnityEngine;
using System.Collections;

public class NpcEquipPrimaryWeaponOnEnable : MonoBehaviour {

	public string weaponName;
	public int ammoCount;
	public bool dropCurrentWeapon;
	[Header("Target GameObject for Post-Script Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool alreadyEnabledOnce = false;
	
	void OnEnable(){
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;
			GetComponentInParent<NonPlayerCharacterWeaponChange> ().EquipPrimaryWeapon (weaponName, ammoCount, dropCurrentWeapon);
			
			//if there is a chained action...
			if(targetGO){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
			}
		}
	}
}
