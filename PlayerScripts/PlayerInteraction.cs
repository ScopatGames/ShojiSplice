using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {

	public LayerMask layerMask;
	public float interactionRate = 0.3f;

	public float nextInteraction;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown ("Interact") && Time.time > nextInteraction){

			nextInteraction = Time.time + interactionRate;

			RaycastHit hitForward;
			Physics.Raycast (transform.position, transform.forward, out hitForward, 1.5f, layerMask);

			if(hitForward.collider != null && hitForward.collider.tag == Tags.npcInteraction){
				// Start talking to NPC...
				hitForward.collider.GetComponent<NpcInteractionController>().InitiateTalking();
			}


			RaycastHit hitDown;
			Physics.Raycast (transform.position, -transform.up, out hitDown, 1.0f, layerMask);

			if(hitDown.collider != null && hitDown.collider.tag == Tags.door){
				//Open door...
				DoorController doorController = hitDown.collider.GetComponentInParent<DoorController>();
				if(!doorController.isLocked){
					doorController.OpenDoor();
				}
				else{
					doorController.audioSource.clip = doorController.audioDoorLocked;
					doorController.audioSource.Play ();
				}
			}
			else if(hitDown.collider != null && hitDown.collider.tag == Tags.gate){
				// Start the gate...
				hitDown.collider.GetComponentInParent<GateController>().UseGate();
			}
			else if(hitDown.collider != null && hitDown.collider.tag == Tags.button){
				//Activate the button...
				hitDown.collider.GetComponent<PlayerEnableScript>().EnableScript();
			}
		}
	}
}
