using UnityEngine;
using System.Collections;

public class MeleeInfo : MonoBehaviour {

	public float attackRate;
	public bool noAmmoBool= false;
	[HideInInspector] public float nextAttack;
	private bool doNotRunAgain = false;
	public GameObject meleeAudio;
	public bool playSwing = false;

	Animator anim;
	int meleeHash = Animator.StringToHash("melee");
	int noAmmoHash = Animator.StringToHash("noAmmo");

	private SphereCollider col;

	void Start(){
		anim = GetComponent<Animator> ();
	}


	void Update(){
		if(gameObject.GetComponentInParent<weaponIndex>().ammoCount == 0 && !noAmmoBool && !doNotRunAgain){
			
			//Set weapon collider back to unarmed...
			col = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<SphereCollider> ();
			col.enabled = false;
			noAmmoBool = true;
			nextAttack = Time.time + attackRate; //Prevents auto melee attack on ammo running out
			anim.SetTrigger(noAmmoHash);

			doNotRunAgain = true;
		}
		if (Input.GetButton ("Attack") && !Input.GetButton ("Shift") && Time.time > nextAttack && gameObject.GetComponentInParent<weaponIndex>().ammoCount == 0) {
			nextAttack = Time.time + attackRate;

			//Instantiate (meleeAudio, shotSpawn.position, shotSpawn.rotation);
			//Perform attack animation...
			anim.SetTrigger(meleeHash);

		}
		if(playSwing){
			Instantiate (meleeAudio, transform.position, transform.rotation);
		}
	
	}


}
