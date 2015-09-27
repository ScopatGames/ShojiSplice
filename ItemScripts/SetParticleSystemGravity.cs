using UnityEngine;
using System.Collections;

public class SetParticleSystemGravity : MonoBehaviour {
	public float particleGravityModifier = 0f;
	public float waitTime = 0f;


	// Use this for initialization
	void Start () {
		StartCoroutine(SetGravity());
	}

	IEnumerator SetGravity(){
		yield return new WaitForSeconds(waitTime);
		ParticleSystem ps = GetComponent<ParticleSystem>();
		ps.gravityModifier = particleGravityModifier;
	}
}
