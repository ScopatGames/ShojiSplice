using UnityEngine;
using System.Collections;

public class ThrowWeapon : MonoBehaviour {

	public Transform shotSpawn;
	public Rigidbody weapon;

	void Update () {
		if (Input.GetButtonDown ("Attack")&& Input.GetButton ("Shift")) {
			Rigidbody newThrown = Instantiate (weapon, shotSpawn.position, shotSpawn.rotation) as Rigidbody;
			newThrown.GetComponent<Rigidbody>().velocity = transform.parent.GetComponent<Rigidbody>().velocity;
			//audio.Play();
			transform.parent.gameObject.GetComponent<PlayerWeaponChange>().hasSecondaryWeap = false;

			Destroy (gameObject);

		}
	}
}
