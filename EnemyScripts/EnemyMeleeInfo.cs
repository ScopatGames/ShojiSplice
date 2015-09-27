using UnityEngine;
using System.Collections;

public class EnemyMeleeInfo : MonoBehaviour {

	public float attackRate;
	public bool noAmmoBool= false;
	private float nextAttack;
	public GameObject meleeSwing;
	public bool playSwing = false;

	
	Animator anim;
	int meleeHash = Animator.StringToHash("melee");
	int noAmmoTriggerHash = Animator.StringToHash("noAmmo");
	int noAmmoBoolHash = Animator.StringToHash("noAmmoBool");
	
	//private SphereCollider col;
	
	void Start(){
		anim = GetComponent<Animator> ();
	}
	
	
	void Update(){
		if(gameObject.GetComponentInParent<weaponIndex>().ammoCount == 0 && !noAmmoBool){
			
			/*Disable Collider part for enemies for now...
			//Set weapon collider back to unarmed...
			col = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<SphereCollider> ();
			col.enabled = false;
			*/
			noAmmoBool = true;
			anim.SetBool(noAmmoBoolHash, true); //Tells the animator that the enemy is out of ammo
			nextAttack = Time.time + attackRate; //Prevents auto melee attack on ammo running out
			anim.SetTrigger(noAmmoTriggerHash); //Signals the animator to move from Attack to Melee
		}
		if (gameObject.GetComponentInParent<EnemyAI>().isAttacking && Time.time > nextAttack && gameObject.GetComponentInParent<weaponIndex>().ammoCount == 0) {
			nextAttack = Time.time + attackRate;
			
			//Instantiate (meleeAudio, shotSpawn.position, shotSpawn.rotation);
			//Perform attack animation...
			anim.SetTrigger(meleeHash);

		}
		//play swing audio...
		if(playSwing){
			Instantiate(meleeSwing, transform.position, transform.rotation);
		}
		
	}
}
