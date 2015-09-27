using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChapterController : MonoBehaviour {

	public List<GameObject> chapterGeometry = new List<GameObject>();
	public List<GameObject> planetXes = new List<GameObject>();
	[HideInInspector] public bool levelSelected = false;

	private PersistentData persistentData;

	// Use this for initialization
	void Awake () {
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
		//Disable planet Xes and enable chapterGeometry...
		for (int i = 0; i < planetXes.Count; i++) {
			if(i< persistentData.chaptersUnlocked){
				//unlock chapter...
				planetXes[i].SetActive (false);
			}
			else{
				//lock chapter...
				chapterGeometry[i].SetActive (false);
			}
		}
	}
	

}
