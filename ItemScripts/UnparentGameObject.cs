using UnityEngine;
using System.Collections;

public class UnparentGameObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.parent = null;
	}

}
