using UnityEngine;
using System.Collections;

public class SceneChangeByCollider : MonoBehaviour {

	private SceneFadeInOut sceneFadeInOut;
	private GameObject player;
	//[Header("If Next Scene = 0, then automatically loads next scene")]
	[Header("Next relative scene, e.g. 1 for thisScene+1")]
	public int nextScene=0;
	public int spawnLocation = 0;
	public bool clearPersistentGameObjects;
	public bool keepWeaponsThroughSceneChange = false;
	[Header("Check this box if the box/sphere collider needs to be reset for use with Activate Box/Sphere Collider Script")]
	public bool useWithActivateCollider;
	private PersistentData persistentData;
	private bool hasSecondaryWeapon = false;


	void Start(){
		player = GameObject.FindGameObjectWithTag (Tags.player);
		sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<SceneFadeInOut>();
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();

	}

	void Update(){
		if (player == null) {
			player = GameObject.FindGameObjectWithTag (Tags.player);
			sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<SceneFadeInOut>();
			persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();

		}
	}

	void OnTriggerEnter(Collider other){


		if (other.gameObject){ 

			if(other.gameObject == player){
				//Reset saved player position...
				persistentData.savedPlayerPosition = new Vector3(1000f, 1000f, 1000f);

				//Reset secondary weapon bool...
				hasSecondaryWeapon = false;

				//Save helmet state...
				Animator[] anims = player.GetComponentsInChildren<Animator>();
				foreach (Animator a in anims){
					AnimatorControllerParameter[] acps = a.parameters;
					foreach (AnimatorControllerParameter b in acps){
						if(b.name == "helmetEquipped"){
							bool currentValue = a.GetBool ("helmetEquipped");
							if(currentValue){
								persistentData.playerHelmetEquipped = 2;
							}
							else {
								persistentData.playerHelmetEquipped = 1;
							}
						}
					}
				}

				if(clearPersistentGameObjects){
					foreach(GameObject go in persistentData.localScenePersistentContainers){
						Destroy (go);
					}
					persistentData.localScenePersistentContainers.Clear();
				}
				if(keepWeaponsThroughSceneChange){
					weaponIndex[] weapons = player.GetComponentsInChildren<weaponIndex>();
					foreach (weaponIndex w in weapons){
						if(w.primaryWeapon){
							persistentData.primaryWeapon = w.weaponName;
							persistentData.primaryWeaponAmmoCount = w.ammoCount;
						}
						else{
							hasSecondaryWeapon = true;
							persistentData.secondaryWeapon = w.weaponName;
							persistentData.secondaryWeaponAmmoCount = w.ammoCount;
						}
					}
					//If the player doesn't have a secondary weapon after iterating through weapons list...
					if(!hasSecondaryWeapon){
						//Set persistent data to null...
						persistentData.secondaryWeapon = "";
					}
				}
				else{
					persistentData.primaryWeapon = "";
					persistentData.secondaryWeapon = "";
				}
				//if(nextScene==0){
				persistentData.nextSpawnPoint = spawnLocation;
				if(useWithActivateCollider){
					if(GetComponent<BoxCollider>()){
						GetComponent<BoxCollider>().enabled = false;
						if(GetComponent<ActivateBoxCollider>()){
							GetComponent<ActivateBoxCollider>().enabled = false;

						}
					}
					else if(GetComponent<SphereCollider>()){
						GetComponent<SphereCollider>().enabled = false;
						if(GetComponent<ActivateSphereCollider>()){
							GetComponent<ActivateSphereCollider>().enabled = false;
						}
					}
				}
				sceneFadeInOut.LoadScene (sceneFadeInOut.thisScene+nextScene);

				//}
				//else{
				//	persistentData.nextSpawnPoint = spawnLocation;
				//	sceneFadeInOut.LoadScene (nextScene); 
				//}

			}
		}
	}

}
