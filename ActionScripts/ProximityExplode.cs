using UnityEngine;
using System.Collections;

public class ProximityExplode : MonoBehaviour {

	public GameObject explodeAnimation;
	public GameObject explodeAudio;
	public float cameraShakeMagnitude = 1.0f;
	public float maxCameraShakeDistance = 10.0f;
	public float sightRadius;
	public LayerMask colliderMask;
	
	SmoothCamera2D shakeCamera;
	float distanceFromCamera;
	float cameraShakeFactor;
	bool hasExploded = false;
	private Collider[] hitColliders;

	// Use this for initialization
	void Start () {
		shakeCamera = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<SmoothCamera2D> ();
	}

	void Update(){
		if(!hasExploded){
			hitColliders = Physics.OverlapSphere(transform.position, sightRadius, colliderMask);
			foreach (Collider other in hitColliders) {
				if(other.tag == Tags.player){
					Explode();
					Invoke ("DestroyThisGO", 0.1f);
					hasExploded = true;
				}
			}
		}
	}
	
	

	void Explode(){
		GetComponentInChildren<SphereCollider> ().enabled = true;
		GetComponentInChildren<DestroyByExplosion> ().ExplodeAndDamage ();
		GetComponentInChildren<SpriteRenderer> ().enabled = false;
		Instantiate (explodeAudio, transform.position, transform.rotation);
		Instantiate (explodeAnimation, transform.position, Quaternion.AngleAxis (Random.Range (0f, 360f),Vector3.up));
		
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
	}

	void DestroyThisGO(){
		Destroy (gameObject);
	}
}
