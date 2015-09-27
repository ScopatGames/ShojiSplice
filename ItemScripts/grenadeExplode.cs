using UnityEngine;
using System.Collections;

public class grenadeExplode : MonoBehaviour {

	public float timeFuse;
	public GameObject grenadeAudio;
	public GameObject grenadeAnimation;
	public GameObject grenadeParticleSystem;
	public Vector3 particleSystemRotationVector = Vector3.zero;
	public float cameraShakeMagnitude = 1.0f;
	public float maxCameraShakeDistance = 10.0f;


	SmoothCamera2D shakeCamera;
	float distanceFromCamera;
	float cameraShakeFactor;
	float timer = 0f;

	public void Explode(){
		GetComponentInChildren<SphereCollider> ().enabled = true;
		GetComponentInChildren<DestroyByExplosion> ().ExplodeAndDamage ();

		//Disable the renderer...
		if (GetComponent<MeshRenderer> ()) {
			GetComponent<MeshRenderer> ().enabled = false;
		}
		else if(GetComponentInChildren<SpriteRenderer>()){
			GetComponentInChildren<SpriteRenderer>().enabled = false;
		}

		if(grenadeAudio){
			Instantiate (grenadeAudio, transform.position, transform.rotation);
		}
		if(grenadeAnimation){
			Instantiate (grenadeAnimation, transform.position, Quaternion.AngleAxis (Random.Range (0f, 360f),Vector3.up));
		}
		if(grenadeParticleSystem){
			Instantiate (grenadeParticleSystem, transform.position, Quaternion.Euler (0f, transform.eulerAngles.y, 0f)*Quaternion.Euler(particleSystemRotationVector));
		}

		//Camera Shake...
		distanceFromCamera = new Vector2 (shakeCamera.transform.position.x - transform.position.x, shakeCamera.transform.position.z - transform.position.z ).magnitude;
		if(distanceFromCamera <= 1.0f){
			cameraShakeFactor = 1.0f;
		}
		else if (distanceFromCamera < maxCameraShakeDistance) {
			cameraShakeFactor = 1.0f/(distanceFromCamera*distanceFromCamera)-1.0f/(maxCameraShakeDistance*maxCameraShakeDistance);		
		}
		else {
			cameraShakeFactor = 0f;
		}
		shakeCamera.shakeCamera (cameraShakeFactor*cameraShakeMagnitude, transform.position);
		Destroy (gameObject);
	}

	void Awake(){
		shakeCamera = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<SmoothCamera2D> ();

	}


	void Update(){
		if(timeFuse > 0f){
			if(timer > timeFuse){
				Explode();
			}
			timer += Time.deltaTime;
		}
	}



}
