using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {
	public GameObject strike;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boundary") {
			return;
		}
		if (other.tag == "Wall") {
			Instantiate (strike,transform.position,transform.rotation);
			Destroy (gameObject);
		}
		if (other.tag == "Enemy" && !other.isTrigger) {
			float damage = Random.Range (gameObject.GetComponent<BulletDamage>().minDamage,gameObject.GetComponent<BulletDamage>().maxDamage);
			other.gameObject.GetComponent<EnemyStats>().enemyHealth -= damage; 
			//Destroy (other.gameObject);
			Instantiate (strike,transform.position,transform.rotation);
			Destroy (gameObject);
		}
		if (other.tag == "Player" && !other.isTrigger) {
			float damage = Random.Range (gameObject.GetComponent<BulletDamage>().minDamage,gameObject.GetComponent<BulletDamage>().maxDamage);
			GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerStats>().playerHealth -= damage; 
			//Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}

}
