using UnityEngine;
using System.Collections;

public class MenuTitleController : MonoBehaviour {
	private Animator anim;

	void Awake(){
		//Set Animator...
		anim = GetComponentInParent<Animator> ();
	
	}

	public void SlideMainTitlesLeft(){
		anim.SetTrigger ("triggerSlideMainTitlesLeft");
	}

	public void SlideMainTitlesRight(){
		anim.SetTrigger ("triggerSlideMainTitlesRight");
	}
}
