using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractionController : MonoBehaviour {

	public bool noPlayerInScene;
	public bool cameraTargetPlayer;

	[Header("Character Name and Dialogue Text")]
	public string characterName;
	public TextAsset dialogueText;
	[Header("Dialogue Canvas Info")]
	public Text dialogueTextField;
	public Text characterNameText;
	public Scrollbar scrollBar;

	[Header("Character Name and Dialogue Text")]
	public bool hasParticleSystemIndicator;
	public bool playParticleSystemIndicatorBeforeEnabled;

	[Header("Target GameObject for Post-Dialogue Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool targetEnabled = false;

	[HideInInspector] public bool cancelButtonClicked;
	
	private string[] dialogueTextParsed;
	private ParticleSystem indicatorParticleSystem;
	private Rigidbody playerRB;
	private bool isTalking = false;
	private int currentLine = 0;
	private Transform playerTransform;
	private float interactionRate = 0.3f;
	private PlayerAnimator[] playerAnimatorArray;
	private DialoguePanelOpenClose dialoguePanelOpenClose;
	private bool updateText = true;
	private bool endParagraph = false;
	private int noOfLines = 0;
	private float textLineTimer = 0f;
	private float textLineTimeIncrement = 0.1f;
	private GameObject cursorGO;
	
	
	void Start(){
		InitializeStuff ();
	}
	
	public void InitializeStuff(){
		//Initialize Canvas items...
		GameObject canvas = GameObject.FindGameObjectWithTag (Tags.canvas);
		Text[] allText = canvas.GetComponentsInChildren<Text> ();
		foreach (Text txt in allText){
			if(txt.tag == Tags.dialogueText){
				dialogueTextField = txt;
			}
			else if(txt.tag == Tags.characterNameText){
				characterNameText = txt;
			}
		}
		Scrollbar[] allScrollBars = canvas.GetComponentsInChildren<Scrollbar> ();
		foreach (Scrollbar sb in allScrollBars){
			if(sb.tag == Tags.scrollBar){
				scrollBar = sb;
			}
		}

		//Initialize particle system for Interaction Indicator...
		indicatorParticleSystem = GetComponentInChildren<ParticleSystem> ();
		
		cursorGO = GameObject.FindGameObjectWithTag (Tags.cursor);

		//Parse the text file into the array, dialogueTextParsed...
		dialogueTextParsed = dialogueText.text.Split('\n');
		
		if(hasParticleSystemIndicator && playParticleSystemIndicatorBeforeEnabled){
			indicatorParticleSystem.Play ();
		}

		if(!noPlayerInScene){
			GameObject playerGO = GameObject.FindGameObjectWithTag(Tags.player);
			
			playerTransform = playerGO.transform;
			
			playerRB = playerTransform.GetComponent<Rigidbody>();
			
			//Get interaction rate from Player..
			interactionRate = playerGO.GetComponent<PlayerInteraction> ().interactionRate;
		}
		else{
			//if no player in scene, then the camera cannot target the player...
			cameraTargetPlayer = false;
		}
		
		//Initialize dialoguePanelOpenClose...
		dialoguePanelOpenClose = canvas.GetComponentInChildren<DialoguePanelOpenClose> ();
	}

	void Update () {
		if(dialogueTextField == null){
			InitializeStuff();
		}

		if(isTalking){
			//Display current dialogue line...
			if(updateText){
				if(!endParagraph){
					if(textLineTimer >= textLineTimeIncrement){
						if(currentLine < dialogueTextParsed.Length -1){
							dialogueTextField.text += dialogueTextParsed[currentLine] + '\n';
						}
						else{
							dialogueTextField.text += dialogueTextParsed[currentLine];
						}
						noOfLines++;
						if(dialogueTextParsed[currentLine]== "...\r" || dialogueTextParsed[currentLine]== "---" || noOfLines == 4){
							endParagraph = true;
							updateText = false;
						}
						scrollBar.value = 0.0f;
						currentLine++;
						textLineTimer = 0f;
					}
					textLineTimer += Time.deltaTime;
				}
			}

			if(Input.GetButtonDown ("Interact") && endParagraph){
				if(currentLine < dialogueTextParsed.Length -1){
					updateText = true;
					noOfLines = 0;
					endParagraph = false;
				}
				else{
					updateText = true;
					noOfLines = 0;
					endParagraph = false;
					scrollBar.size = 1.0f;
					scrollBar.value = 1.0f;
					EndTalking();
					
				}
			}
			else if(Input.GetButtonDown ("Skip") || cancelButtonClicked){
				updateText = true;
				noOfLines = 0;
				endParagraph = false;
				scrollBar.size = 1.0f;
				scrollBar.value = 1.0f;
				EndTalking();
			}
		}
	}

	public void InitiateTalking(){
		//Send this controller info to the dialogue panel...
		dialoguePanelOpenClose.activeInteractionController = this;

		if (hasParticleSystemIndicator && !playParticleSystemIndicatorBeforeEnabled) {
			indicatorParticleSystem.Play ();		
		}

		if(!noPlayerInScene){

			playerAnimatorArray = playerTransform.GetComponentsInChildren<PlayerAnimator> ();

			//disable player movement, disable animations, and have player look at NPC...
			playerTransform.GetComponent<MoveViaInputAxis> ().enabled = false;
			playerTransform.GetComponent<Rigidbody>().velocity = Vector3.zero;
			playerRB.isKinematic = true;
			
			
			//disable player weapons...
			if(playerTransform.GetComponentInChildren<ShootBullet> ()){
				playerTransform.GetComponentInChildren<ShootBullet> ().enabled = false;
			}
			if(playerTransform.GetComponentInChildren<MeleeInfo> ()){
				playerTransform.GetComponentInChildren<MeleeInfo> ().enabled = false;
			}
			if(playerTransform.GetComponentInChildren<ThrowWeapon> ()){
				playerTransform.GetComponentInChildren<ThrowWeapon> ().enabled = false;
			}
			
			interactionRate = playerTransform.GetComponent<PlayerInteraction> ().interactionRate;


			playerTransform.GetComponent<LookAtGameObject> ().target = transform;

			foreach(PlayerAnimator pAnim in playerAnimatorArray){
				pAnim.pause = true;
			}
			
			//Disable player feet animation...
			playerTransform.GetComponentInChildren<PlayerFeetAnimator> ().pause = true;
				
			//Disable camera movement... Set target...
			SmoothCamera2D smoothCamera2D = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<SmoothCamera2D> ();
			smoothCamera2D.ChangeLag(0.01f, 3f);

			if(cameraTargetPlayer){
				smoothCamera2D.cursor = playerTransform.transform;
			}
			else{
				smoothCamera2D.cursor = transform;
			}
		}
		
		//Set bool to begin talking...
		textLineTimer = 0f;
		isTalking = true;
		currentLine = 0;

		
		
		//Open dialogue panel and fill character name...
		dialoguePanelOpenClose.OpenDialogue ();
		characterNameText.text = characterName;
		dialogueTextField.text = null;
		updateText = true;
		
	}
	
	public void EndTalking(){
		isTalking = false;
		currentLine = 0;
		cancelButtonClicked = false;
		dialoguePanelOpenClose.activeInteractionController = null;
		
		if(!noPlayerInScene){
			//Enable player movement, enable animations, and have player look at cursor...
			playerTransform.GetComponent<MoveViaInputAxis> ().enabled = true;
			playerRB.isKinematic = false;
			
			//Enable player weapons...
			if(playerTransform.GetComponentInChildren<ShootBullet> ()){
				playerTransform.GetComponentInChildren<ShootBullet> ().enabled = true;
			}
			if(playerTransform.GetComponentInChildren<MeleeInfo> ()){
				playerTransform.GetComponentInChildren<MeleeInfo> ().enabled = true;
			}
			if(playerTransform.GetComponentInChildren<ThrowWeapon> ()){
				playerTransform.GetComponentInChildren<ThrowWeapon> ().enabled = true;
			}
			playerTransform.GetComponent<PlayerInteraction>().nextInteraction = Time.time + interactionRate;
			playerTransform.GetComponent<LookAtGameObject> ().target = cursorGO.transform;

			PlayerAnimator[] playerAnimatorArray = playerTransform.GetComponentsInChildren<PlayerAnimator> ();
			foreach (PlayerAnimator pAnim in playerAnimatorArray) {
				pAnim.pause = false;
			}
			//Enable player feet animation...
			playerTransform.GetComponentInChildren<PlayerFeetAnimator> ().pause = false;
		
			//Enable camera movement...
			SmoothCamera2D smoothCamera2D = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<SmoothCamera2D> ();
			smoothCamera2D.cursor = cursorGO.transform;
			smoothCamera2D.ChangeLag(0.5f, 12f);

		}
		
		scrollBar.size = 1.0f;
					
		//Close dialogue panel and delete character name & text field...
		dialoguePanelOpenClose.CloseDialogue ();
		characterNameText.text = null;
		dialogueTextField.text = null;
		scrollBar.value = 1.0f;
		
		
		//Disable particle system indicating dialogue...
		if(hasParticleSystemIndicator){
			indicatorParticleSystem.Stop ();
			indicatorParticleSystem.playOnAwake = false;
		}
		
		//If post-dialogue event exists, execute it...
		if(!targetEnabled && targetGO != null){
			(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
			targetEnabled = true;
		}
		
	}
}
