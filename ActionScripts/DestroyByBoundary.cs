using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		Destroy (other.gameObject);
	}
}
