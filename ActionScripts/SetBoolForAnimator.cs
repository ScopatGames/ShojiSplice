using UnityEngine;
using System.Collections;

public class SetBoolForAnimator : MonoBehaviour {

	private Animator anim;
	private bool currentValue;


	void Start () {
		anim = GetComponent<Animator> ();
	
	}
	


	void SetBoolToOpposite(string boolName){
		currentValue = anim.GetBool(boolName);
		anim.SetBool (boolName,!currentValue);
	}
}
