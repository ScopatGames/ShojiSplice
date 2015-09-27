using UnityEngine;
using System.Collections;

public class WeaponCollider : MonoBehaviour {

	public bool colliderEnabled = false;
	public float colliderRadius = 0.0f;
	public float centerX = 0.0f;
	public float centerY = 0.0f;
	public float centerZ = 0.0f;

	private SphereCollider col;
	private Vector3 newCenter;

	void Start(){
		//get the player sphere collider...
		col = GameObject.FindGameObjectWithTag (Tags.player).GetComponentInChildren<SphereCollider> ();
		col.enabled = colliderEnabled;
		col.radius = colliderRadius;
		newCenter = new Vector3 (centerX, centerY, centerZ);
		col.center = newCenter;
	}
}
