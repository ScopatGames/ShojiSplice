using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnableScriptWhenListIsEmpty : MonoBehaviour {

	public List<GameObject> listToEmpty = new List<GameObject>();
	[Header("Script location and name to enable when the list is empty:")]
	public GameObject targetGO;
	public string scriptNameToEnable;

	[Header("Event to trigger after script enabled?")]
	public GameObject targetGO2;
	public string scriptNameToEnable2;

		
	// Update is called once per frame
	void Update () {

	
		if (listToEmpty.Count == 0) {
			if(targetGO != null){
				(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;

				if(targetGO2 != null){
					(targetGO2.GetComponent (scriptNameToEnable2) as MonoBehaviour).enabled = true;
				}
			}		
		}
		else{
			for(int i = 0; i < listToEmpty.Count; i++){
				if(listToEmpty[i]==null){
					listToEmpty.RemoveAt(i);
				}
			}
		}

	}
}
