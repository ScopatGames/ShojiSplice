using UnityEngine;
using System.Collections;

public class PlayerDisableMovement : MonoBehaviour {

	public GameObject targetGO;
	public string scriptNameToEnable;

	private Animator[] animatorArray;


	void OnEnable () {

		GetComponent<MoveViaInputAxis> ().enabled = false;
		animatorArray = GetComponentsInChildren<Animator> ();
		foreach (Animator anim in animatorArray) {
			if(anim.name != "sprite_player_legs"){
				anim.enabled = false;		
			}
		}
		
		//Disable player feet animation...
		GetComponentInChildren<PlayerFeetAnimator> ().enabled = false;


		if(targetGO){
			(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
		}

		this.enabled = false;
	
	}
	

}
