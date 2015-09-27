using UnityEngine;
using System.Collections;

public class ScriptedEventCompleteOnEnable : MonoBehaviour {

	public int scriptedEventIndex;

	[Header("Target GameObject for Post-enabled Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool targetEnabled = false;
	[HideInInspector] public bool alreadyEnabledOnce = false;
	
	//private SceneFadeInOut sceneFadeInOut;
	private GameObject localPersistentGOs;
	private GameObject[] scriptedEvents;
	private ScriptedEvent scriptedEvent;

	void OnEnable () {
		//sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<SceneFadeInOut>();
		scriptedEvents = GameObject.FindGameObjectsWithTag (Tags.scriptedEvent);
		foreach (GameObject go in scriptedEvents) {
			if(go.GetComponent<ScriptedEvent>().scriptedEventIndex == scriptedEventIndex){
				scriptedEvent = go.GetComponent<ScriptedEvent>();
				scriptedEvent.CompleteScriptedEvent ();
				break;
			}
		}

		//If post-action event exists, execute it...
		if(!targetEnabled && targetGO != null){
			(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
			targetEnabled = true;
		}

		StartCoroutine (DelayedDestroy ());
	}

	IEnumerator DelayedDestroy(){
		yield return new WaitForSeconds(1.0f);
		Destroy (gameObject);
	}
}
