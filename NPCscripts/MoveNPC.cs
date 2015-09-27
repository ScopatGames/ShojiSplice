using UnityEngine;
using System.Collections;

public class MoveNPC : MonoBehaviour {
	private PersistentData persistentData;
	private AvailableSpawnPoints availableSpawnPoints;
	private Transform npcTransform;
	private Vector3 npcPosition;

	void Awake(){
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
		availableSpawnPoints = GetComponent<AvailableSpawnPoints> ();
		npcPosition = availableSpawnPoints.spawnPoints [persistentData.nextSpawnPoint].position;

	}

	// Use this for initialization
	void Start () {
		transform.position = npcPosition;
	}
	

}
