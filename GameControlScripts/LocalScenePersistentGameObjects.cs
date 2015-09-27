using UnityEngine;
using System.Collections;

public class LocalScenePersistentGameObjects : MonoBehaviour {
	
	[Header("Local scene number GameObjects belong to:")]
	public int localScene;
	
	public bool isCleared = false;
	public bool isCopy = false;

	private PersistentData persistentData;
	private bool exists = false;
	private bool copyExists = false;
	private LocalScenePersistentGameObjects localScenePersistentGameObjects;
	
	void Awake(){
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
				
		//This tests to see if a LocalScenePersistentGameObject already exists within the persistent data for this scene.
		if(persistentData.localScenePersistentContainers.Count > 0){
			for(int i = 0; i < persistentData.localScenePersistentContainers.Count; i ++){
				GameObject go = persistentData.localScenePersistentContainers[i];
				if(go.GetComponent<LocalScenePersistentGameObjects>()){
					localScenePersistentGameObjects = go.GetComponent<LocalScenePersistentGameObjects>();
					if(localScenePersistentGameObjects.localScene == localScene){ 
						if(localScenePersistentGameObjects.isCopy){
							go.SetActive(false);
							copyExists = true;
						}
						else {
							go.SetActive(true);
							exists = true;
						}
					}
					else{
						go.SetActive(false);
					}
				}
			}
			if(exists && copyExists){
				//If it already exists and a copy already exists, then destroy this...
				Destroy (gameObject);
			}
			else if (exists && !copyExists){
				//If it already exists, but a copy does not exist, then make this the copy and save it to the persistent list...
				isCopy = true;
				persistentData.localScenePersistentContainers.Add (gameObject);
			}
			else if(!exists && !copyExists){
				//It does not exist, or this is a Copy; add this copy to the persistent list...
				persistentData.localScenePersistentContainers.Add (gameObject);
			}
			else{
			}
		}
		else{
			//It does not exist, add this to the persistent list...
			persistentData.localScenePersistentContainers.Add (gameObject);
			
		}
		//persistentData.gameObjectsDoNotDestroyList.Add (gameObject);
		DontDestroyOnLoad (gameObject);
	}
	
		
}
