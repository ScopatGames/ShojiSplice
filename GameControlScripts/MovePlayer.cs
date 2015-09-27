using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

	public float cameraHeight = 23.9f;
	private Vector3 playerPosition;
	private PersistentData persistentData;
	private AvailableSpawnPoints availableSpawnPoints;
	private Transform playerTransform;
	private Transform cameraTransform;
	private Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);

	void Awake(){
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
		availableSpawnPoints = GetComponent<AvailableSpawnPoints> ();
		playerTransform = GameObject.FindGameObjectWithTag (Tags.player).transform;
		playerPosition = availableSpawnPoints.spawnPoints [persistentData.nextSpawnPoint].position;
		cameraTransform = GameObject.FindGameObjectWithTag (Tags.mainCamera).transform;
	}

	// Use this for initialization
	void Start () {
		//Set player and camera position
		if (persistentData.savedPlayerPosition != resetPosition) {
			playerPosition = persistentData.savedPlayerPosition;
			playerTransform.position = playerPosition;
			cameraTransform.position = new Vector3(playerPosition.x, cameraHeight, playerPosition.z);			
		}
		else{
			playerTransform.position = playerPosition;
			cameraTransform.position = new Vector3(playerPosition.x, cameraHeight, playerPosition.z);
		}
	}
}
