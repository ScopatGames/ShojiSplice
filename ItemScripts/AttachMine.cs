using UnityEngine;
using System.Collections;

public class AttachMine : MonoBehaviour {

	public GameObject weapon;
	public LayerMask attachLayerMask;

	private Transform playerTransform;
	private float attachDistance = 1f;


	void Start(){
		playerTransform = GameObject.FindGameObjectWithTag (Tags.player).transform;
	}

	void Update () {
		if (Input.GetButtonDown ("Attack")&& Input.GetButton ("Shift")) {

			//Get cursor position and player position to find direction of Raycast...
			Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
			cursorPosition.y = 0.5f;
			Vector3 direction = new Vector3 (cursorPosition.x - transform.position.x, cursorPosition.y - transform.position.y, cursorPosition.z-transform.position.z);
			direction.y = 0.5f;

			RaycastHit hit;

			Physics.Raycast (playerTransform.position, direction, out hit, attachDistance, attachLayerMask);

			if(hit.collider != null && hit.collider.tag != Tags.physicsInteractable && hit.collider.tag != Tags.door){

				//Define the normal vector at the hit point, with the y-component=0f.
				Vector3 hitNormal = new Vector3(hit.normal.x, 0f, hit.normal.z);

				Instantiate (weapon, hit.point + 0.05f*hitNormal, Quaternion.LookRotation(hitNormal));//*Quaternion.Euler(90, 0, 0));
				transform.parent.gameObject.GetComponent<PlayerWeaponChange>().hasSecondaryWeap = false;
				
				Destroy (gameObject);
			}
			
		}
	}
}
