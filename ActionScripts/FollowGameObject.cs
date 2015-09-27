using UnityEngine;
using System.Collections;

public class FollowGameObject : MonoBehaviour {

	public GameObject followedGO;
	public Vector3 offsetVector;

	private Vector3 newPosition;


	void Update () {

		if(followedGO){
			newPosition = followedGO.transform.position + offsetVector;
			transform.position = newPosition;
		}
	}
}
