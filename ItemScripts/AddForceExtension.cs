using UnityEngine;
using System.Collections;

public class AddForceExtension : MonoBehaviour {

	public float minForceMagnitude;
	public float maxForceMagnitude;
	public int minPeriod;
	public int maxPeriod;


	private int counter;
	private float forceSign = 1f;
	private float forceInstant = 0f;
	private Rigidbody rb;
	private Vector3 vectorForce;
	private float forceMagnitude;
	private int period;

	void Start(){
		rb = GetComponent<Rigidbody> ();
		period = Random.Range (minPeriod, maxPeriod + 1)*2;
		forceMagnitude = Random.Range (minForceMagnitude, maxForceMagnitude);
		counter = period / 2;
		if (Random.Range (0, 2) == 0) {
			forceSign = 1f;
		}
		else{
			forceSign = -1f;
		}
	}

	
	// Update is called once per frame
	void Update () {
		if (counter <= period) {
			counter ++;
		}
		else{
			counter = 0;
			forceSign = forceSign*-1f;
		}
		forceInstant = forceMagnitude * forceSign;
		vectorForce = forceInstant*transform.right;
		rb.AddForce(vectorForce);

	}
}
