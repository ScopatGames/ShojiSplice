using UnityEngine;
using System.Collections;

public class SetAnimatorSpeedInChildren : MonoBehaviour {

	public float animationSpeed;
	private Animator[] anim;


	void Start(){
		Invoke ("SetAnimationSpeed", 0.5f);
	}

	void SetAnimationSpeed () {
		anim = GetComponentsInChildren<Animator> ();
		foreach (Animator a in anim) {
			a.speed = animationSpeed;	
		}
	}

}
