using UnityEngine;
using System.Collections;

public class UpdateParticleRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log (GameObject.Find ("PlayerEmpty").transform.eulerAngles.y);
		transform.GetComponent<ParticleSystem>().startRotation = GameObject.Find ("PlayerEmpty").transform.eulerAngles.y;
	}
	

}
