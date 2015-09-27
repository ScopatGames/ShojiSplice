using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectEmitter : MonoBehaviour {
	public List<Rigidbody> gameObjectToEmit = new List<Rigidbody>();
	public int minNumberObjects;
	public int maxNumberObjects;
	public float minEmitForce;
	public float maxEmitForce;

	private int numberObjects;
	private Vector3 direction;
	private float emitForce;

	// Use this for initialization
	void Start () {
		numberObjects = Random.Range (minNumberObjects, maxNumberObjects);
		for(int i=0; i<numberObjects; i++){
			direction = Random.insideUnitSphere;
			direction.y = 0f;
			emitForce = Random.Range(minEmitForce, maxEmitForce);

			//Randomly pick index...
			int goIndex = Random.Range (0, gameObjectToEmit.Count);
			Rigidbody go = Instantiate(gameObjectToEmit[goIndex], transform.position, transform.rotation) as Rigidbody;
			direction = emitForce*direction;
			go.AddForce(direction);
		}
	}

}
