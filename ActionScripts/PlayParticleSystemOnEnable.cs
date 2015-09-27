using UnityEngine;
using System.Collections;

public class PlayParticleSystemOnEnable : MonoBehaviour {

	void OnEnable(){
		GetComponent<ParticleSystem> ().Play ();
	}
}
