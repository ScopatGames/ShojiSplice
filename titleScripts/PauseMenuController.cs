using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class PauseMenuController : MonoBehaviour {

	private PersistentData persistentData;
	private SceneFadeInOut sceneFadeInOut;
	private GameObject mainCamera;
	private Animator anim;
	[HideInInspector] public bool isPaused;
	private float interactionRate = 0.3f;
	private bool canInteract = true;
	private float interactionTimer = 0f;
	private GameObject player;
	private ShootBullet playerShootBullet;
	private bool playerSBenabledBeforePause;
	private MeleeInfo playerMeleeInfo;
	private bool playerMIenabledBeforePause;

	// Use this for initialization
	void Start () {
		sceneFadeInOut = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponentInChildren<SceneFadeInOut> ();
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
		mainCamera = GameObject.FindGameObjectWithTag (Tags.mainCamera);
		player = GameObject.FindGameObjectWithTag (Tags.player);
		anim = GetComponentInParent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!canInteract){
			interactionTimer += Time.deltaTime;
			if(interactionTimer> interactionRate){
				canInteract = true;
			}
		}

		if(Input.GetKeyDown(KeyCode.Escape) &&  Application.loadedLevel != 1 && canInteract){
			if(!isPaused){
				PauseGame();
			}
			canInteract = false;
			interactionTimer = 0f;
		}

	}

	public void PauseGame(){
		isPaused = true;
		anim.SetTrigger ("triggerOpenPauseMenu");
		mainCamera.GetComponent<Blur> ().enabled = true;
		Time.timeScale = 0f;
		//Turn off the ability to Shoot and Melee Attack
		if(player){
			if(player.GetComponentInChildren<ShootBullet>()){
				playerShootBullet = player.GetComponentInChildren<ShootBullet>();
				if(playerShootBullet.enabled){
					playerShootBullet.enabled = false;
					playerSBenabledBeforePause = true;
				}
				else{
					playerSBenabledBeforePause = false;
				}
			}
			if(player.GetComponentInChildren<MeleeInfo>()){
				playerMeleeInfo = player.GetComponentInChildren<MeleeInfo>();
				if(playerMeleeInfo.enabled){
				   	playerMeleeInfo.enabled = false;
					playerMIenabledBeforePause = true;
				}
				else{
					playerMIenabledBeforePause = false;
				}
			}
		}
	}

	public void UnpauseGame(){
		isPaused = false;
		anim.SetTrigger ("triggerClosePauseMenu");
		mainCamera.GetComponent<Blur> ().enabled = false;
		//Turn on the ability to shoot and melee attack
		if(player){
			if(player.GetComponentInChildren<ShootBullet>()){
				if(playerSBenabledBeforePause){
					playerShootBullet.enabled = true;
				}
			}
			if(player.GetComponentInChildren<MeleeInfo>()){
				if(playerMIenabledBeforePause){
					playerMeleeInfo.enabled = true;
				}
			}
		}
		persistentData.Save ();
		Time.timeScale = 1f;

	}

	public void MainMenu(){
		isPaused = false;
		if(persistentData.localScenePersistentContainers.Count > 0){
			foreach(GameObject go in persistentData.localScenePersistentContainers){
				Destroy (go);
			}
		}
		persistentData.ResetDefaults();
		sceneFadeInOut.LoadScene (1);
		mainCamera.GetComponent<Blur> ().enabled = false;
		persistentData.Save ();
		Time.timeScale = 1f;
	}

	public void QuitGame(){
		isPaused = false;
		persistentData.Save ();
		Time.timeScale = 1f;
		mainCamera.GetComponent<Blur> ().enabled = false;
		Application.Quit ();
	}
}
