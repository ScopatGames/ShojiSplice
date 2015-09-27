using UnityEngine;
using System.Collections;

public class FollowCursor : MonoBehaviour {

	[HideInInspector] public float defaultFloorHeight;

	private int floorMask;

	void Start(){
		//Define floor mask
		//floorMask = LayerMask.GetMask ("Floor");
	}

	void Update () {

		//Find mouse cursor position (cursorPosition) at z=0
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		cursorPosition.y = defaultFloorHeight + 0.5f;

		// Figure out if the player is targeting an elevated location.  If so, adjust the y-component of the direction vector to account for it.
		/*Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;
		if (Physics.Raycast (camRay, out floorHit, 100f, floorMask)) {
			cursorPosition.y = floorHit.point.y + 0.6f;
			Debug.Log ("Floorhit, floorHit.point.y: " + floorHit.point.y);
		}
		else {
			cursorPosition.y = defaultFloorHeight + 0.6f;
			Debug.Log ("Defaulthit, defaultFloor: " + defaultFloorHeight);
		}*/
		
		//Move GameObject to cursorPosition
		transform.position = cursorPosition;
	}
}
