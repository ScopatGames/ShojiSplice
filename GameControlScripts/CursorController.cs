using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

	public bool cursorNotVisibleOnAwake;

	private bool cursorChecked = false;


	void Update () {
		if(!cursorChecked){
			if (cursorNotVisibleOnAwake) {
				//make cursor not visible...
				SetCursorNotVisible();
			}
			else{
				//make cursor visible...
				SetCursorVisible();
			}
			cursorChecked = true;
		}

	}
	

	public void SetCursorVisible () {
		//Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void SetCursorNotVisible(){
		//Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

}
