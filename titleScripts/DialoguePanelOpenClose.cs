using UnityEngine;
using System.Collections;

public class DialoguePanelOpenClose : MonoBehaviour {

	[HideInInspector] public InteractionController activeInteractionController;
	[HideInInspector] public NpcInteractionController activeNpcInteractionController;

	private Animator anim;

	void Awake(){
		//Set Animator...
		anim = GetComponentInParent<Animator> ();
	}
		
	public void OpenDialogue(){
		anim.SetTrigger("triggerDialogueOpen");
	}

	public void CloseDialogue(){
		anim.SetTrigger("triggerDialogueClose");
	}

	public void CancelDialogue(){
		if (activeInteractionController != null) {
			activeInteractionController.cancelButtonClicked = true;
		}
		else if(activeNpcInteractionController != null){
			activeNpcInteractionController.cancelButtonClicked = true;
		}

	}
}
