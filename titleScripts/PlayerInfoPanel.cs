using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfoPanel : MonoBehaviour {

	private Text primaryWeaponInfo;

	private Animator anim;
	private weaponIndex[] weapIndex;
	private string newString;
	private GameObject playerGO;

	void Awake(){
		//Setup...
		playerGO = GameObject.FindGameObjectWithTag(Tags.player);
		anim = GetComponentInParent<Animator> ();
		primaryWeaponInfo = GetComponentInChildren<Text>();

	}



	void Update(){
		if(Input.GetButtonDown ("Info")){
			OpenInfoPanel();
		}

		if(Input.GetButtonUp ("Info")){
			CloseInfoPanel();
		}
	}

	public void UpdatePrimaryWeaponInfo(){
	
		weapIndex = playerGO.GetComponentsInChildren<weaponIndex> ();

		foreach (weaponIndex wi in weapIndex) {
			if(wi.primaryWeapon){
				if(wi.weaponName == "unarmed"){
					newString = null;
					primaryWeaponInfo.text = newString;
				}
				else if(wi.ammoCount>0){
					newString = "(" + wi.ammoCount + ")";
					primaryWeaponInfo.text = newString;
				}
				else{
					newString = "";
					primaryWeaponInfo.text = newString;
				}
			}
		}
	

	
	}

	public void OpenInfoPanel(){
		anim.SetTrigger("triggerPlayerInfoOpen");
	
	}
	
	public void CloseInfoPanel(){
		anim.SetTrigger("triggerPlayerInfoClose");

	}

	public void DisplayNewWeaponInfo(){
		OpenInfoPanel ();
		Invoke ("CloseInfoPanel", 1f);
	}
}
