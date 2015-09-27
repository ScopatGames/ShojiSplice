using UnityEngine;
using System.Collections;

public class PlayerEnableMovement : MonoBehaviour {

	public GameObject targetGO;
	public string scriptNameToEnable;

	private Animator[] animatorArray;
	
	void OnEnable () {
		GetComponent<MoveViaInputAxis> ().enabled = true;
		animatorArray = GetComponentsInChildren<Animator> ();
		foreach (Animator anim in animatorArray) {
			anim.enabled = true;		
		}
		//Enable player feet animation...
		GetComponentInChildren<PlayerFeetAnimator> ().enabled = true;

		if(targetGO){
			(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
		}

		this.enabled = false;
	}
}
