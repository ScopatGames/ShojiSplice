using UnityEngine;
using System.Collections;


public class DoorController : MonoBehaviour {

	public bool isLocked;
	public bool opensForPatrol;
	public AudioClip audioDoorClose;
	public AudioClip audioDoorOpen;
	public AudioClip audioDoorLocked;

	public GameObject panel;
	public GameObject frameLocked;
	public GameObject frameUnlocked;
	public GameObject toggleCollider;



	[HideInInspector] public AudioSource audioSource;
	private bool isOpen = false;

	void Awake(){

		//On awake, check to see if the door is locked or unlocked and setup door...
		if(isLocked){
			LockDoor();
		}
		else{
			UnlockDoor ();
		}
		audioSource = GetComponent<AudioSource> ();
	}

	public void OpenDoor(){
		//Disable the panel and toggleCollider...
		if(!isOpen){
			audioSource.clip = audioDoorOpen;
			audioSource.Play ();
			panel.SetActive (false);
			isOpen = true;
		}
	}

	public void CloseDoor(){
		//Enable the panel and toggleCollider...
		if(isOpen){
			audioSource.clip = audioDoorClose;
			audioSource.Play ();
			panel.SetActive (true);
			isOpen = false;
		}
	}

	public void LockDoor(){
		CloseDoor ();
		frameLocked.SetActive(true);
		frameUnlocked.SetActive(false);
		toggleCollider.GetComponent<DoorTrigger> ().hitColliders.Clear ();
		//toggleCollider.SetActive (false);
		isLocked = true;
	}

	public void UnlockDoor(){
		frameLocked.SetActive(false);
		frameUnlocked.SetActive(true);
		//toggleCollider.SetActive (true);
		isLocked = false;
	}



}
