using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

	public float speed = 10f;
	public Vector3 rotateVector;
	
	
	void Update ()
	{
		transform.Rotate(rotateVector, speed * Time.deltaTime);
	}
}
