using UnityEngine;
using System.Collections;

public class UnlockChapter : MonoBehaviour {

	public int chapter;

	private PersistentData persistentData;


	// Use this for initialization
	void Start () {
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();

		if (persistentData.chaptersUnlocked < chapter) {
			persistentData.chaptersUnlocked = chapter;
			persistentData.ResetDefaults();
			persistentData.Save();
		}
	
	}

}
