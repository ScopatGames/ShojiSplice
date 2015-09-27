using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {

	Animator anim;
	public bool pause = false;

	void Start(){
		anim = GetComponent<Animator> ();

	}

	void FixedUpdate(){
		if(!pause){
			float move = (Mathf.Abs (Input.GetAxisRaw ("Vertical"))+Mathf.Abs (Input.GetAxisRaw ("Horizontal")))*0.5F;
			anim.SetFloat ("speed", move);
		}
		else{
			anim.SetFloat ("speed", 0f);
		}


	}

}
