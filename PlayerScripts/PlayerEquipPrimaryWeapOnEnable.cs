using UnityEngine;
using System.Collections;

public class PlayerEquipPrimaryWeapOnEnable : MonoBehaviour {
	public string weaponName;
	public int ammoCount;
	public bool dropCurrentWeapon;

	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	void OnEnable(){
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;
			GameObject.FindGameObjectWithTag (Tags.player).GetComponent<PlayerWeaponChange> ().EquipPrimaryWeapon (weaponName, ammoCount, dropCurrentWeapon);

			//if there is a chained action...
			if(targetGO){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
			}
		}
	}
}
