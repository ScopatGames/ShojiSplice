using UnityEngine;
using System.Collections;

public class titleShipMovement : MonoBehaviour {


	public float xSpeed = 0.3f;
	public float zSpeed = 0.1f;
	public float xMin = -1.0f;
	public float xMax = 1.0f;
	public float zMin = -0.5f;
	public float zMax = 0.5f;

	private float xPos;
	private float zPos;
	private float newxPos;
	private float newzPos;
	private Vector3 pos;

	void Start(){
		newxPos = transform.position.x;
		newzPos = transform.position.z;
	}

	void FixedUpdate () {
		xPos = Mathf.Lerp(xPos, newxPos, xSpeed * Time.deltaTime);
		zPos = Mathf.Lerp(zPos, newzPos, zSpeed * Time.deltaTime);
		pos = new Vector3 (xPos, 0.0f, zPos);
		GetComponent<Rigidbody>().position = pos;

		if(Mathf.Abs ((GetComponent<Rigidbody>().position.x-newxPos))<0.2f){
			//set new random x position target...
			newxPos = Random.Range (xMin, xMax);
		}
		if(Mathf.Abs ((GetComponent<Rigidbody>().position.z-newzPos))<0.2f){
			//set new random z position target...
			newzPos = Random.Range (zMin, zMax);
		}
	}
}
