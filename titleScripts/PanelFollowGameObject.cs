using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelFollowGameObject : MonoBehaviour {

	public GameObject followedGO;

	private Vector3 newPosition;
	private RectTransform rectTransform;

	void Start(){
		if(followedGO == null){
			followedGO = GameObject.FindGameObjectWithTag(Tags.player);
		}
		rectTransform = GetComponent<RectTransform> ();
	}

	void Update () {
		if(followedGO){
			newPosition = Camera.main.WorldToScreenPoint(followedGO.transform.position);
			rectTransform.position = newPosition;
		}
	}
}
