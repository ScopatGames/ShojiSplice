using UnityEngine;
using System.Collections;

public class RoofController : MonoBehaviour {

	public LayerMask sightMask;
	public Vector3 direction;
	public GameObject roofGameObject;
	public float fadeSpeed= 0.5f;
	public GameObject ambientLight;
		
	private SphereCollider col;
	private GameObject player;
	private bool roofFaded = false;

	void Awake(){
		col = GetComponent<SphereCollider> ();
		player = GameObject.FindGameObjectWithTag (Tags.player);

	}

	void Update(){
		/*if(roofFading){
			FadeRoofOut();
			roofFading = false;
		}
		else if(roofShowing){
			FadeRoofIn ();
			roofShowing = false;
		}*/
	}

	void OnTriggerEnter(Collider other){
		//If the player has entered the trigger sphere...
		if(other.gameObject == player)
		{

			direction = other.transform.position - transform.position;

			RaycastHit hit;
			
			//... and if a raycast towards the player hits something...
			Debug.DrawRay(transform.position, direction, Color.green);

			if(Physics.Raycast (transform.position, direction.normalized, out hit, col.radius, sightMask))
			{
				// ... and if the raycast hits the player...
				if(hit.collider.gameObject == player && roofFaded == false)
				{
					//... disable the roof.
					StartCoroutine(FadeTo (0.0f, 0.05f, 0.6f));
					roofFaded = true;

				}
				else if(hit.collider.gameObject != player && roofFaded == true){
					//... enable roof.
					StartCoroutine(FadeTo (1.1f, 0.3f, 0.6f));
					roofFaded = false;
				}
			}
		}
	}
	
	void OnTriggerExit(Collider other){
		//If the player leaves the trigger zone...
		if (other.gameObject == player && roofFaded == true){
			// ... enable the roof.
			roofFaded = false;
			StartCoroutine(FadeTo (1.1f, 0.3f, 0.6f));
		}
	}

	IEnumerator FadeTo(float aValue, float iValue, float aTime)
	{
		float alpha = roofGameObject.GetComponent<Renderer>().material.color.a;
		float intensity = ambientLight.GetComponent<Light> ().intensity;

		for(float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			Color newColor = new Color (1,1,1, Mathf.Lerp (alpha, aValue, t));
			float newIntensity = Mathf.Lerp (intensity, iValue, t);
			roofGameObject.GetComponent<Renderer>().material.color = newColor;
			ambientLight.GetComponent<Light>().intensity = newIntensity;
			yield return null;
		}
	}

}
