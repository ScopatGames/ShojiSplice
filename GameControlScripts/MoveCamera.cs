using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

	public Vector3 cameraPosition;
	
	private Transform cameraTransform;

	void Start () {
		cameraTransform = GameObject.FindGameObjectWithTag (Tags.mainCamera).transform;
		cameraTransform.position = cameraPosition;
	}

}
