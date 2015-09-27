using UnityEngine;
using System.Collections;

public class thrownMover : MonoBehaviour {

	public float throwStrengthFactor;
	private float angleLimit= 10.0f;
	public int floorMask;

	// Use this for initialization
	void Start () {
		floorMask = LayerMask.GetMask ("Floor");
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;
		if (Physics.Raycast (camRay, out floorHit, 100f, floorMask)) {
			cursorPosition.y = floorHit.point.y + 0.5f;
		}
		else {
			cursorPosition.y = 0.5f;
		}
		Vector3 direction = new Vector3 (cursorPosition.x - transform.position.x, cursorPosition.y - transform.position.y, cursorPosition.z-transform.position.z);
		float throwStrength = direction.sqrMagnitude;
		direction = Vector3.Normalize (direction);

		//Similar to the bulletMover() script, the direction angles need to be checked and corrected to avoid awkward
		//angles while firing...

		// theta is the angle between "direction" and transform.up of the bullet.  If theta is too large, the player will awkwardly shoot sideways, so...
		//Use vector projection onto a plane (directionProjection) to figure out theta...
		Vector3 directionProjection = new Vector3 ();
		directionProjection = direction - Vector3.Dot (direction, transform.forward)*transform.forward;
		float theta = Vector3.Angle (directionProjection, transform.up);

		// If the "direction" is too great an angle...
		if(theta > angleLimit){
			//Set the angleLimit as the limiting angle on the "direction"
			float phi = Mathf.Atan2 (transform.up.z,transform.up.x);
			direction = new Vector3(Mathf.Cos (phi+angleLimit*Mathf.Deg2Rad), direction.y, Mathf.Sin (phi+angleLimit*Mathf.Deg2Rad));
		}		
		GetComponent<Rigidbody>().AddForce(Mathf.Sqrt (1f * gameObject.GetComponent<Rigidbody>().drag * throwStrength)*direction*throwStrengthFactor);
	}
}
