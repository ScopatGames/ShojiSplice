using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneFadeInOut : MonoBehaviour {
	
	[Header("Global scene numbers:")]
	public int thisScene;//set to HideInInspector if new method of relative scene change works
	public int nextScene;//remove is new method of relative scene change works
	
	[Header("Chapter number:")]
	public int thisChapter;
	
	[Header("Chapter local scene number:")]
	public int thisLocalScene;
	
	[HideInInspector] public bool disableRespawn;
	[HideInInspector] public bool sceneStarting = true;
	
	//[Header("Reset local scene persistent gameobjects prefab:")]
	//public GameObject resetLocalPersistentGameobjects;
	
	[Header("Target GameObject for Post-SceneClear Event")]
	public bool autoFindLocalObjectsContainerForPostClearAction = false;
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool targetEnabled = false;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	[Header("Conditions for scene clear (exit):")]
	public bool scriptingMustBeComplete = false;
	[HideInInspector] public bool scriptingComplete = false;
	public bool enemiesMustBeDestroyed = false;
	[HideInInspector] public bool enemiesComplete = false;
	public List<GameObject> remainingScriptedEventsList = new List<GameObject>();
	public List<GameObject> remainingEnemiesList = new List<GameObject>();

	private Animator anim;
	private GameObject localPersistentGOs;
	private GameObject localPersistentGOsCopy;
	private LocalScenePersistentGameObjects localPersistentScript;
	private PauseMenuController pauseMenuController;
	private PersistentData persistentData;
	private GameObject player;
	private bool resetLocalObjects = false;
	private int tempNextScene;
	private GameObject[] allGameObjectsWithTransforms;
	private weaponIndex[] weapons;
	private SmoothCamera2D smoothCamera2D;


	[HideInInspector] public bool localPersistentSet = false;
		
	
	void Awake(){


		//Set Animator...
		anim = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponent<Animator> ();
		pauseMenuController = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponentInChildren<PauseMenuController> ();
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
		//persistentData.realGameObjectsToDestroyListArray = null;
		smoothCamera2D = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<SmoothCamera2D> ();
	}
	
	void Start(){
		smoothCamera2D.ChangeLag (0.5f, 12f);

		if(autoFindLocalObjectsContainerForPostClearAction){
			targetGO = GameObject.FindGameObjectWithTag(Tags.localScene);
		}

		/*ENABLE FOR ADDITIVE LOADING
		//set thisScene...
		thisScene = persistentData.lastLoadedLevel;
		ENABLE FOR ADDITIVE LOADING*/

		//DISABLE FOR ADDITIVE LOADING
		thisScene = Application.loadedLevel;
		//DISABLE FOR ADDITIVE LOADING

		player = GameObject.FindGameObjectWithTag (Tags.player);
		//Start fading in:
		anim.SetTrigger("triggerFadeIn");


	}
	
	void Update (){
		//Sort out persistent gameobjects...
		if(!localPersistentSet){
			if(GameObject.FindGameObjectWithTag(Tags.localScene)){
				localPersistentGOs = GameObject.FindGameObjectWithTag(Tags.localScene);
				localPersistentScript = localPersistentGOs.GetComponent<LocalScenePersistentGameObjects>();
				//Create copy of persistent GameObjects...
				localPersistentGOsCopy = CopyPersistentGameObjects(localPersistentGOs, localPersistentScript);
				localPersistentSet = true;
			}
		}
		
		//Respawn logic...
		if (Input.GetKeyDown (KeyCode.Tab) && Application.loadedLevel != 1 && !disableRespawn && !pauseMenuController.isPaused && player == null) {
			resetLocalObjects = true;
			tempNextScene = thisScene;
			EndScene ();
		}
	}

	void EndScene(){
		smoothCamera2D.lagTime = 3f;

		//Start fading out:
		anim.SetTrigger("triggerFadeOut");
		//Pause for the screen to fade out:
		Invoke ("ChangeScene", 0.5f);
	}

	void ChangeScene(){
		//Continue with scene change:
		if(localPersistentGOs){
			//reset local persistent gameobjects to copy version...
			if (resetLocalObjects || !localPersistentScript.isCleared) {
				if(resetLocalObjects){
				}
				else if(!localPersistentScript.isCleared){
				}
				Destroy(localPersistentGOs);
				persistentData.localScenePersistentContainers.Remove (localPersistentGOs);
				localPersistentGOsCopy.SetActive (true);
				localPersistentGOsCopy.GetComponent<LocalScenePersistentGameObjects> ().isCopy = false;
				localPersistentGOs = localPersistentGOsCopy;
				localPersistentScript = localPersistentGOs.GetComponent<LocalScenePersistentGameObjects>();
				localPersistentGOsCopy = CopyPersistentGameObjects(localPersistentGOs, localPersistentScript);


			}
			//else, save a copy of the existing state as the copy version...
			else {
				localPersistentGOsCopy = CopyPersistentGameObjects(localPersistentGOs, localPersistentScript);
			}
		}

		/*ENABLE FOR ADDITIVE LOADING
		//Find all Transforms...
		allGameObjectsWithTransforms = Resources.FindObjectsOfTypeAll (typeof(GameObject)) as GameObject[];
		List<GameObject> realList = new List<GameObject> ();
		GameObject tempGameObject = new GameObject();
		tempGameObject.transform.SetAsFirstSibling();
		foreach (GameObject GO in allGameObjectsWithTransforms){
			//get the root transform...
			Transform rootObject = GO.transform.root;
			if(!realList.Contains (rootObject.gameObject)){
				if(rootObject.GetSiblingIndex() != 0 && !persistentData.gameObjectsDoNotDestroyList.Contains (rootObject.gameObject)){
					realList.Add(rootObject.gameObject);
				}
			}
		}
		Destroy(tempGameObject);
		persistentData.realGameObjectsToDestroyListArray = realList.ToArray ();
		persistentData.ChangeSceneAdditive(tempNextScene);
		ENABLE FOR ADDITIVE LOADING*/

		//DISABLE FOR ADDITIVE LOADING
		Application.LoadLevel(tempNextScene);
		//DISABLE FOR ADDITIVE LOADING
	}

	//Function to load a scene from another script:
	public void LoadScene(int level){
		tempNextScene = level;
		EndScene ();
	}
	
	public GameObject CopyPersistentGameObjects(GameObject localGOs, LocalScenePersistentGameObjects localScript){

		//Check if there is already a copy stored...
		for(int i = 0; i < persistentData.localScenePersistentContainers.Count; i++){
			GameObject go = persistentData.localScenePersistentContainers[i];
			//If there is a copy, destroy it...
			if(go.GetComponent<LocalScenePersistentGameObjects>()){
				LocalScenePersistentGameObjects lspgo = go.GetComponent<LocalScenePersistentGameObjects>();
				if(lspgo.localScene == thisLocalScene && lspgo.isCopy){
					persistentData.localScenePersistentContainers.Remove (go);
					Destroy (go);
				}
			}
		}
		
		//Change isCopy before instantiation, so the new persistent gameobjects get catalogued as a copy correctly...
		//localScript.isCopy = true;
		//Instantiate a copy of the persistent gameobjects...
		GameObject localGOsCopy = Instantiate(localGOs, localGOs.transform.position, localGOs.transform.rotation) as GameObject;
		//Reset the current version's isCopy bool...
		//localScript.isCopy = false;
		//Set the copy to inactive...
		localGOsCopy.SetActive (false);
		return localGOsCopy;
	}
	
	public void SceneClear(){
		if(GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerStats>().playerIsAlive){

			//If there is a container of level exits for scenechangebycollider scripts...
			if(GameObject.FindGameObjectWithTag(Tags.levelExits)){
				//Set each scenechangebycollider script to be enabled to allow the player to leave the scene...
				SceneChangeByCollider[] sceneChangeByCollider = GameObject.FindGameObjectWithTag(Tags.levelExits).GetComponentsInChildren<SceneChangeByCollider>();
				foreach (SceneChangeByCollider scbc in sceneChangeByCollider){
					scbc.enabled = true;
					if(scbc.GetComponentInChildren<PlayParticleSystem>()){
						scbc.GetComponentInChildren<PlayParticleSystem>().PlayPS();
					}
				}
			}
			
			//Save player weapons and location...
			weapons = player.GetComponentsInChildren<weaponIndex>();
			foreach (weaponIndex w in weapons){
				if(w.primaryWeapon){
					persistentData.primaryWeapon = w.weaponName;
					persistentData.primaryWeaponAmmoCount = w.ammoCount;
				}
				else{
					persistentData.secondaryWeapon = w.weaponName;
					persistentData.secondaryWeaponAmmoCount = w.ammoCount;
				}
			}
			persistentData.savedPlayerPosition = player.transform.position;

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


			//If post-action event exists, execute it...
			if(!targetEnabled && targetGO != null){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
				targetEnabled = true;
			}
							
			//Do this step last!
			//set the bool for level clear...
			localPersistentScript.isCleared = true;
			//Again, make sure this step is last!!
			localPersistentGOsCopy = CopyPersistentGameObjects (localPersistentGOs, localPersistentScript);
		}
	}

	public void SceneStatusCheck(bool wasEnemy){
		if(GameObject.FindGameObjectWithTag(Tags.localScene)){
			if(!localPersistentScript.isCleared){
				if(enemiesMustBeDestroyed && scriptingMustBeComplete){
					if(enemiesComplete && scriptingComplete){
						if(wasEnemy){
							StartCoroutine(SceneClearDelay(2.0f));
						}
						else{
							StartCoroutine(SceneClearDelay(0.1f));
						}
					}
				}
				else if(enemiesMustBeDestroyed && !scriptingMustBeComplete){
					if(enemiesComplete){
						StartCoroutine(SceneClearDelay(2.0f));

					}
				}
				else if(!enemiesMustBeDestroyed && scriptingMustBeComplete){
					if(scriptingComplete){
						StartCoroutine(SceneClearDelay(0.1f));

					}
				}

			}
		}
	}

	IEnumerator SceneClearDelay(float delay){
		yield return new WaitForSeconds(delay);
		SceneClear ();
	}

	public void SceneStatusCheckDelay(bool wasEnemy){
		StartCoroutine (SceneStatusCheckDelayIE(wasEnemy));
	}

	IEnumerator SceneStatusCheckDelayIE(bool wasEnemy){
		yield return new WaitForSeconds(0.2f);
		if(wasEnemy){
			if(remainingEnemiesList.Count == 0){
				enemiesComplete = true;
			}
		}
		else{
			if(remainingScriptedEventsList.Count == 0){
				scriptingComplete = true;
			}
		}
		SceneStatusCheck (wasEnemy);


	}

}
