using UnityEngine;
using System.Collections;

public class InvokeAfterSeconds : MonoBehaviour {

	public float seconds;

	public GameObject targetGO;
	public string scriptNameToEnable;

	[HideInInspector] public bool alreadyEnabledOnce = false;

	// Use this for initialization
	void OnEnable () {
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;
			Invoke ("executeScript", seconds);
		}
	}
	
	void executeScript () {
		if(targetGO){
			(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
		}
	}
}
