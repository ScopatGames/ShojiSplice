using UnityEngine;
using System.Collections;

public class LookAtGameObject : MonoBehaviour {
	public Transform target;
	public float rotationSpeed = 3.0f;

	//private EnemySight enemySight;

	//private bool targetNullPrevFrame=true;

	/*void Start(){
		if (transform.tag != Tags.player) {
			enemySight = GetComponentInChildren<EnemySight>();

		}
	}*/


	void FixedUpdate () {

		if(target){
			float angle = Mathf.Atan2 (target.transform.position.x-transform.position.x, target.transform.position.z-transform.position.z) * Mathf.Rad2Deg+0.0f;
			GetComponent<Rigidbody>().MoveRotation(Quaternion.AngleAxis (angle, Vector3.up));
		}
	}


}
