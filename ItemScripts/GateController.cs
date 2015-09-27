using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour {

	public bool isActive;
		
	public GameObject activateGate;
	public GameObject toggleCollider;
	public GameObject gateEffects;
	[Header("Next relative scene, e.g. 1 for thisScene+1")]
	public int nextScene = 0;
	public int spawnLocation = 0;

	private GameObject player;
	//private SmoothCamera2D shakeCam;
	private SceneFadeInOut sceneFadeInOut;
	private PersistentData persistentData;
	
	
	void Awake(){
		//On awake, check to see if the gate is active or inactive and setup gate...
		if(isActive){
			ActivateGate();
		}
		else{
			DeactivateGate ();
		}

		//shakeCam = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<SmoothCamera2D> ();
		player = GameObject.FindGameObjectWithTag (Tags.player);
		sceneFadeInOut = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<SceneFadeInOut> ();
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
	}
	
	public void ActivateGate(){
		//Enable the gate and toggleCollider...
		activateGate.SetActive (true);
		toggleCollider.SetActive (true);
		isActive = true;
	}
	
	public void DeactivateGate(){
		//Disable the gate and toggleCollider...
		activateGate.SetActive (false);
		toggleCollider.SetActive (false);
		isActive = false;
	}
	
	public void UseGate(){
		gateEffects.SetActive (true);
		player.GetComponent<MoveViaInputAxis> ().enabled = false;
		Animator[] anim = player.GetComponentsInChildren<Animator> ();
		foreach (Animator a in anim) {
			a.enabled = false;		
		}
		Invoke ("TeleportPlayer", 3.16f);

	}

	void TeleportPlayer(){
		//shakeCam.player = transform;
		//shakeCam.shakeCamera (0.3f);
		Destroy (player);
		persistentData.nextSpawnPoint = spawnLocation;
		sceneFadeInOut.LoadScene (sceneFadeInOut.thisScene+nextScene);

	}


	

}
