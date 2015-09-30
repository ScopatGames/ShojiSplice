using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NpcInteractionController : MonoBehaviour {

	[Header("If no message, but need to move, set True and isUrgent True")]
	public bool noMessageJustMove;

	[Header("Urgent Message Setup")]
	public bool messageIsUrgent;
	public Vector3 urgentMessageColliderPosition;
	public float urgentMessageColliderRadius;

	[Header("Standard Message Setup")]
	public Vector3 standardMessageColliderPosition;
	public float standardMessageColliderRadius = 0.35f;

	[Header("Character Name and Dialogue Text")]
	public string characterName;
	public TextAsset dialogueText;


	[Header("Dialogue Canvas Info")]
	public Text dialogueTextField;
	public Text characterNameText;
	public Scrollbar scrollBar;

	[Header("Target GameObject for Pre-Dialogue Event")]
	public GameObject targetGO2;
	public string scriptNameToEnable2;
	[HideInInspector] public bool targetEnabled2 = false;

	[Header("Target GameObject for Post-Dialogue Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;
	[HideInInspector] public bool targetEnabled = false;

	[HideInInspector] public bool cancelButtonClicked = false;

	private string[] dialogueTextParsed;
	private SphereCollider col;
	private ParticleSystem indicatorParticleSystem;
	private Rigidbody playerRB;
	private NavMeshAgent nav;
	private EnemyAI enemyAI;
	private bool isTalking = false;
	private int currentLine = 0;
	private Transform playerTransform;
	private float interactionRate;
	private Animator[] animatorArrayNPC;
	private PlayerAnimator[] playerAnimatorArray;
	private Quaternion originalRotation;
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

		//Initialize interaction collider on awake...
		col = GetComponent<SphereCollider>();
		SetColliderPosition ();

		//Parse the text file into the array, dialogueTextParsed...
		dialogueTextParsed = dialogueText.text.Split('\n');

		if(messageIsUrgent && !noMessageJustMove){
			indicatorParticleSystem.Play ();
		}
		playerTransform = GameObject.FindGameObjectWithTag (Tags.player).transform;
		
		playerRB = playerTransform.GetComponent<Rigidbody>();
		
		
		//Initialize NPC NavMeshAgent and EnemyAI...
		nav = gameObject.GetComponentInParent<NavMeshAgent> ();
		enemyAI = gameObject.GetComponentInParent<EnemyAI> ();
		
		
		
		
		
		//Get interaction rate from Player..
		interactionRate = GameObject.FindGameObjectWithTag (Tags.player).GetComponent<PlayerInteraction> ().interactionRate;
		
		//Get animator info...
		animatorArrayNPC = transform.parent.GetComponentsInChildren<Animator> ();
		
		
		//Initialize dialoguePanelOpenClose...
		dialoguePanelOpenClose = canvas.GetComponentInChildren<DialoguePanelOpenClose> ();
	}
	
	public void SetColliderPosition(){
		if(messageIsUrgent){
			col.center = urgentMessageColliderPosition;
			col.radius = urgentMessageColliderRadius;
		}
		else{
			col.center = standardMessageColliderPosition;
			col.radius = standardMessageColliderRadius;
		}
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

			foreach (Animator anim in animatorArrayNPC){
				if(anim.tag != Tags.exempt){
					anim.Play("Default", 0, 0.0f);
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

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == Tags.player && messageIsUrgent) {
			//If the player enters and the message is urgent, then start talking...

			InitiateTalking ();
		}
	}



	public void InitiateTalking(){
		//Send this controller info to the dialogue panel...
		dialoguePanelOpenClose.activeNpcInteractionController = this;

		//Check if there is a pre-dialogue event...
		if (targetGO2 != null && !targetEnabled2) {
			//Execute event...
			(targetGO2.GetComponent(scriptNameToEnable2) as MonoBehaviour).enabled = true;
			targetEnabled2 = true;
		}

		if(messageIsUrgent){
			messageIsUrgent = false;
			SetColliderPosition();
		}

		if (noMessageJustMove) {
			transform.parent.GetComponentInParent<UpdateNPCAI>().enabled = true;
		}
		else{

			playerAnimatorArray = playerTransform.GetComponentsInChildren<PlayerAnimator> ();
			animatorArrayNPC = transform.parent.GetComponentsInChildren<Animator> ();

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
			col.enabled = false;
			//playerTransform.GetComponent<PlayerInteraction> ().enabled = false;
			playerTransform.GetComponent<LookAtGameObject> ().target = transform.parent;

			foreach (PlayerAnimator pAnim in playerAnimatorArray) {
				pAnim.pause = true;
			}

			//Disable player feet animation...
			playerTransform.GetComponentInChildren<PlayerFeetAnimator> ().pause = true;

			//disable NPC movement, and have NPC look at player...
			if(nav.enabled){
				nav.Stop ();
				enemyAI.enabled = false;
			}

			originalRotation = transform.parent.rotation;
		
			transform.parent.GetComponent<LookAtGameObject>().target = playerTransform;

			//Disable camera movement...
			SmoothCamera2D smoothCamera2D = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<SmoothCamera2D> ();
			smoothCamera2D.cursor = transform;
			smoothCamera2D.lagTime = 3f;

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
	}

	public void EndTalking(){
		isTalking = false;
		currentLine = 0;
		cancelButtonClicked = false;
		dialoguePanelOpenClose.activeNpcInteractionController = null;

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

		col.enabled = true;
		//playerTransform.GetComponent<PlayerInteraction> ().enabled = true;
		playerTransform.GetComponent<PlayerInteraction> ().nextInteraction = Time.time + interactionRate;
		playerTransform.GetComponent<LookAtGameObject> ().target = cursorGO.transform;
		PlayerAnimator[] playerAnimatorArray = playerTransform.GetComponentsInChildren<PlayerAnimator> ();
		foreach (PlayerAnimator pAnim in playerAnimatorArray) {
			pAnim.pause = false;		
		}
		//Enable player feet animation...
		playerTransform.GetComponentInChildren<PlayerFeetAnimator> ().pause = false;

		//enable NPC movement, and have NPC stop looking at player...
		if(nav.enabled){
			nav.Resume ();
			enemyAI.enabled = true;
		}

		transform.parent.GetComponent<LookAtGameObject>().target = null;
		transform.parent.rotation = originalRotation;
		scrollBar.size = 1.0f;

		//Enable camera movement...
		SmoothCamera2D smoothCamera2D = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<SmoothCamera2D> ();
		smoothCamera2D.cursor = cursorGO.transform;
		smoothCamera2D.ChangeLag(0.5f, 12f);

		//Close dialogue panel and delete character name & text field...
		dialoguePanelOpenClose.CloseDialogue ();
		characterNameText.text = null;
		dialogueTextField.text = null;
		scrollBar.value = 1.0f;


		//Disable particle system indicating dialogue...
		indicatorParticleSystem.Stop ();
		indicatorParticleSystem.playOnAwake = false;

		//If post-dialogue event exists, execute it...
		if(!targetEnabled && targetGO != null){
			(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
			targetEnabled = true;
		}

	}

}


