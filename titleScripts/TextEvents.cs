using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TextEvents : MonoBehaviour {

	public Color normalColor;
	public Color highlightedColor;
	public Color disabledColor;

	public bool buttonIsDisabled;

	public bool checkChaptersUnlocked;

	private Text textBox;
	private PersistentData persistentData;

	void Awake(){
		if (checkChaptersUnlocked) {
			persistentData = GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<PersistentData>();
			if(persistentData.chaptersUnlocked > 0){
				//enable button
				buttonIsDisabled = false;
			}
			else{
				//disable button
				buttonIsDisabled = true;
			}
		}
	}

	void Start () {
		textBox = GetComponent<Text> ();
		if (buttonIsDisabled){
			//Set color of button to disabled...
			SetDisabledColor();
			//Disable the PointerClick event triggers...
			EventTrigger eventTrigger = GetComponent<EventTrigger>();
			for(int i = 0; i < eventTrigger.triggers.Count; i++){
				EventTrigger.Entry trigger = eventTrigger.triggers[i];
				EventTrigger.TriggerEvent triggerEvent = trigger.callback;
				for(int j = 0; j < triggerEvent.GetPersistentEventCount(); j++){
					if(trigger.eventID == EventTriggerType.PointerClick){
						triggerEvent.SetPersistentListenerState(j, UnityEventCallState.Off);
					}
				}
			}
		}
		else{
			SetNormalColor();
		}
	}
	
	public void SetNormalColor(){
		if(!buttonIsDisabled){
			textBox.color = normalColor;
		}
	}

	public void SetHighlightedColor(){
		if(!buttonIsDisabled){
			textBox.color = highlightedColor;
		}
	}

	public void SetDisabledColor(){
		textBox.color = disabledColor;
	}


}
