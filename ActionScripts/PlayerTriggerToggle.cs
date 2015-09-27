using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTriggerToggle : MonoBehaviour {

	public GameObject targetGameObject;
	//public string triggerTrue;
	//public string triggerFalse;
	//public bool triggerState;
	public string triggerName;
	public List<Collider> hitColliders = new List<Collider>();

	private Animator[] anim;
	private AnimatorControllerParameter[] acp;
	//private int previousColliderCount;
	private Animator previousAnimator;
	//private string triggerName;

	void FixedUpdate(){
		if(hitColliders.Count > 0 ){
			foreach (Collider col in hitColliders){
				//Debug.Log (col.transform.name);
				if(col == null){
					hitColliders.Remove (col);
					return;
				}
			}
		}
		//previousColliderCount = hitColliders.Count;
		if(previousAnimator != null){
			previousAnimator.ResetTrigger(triggerName);
			previousAnimator = null;
		}
	}


	void OnTriggerEnter(Collider other){

		if(other.gameObject.tag == Tags.player){
			hitColliders.Add (other);
			//if(previousColliderCount == 0){
				anim = targetGameObject.GetComponentsInChildren<Animator> ();

				foreach (Animator a in anim){
					acp = a.parameters;
					foreach(AnimatorControllerParameter b in acp){
						if(b.name == triggerName){
							a.SetTrigger(triggerName);
							previousAnimator = a;
						}
					}
				}
			//}

		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == Tags.player){
			hitColliders.Remove(other);
		}
	}

}
