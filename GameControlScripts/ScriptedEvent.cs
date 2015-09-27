using UnityEngine;
using System.Collections;

public class ScriptedEvent : MonoBehaviour {
	public bool ignoreScriptedEventForSceneClearing = false;
	public int scriptedEventIndex;
	private SceneFadeInOut sceneFadeInOut;
	private LocalScenePersistentGameObjects localScenePersistentGameObjects;
	private bool localSceneObjectsExist = false;

	void Start () {
		sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<SceneFadeInOut>();
		if(GameObject.FindGameObjectWithTag(Tags.localScene)){
			localSceneObjectsExist = true;
			localScenePersistentGameObjects = GameObject.FindGameObjectWithTag(Tags.localScene).GetComponent<LocalScenePersistentGameObjects>();
		}
		if(localSceneObjectsExist){
			if(!ignoreScriptedEventForSceneClearing && !localScenePersistentGameObjects.isCleared){
				StartCoroutine(DelayedListAdd ());
			}
		}
	}

	public void CompleteScriptedEvent(){
		sceneFadeInOut.remainingScriptedEventsList.Remove (gameObject);
		if(sceneFadeInOut.remainingScriptedEventsList.Count == 0){
			sceneFadeInOut.scriptingComplete = true;
		}
		sceneFadeInOut.SceneStatusCheck(false);
	}

	IEnumerator DelayedListAdd(){
		yield return new WaitForSeconds(0.1f);
		sceneFadeInOut.remainingScriptedEventsList.Add(gameObject);
		sceneFadeInOut.scriptingComplete = false;
	}

}
