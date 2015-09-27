using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {

	private Animator anim;
	private NavMeshAgent nav;

	void Start(){
		anim = GetComponent<Animator> ();
		nav = GetComponentInParent<NavMeshAgent> ();
	}

	void Update(){
		float move = nav.desiredVelocity.sqrMagnitude;
		anim.SetFloat ("speed", move);
		
		
	}
}
