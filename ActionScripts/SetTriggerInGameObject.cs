using UnityEngine;
using System.Collections;

public class SetTriggerInGameObject : MonoBehaviour {

	//This script sets a trigger in the targetGameObject's children animators...
	public GameObject targetGameObject;
	public string triggerName;

	[Header("Target GameObject for Post-Script Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool alreadyEnabledOnce = false;

	private Animator[] anim;
	private AnimatorControllerParameter[] acp;

	void OnEnable () {
		if(!alreadyEnabledOnce){
			alreadyEnabledOnce = true;
			anim = targetGameObject.GetComponentsInChildren<Animator> ();
			
			foreach (Animator a in anim){
				acp = a.parameters;
				foreach(AnimatorControllerParameter b in acp){
					if(b.name == triggerName){
						a.SetTrigger(triggerName);
					}
				}
			}
		}
		
		//if there is a chained action...
		if(targetGO){
			(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
		}
	}
}
