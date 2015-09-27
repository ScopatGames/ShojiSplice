using UnityEngine;
using System.Collections;

public class MoveViaInputAxis : MonoBehaviour {

	public float moveSpeed;
	public float lerpSpeed;
	private Rigidbody rb;

	void Start(){
		rb = GetComponent<Rigidbody> ();

	}

	void FixedUpdate() {
		if(Mathf.Abs (Input.GetAxisRaw ("Horizontal"))>0.0f & Input.GetAxisRaw ("Vertical")==0.0f){
			rb.velocity = new Vector3 (Mathf.Lerp (0, Input.GetAxisRaw ("Horizontal") * moveSpeed, lerpSpeed),0.0f, 0.0f);
		}
		else if(Mathf.Abs (Input.GetAxisRaw ("Vertical"))>0.0f & Input.GetAxisRaw ("Horizontal")==0.0f){
			rb.velocity = new Vector3 (0.0f, 0.0f, Mathf.Lerp (0, Input.GetAxisRaw ("Vertical") * moveSpeed, lerpSpeed));
		}
		else {
			rb.velocity = new Vector3 (Mathf.Lerp (0, Input.GetAxisRaw ("Horizontal") * moveSpeed*0.707f, lerpSpeed),0.0f, Mathf.Lerp (0, Input.GetAxisRaw ("Vertical") * moveSpeed*0.707f, lerpSpeed));
		}

		//Script below uses GetAxis instead of GetAxisRaw resulting in smoother movement
		/*if(Mathf.Abs (Input.GetAxis ("Horizontal"))>0.0f & Input.GetAxis ("Vertical")==0.0f){
			rigidbody.velocity = new Vector3 (Mathf.Lerp (0, Input.GetAxis ("Horizontal") * moveSpeed, lerpSpeed),0.0f, 0.0f);
		}
		else if(Mathf.Abs (Input.GetAxis ("Vertical"))>0.0f & Input.GetAxis ("Horizontal")==0.0f){
			rigidbody.velocity = new Vector3 (0.0f, 0.0f, Mathf.Lerp (0, Input.GetAxis ("Vertical") * moveSpeed, lerpSpeed));
		}
		else {
			rigidbody.velocity = new Vector3 (Mathf.Lerp (0, Input.GetAxis ("Horizontal") * moveSpeed*0.707f, lerpSpeed),0.0f, Mathf.Lerp (0, Input.GetAxis ("Vertical") * moveSpeed*0.707f, lerpSpeed));
		}*/

	}
}
