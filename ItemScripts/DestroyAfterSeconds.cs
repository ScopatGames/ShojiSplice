using UnityEngine;
using System.Collections;


public class DestroyAfterSeconds : MonoBehaviour {
	public float delaySeconds;
	public GameObject spawnGameObjectOnDestroy;

	private float delayTimer=0f;

	void FixedUpdate(){
		if(delayTimer < delaySeconds){
			delayTimer += Time.deltaTime; 
		}
		else {
			if(spawnGameObjectOnDestroy){
				Instantiate(spawnGameObjectOnDestroy, transform.position, transform.rotation);
			}
			Destroy (gameObject);
		}
	}


}
