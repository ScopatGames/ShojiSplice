using UnityEngine;
using System.Collections;

public class DontDestroyObject : MonoBehaviour {

	private PersistentData persistentData;

	void Awake () {
		//persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
		//persistentData.gameObjectsDoNotDestroyList.Add (gameObject);
		DontDestroyOnLoad (transform.gameObject);
	}

}
