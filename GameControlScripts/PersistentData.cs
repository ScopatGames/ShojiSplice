using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Audio;

public class PersistentData : MonoBehaviour {

	// [Header("These are persistent variables.. do not change..")]
	public int nextSpawnPoint=0;
	[HideInInspector] public string primaryWeapon;
	[HideInInspector] public int primaryWeaponAmmoCount;
	[HideInInspector] public string secondaryWeapon;
	[HideInInspector] public int secondaryWeaponAmmoCount;
	[HideInInspector] public Vector3 savedPlayerPosition;
	[HideInInspector] public int playerHelmetEquipped; //0 = default; 1 = not equipped; 2 = equipped

	[HideInInspector] public List<GameObject> localScenePersistentContainers = new List<GameObject>();

	public int chaptersUnlocked;
	public AudioMixer masterMixer;
	//public GameObject[] realGameObjectsToDestroyListArray;
	//public List<GameObject> gameObjectsDoNotDestroyList = new List<GameObject>();
	[HideInInspector] public int lastLoadedLevel;

	void Awake(){
		lastLoadedLevel = Application.loadedLevel;
	}

	void Start(){
		savedPlayerPosition = new Vector3 (1000f, 1000f, 1000f); 
		Load ();

	}

	public void Save () {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerinfo.dat");

		PlayerData data = new PlayerData ();
		data.chapsUn = chaptersUnlocked;
		float volMusic;
		masterMixer.GetFloat("musicVolume", out volMusic);
		data.volMusic = volMusic;
		float volSFX;
		masterMixer.GetFloat ("sfxVolume", out volSFX);
		data.volSFX = volSFX;
		
		bf.Serialize (file, data);
		file.Close ();
	}
	
	public void Load () {
		if (File.Exists (Application.persistentDataPath + "/playerinfo.dat")){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();

			chaptersUnlocked = data.chapsUn;
			masterMixer.SetFloat("musicVolume", data.volMusic);
			masterMixer.SetFloat ("sfxVolume", data.volSFX);			

		}
	}
	
	[Serializable]
	class PlayerData{
		public int chapsUn;
		public float volMusic;
		public float volSFX;
	}

	public void ResetDefaults(){
		nextSpawnPoint = 0;
		localScenePersistentContainers.Clear ();
		primaryWeapon = "";
		primaryWeaponAmmoCount=0;
		secondaryWeapon="";
		secondaryWeaponAmmoCount=0;
		savedPlayerPosition = new Vector3 (1000f, 1000f, 1000f);
		playerHelmetEquipped = 0;
	}

	public void ChangeSceneAdditive(int scene){
		/*for(int j=0; j < realGameObjectsToDestroyListArray.Length; j++){
			Destroy (realGameObjectsToDestroyListArray[j]);
		}
		lastLoadedLevel = scene;
		Application.LoadLevelAdditiveAsync (scene);*/
	}
}
