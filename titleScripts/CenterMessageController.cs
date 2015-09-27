using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CenterMessageController : MonoBehaviour {

	private Animator anim;
	public Text messageBox;

	void Awake(){
		//Set Animator...
		anim = GetComponentInParent<Animator> ();

		//Set message box...
		messageBox = GetComponentInChildren<Text> ();

	}
		

	public void UpdateCenterMessage (string newMessage) {
		messageBox.text = newMessage;
	}

	public void OpenCenterMessage(){
		anim.SetTrigger ("triggerCenterMessageOpen");
	}

	public void OpenCenterMessageAfterSeconds(float timeDelay){
		Invoke ("OpenCenterMessage", timeDelay);
	}

	public void CloseCenterMessage(){
		anim.SetTrigger ("triggerCenterMessageClose");
	}

	public void CloseCenterMessageAfterSeconds(float timeDelay){
		Invoke ("CloseCenterMessage", timeDelay);
	}

	public void FlashCenterMessage(){
		anim.SetTrigger ("triggerCenterMessageFlash");
	}

	public void FlashCenterMessageAfterSeconds(float timeDelay){
		Invoke ("FlashCenterMessage", timeDelay);
	}

}
