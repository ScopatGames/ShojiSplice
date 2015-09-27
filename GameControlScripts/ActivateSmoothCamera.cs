using UnityEngine;
using System.Collections;

public class ActivateSmoothCamera : MonoBehaviour {

	public float defaultFloorHeight = 0.0f;

	private GameObject cameraMain;
	private SmoothCamera2D smoothCam;
	private Transform playerTransform;
	private Transform emptyCursorTransform;

	//this script also initialized the AudioListener follow script
	private FollowGameObject followGameObject;

	// Use this for initialization
	void Start () {
		cameraMain = GameObject.FindGameObjectWithTag (Tags.mainCamera);
		smoothCam = cameraMain.GetComponent<SmoothCamera2D> ();
		playerTransform = GameObject.FindGameObjectWithTag(Tags.player).transform;
		smoothCam.player = playerTransform;
		emptyCursorTransform = GameObject.FindGameObjectWithTag(Tags.cursor).transform;
		emptyCursorTransform.gameObject.GetComponent<FollowCursor> ().defaultFloorHeight = defaultFloorHeight;
		smoothCam.cursor = emptyCursorTransform;
		smoothCam.enabled = true;
		playerTransform.GetComponent<LookAtGameObject> ().target = emptyCursorTransform;
		followGameObject = cameraMain.GetComponentInChildren<FollowGameObject> ();
		followGameObject.followedGO = playerTransform.gameObject;

	}

}
