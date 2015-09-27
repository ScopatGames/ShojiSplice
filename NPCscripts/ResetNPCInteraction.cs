using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResetNPCInteraction : MonoBehaviour {

	[Header("Drag target parent GameObject that has Interaction Controller here:")]
	public GameObject specificNPC;

	//NpcInteractionController Values...
	[Header("If no message, but need to move, set True and isUrgent True")]
	public bool noMessageJustMove;
	[Header("Urgent Message Setup")]
	public bool messageIsUrgent;
	public Vector3 urgentMessageColliderPosition;
	public float urgentMessageColliderRadius;
	
	[Header("Standard Message Setup")]
	public Vector3 standardMessageColliderPosition;
	public float standardMessageColliderRadius = 0.35f;

	public TextAsset dialogueText;

	[Header("Target GameObject for Pre-Dialogue Event")]
	public GameObject targetGOpre;
	public string scriptNameToEnablePre;

	[Header("Target GameObject for Post-Dialogue Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;

	//UpdateNPCAI values
	[Header("Update NPCAI values")]
	public bool isOnPatrol;
	public Transform targetPosition;
	public GameObject targetGO2;
	public string scriptNameToEnable2;




	private NpcInteractionController npcIC; 
	private UpdateNPCAI updateNPCAI;

	void OnTriggerEnter(Collider other){
		if(specificNPC != null){
			if(other.gameObject == specificNPC){
				if(other.gameObject.GetComponentInChildren<NpcInteractionController>()){	
					GetComponent<SphereCollider>().enabled = false;
					npcIC = other.gameObject.GetComponentInChildren<NpcInteractionController>();
					
					npcIC.noMessageJustMove = noMessageJustMove;
					npcIC.messageIsUrgent = messageIsUrgent;
					npcIC.urgentMessageColliderPosition = urgentMessageColliderPosition;
					npcIC.urgentMessageColliderRadius = urgentMessageColliderRadius;
					npcIC.standardMessageColliderPosition = standardMessageColliderPosition;
					npcIC.standardMessageColliderRadius = standardMessageColliderRadius;
					npcIC.dialogueText = dialogueText;
					npcIC.targetGO = targetGO;
					npcIC.scriptNameToEnable = scriptNameToEnable;
					npcIC.targetGO2 = targetGOpre;
					npcIC.scriptNameToEnable2 = scriptNameToEnablePre;
					npcIC.targetEnabled = false;
					npcIC.InitializeStuff();
					
					
					if(other.gameObject.GetComponent<UpdateNPCAI>()){
						updateNPCAI = other.gameObject.GetComponent<UpdateNPCAI>();
						updateNPCAI.enabled = false;
						updateNPCAI.isOnPatrol = isOnPatrol;
						updateNPCAI.targetPosition = targetPosition;
						updateNPCAI.targetGO = targetGO2;
						updateNPCAI.scriptNameToEnable = scriptNameToEnable2;
					}
					
					
				}	
			}
		}
		else if (other.gameObject.tag == Tags.enemy || other.gameObject.tag == Tags.npc || other.gameObject.tag == Tags.neutral) { 
			if(other.gameObject.GetComponentInChildren<NpcInteractionController>()){	
				GetComponent<SphereCollider>().enabled = false;
				npcIC = other.gameObject.GetComponentInChildren<NpcInteractionController>();

				npcIC.noMessageJustMove = noMessageJustMove;
				npcIC.messageIsUrgent = messageIsUrgent;
				npcIC.urgentMessageColliderPosition = urgentMessageColliderPosition;
				npcIC.urgentMessageColliderRadius = urgentMessageColliderRadius;
				npcIC.standardMessageColliderPosition = standardMessageColliderPosition;
				npcIC.standardMessageColliderRadius = standardMessageColliderRadius;
				npcIC.dialogueText = dialogueText;
				npcIC.targetGO = targetGO;
				npcIC.scriptNameToEnable = scriptNameToEnable;
				npcIC.targetGO2 = targetGOpre;
				npcIC.scriptNameToEnable2 = scriptNameToEnablePre;
				npcIC.targetEnabled = false;
				npcIC.InitializeStuff();


				if(other.gameObject.GetComponent<UpdateNPCAI>()){
					updateNPCAI = other.gameObject.GetComponent<UpdateNPCAI>();
					updateNPCAI.enabled = false;
					updateNPCAI.isOnPatrol = isOnPatrol;
					updateNPCAI.targetPosition = targetPosition;
					updateNPCAI.targetGO = targetGO2;
					updateNPCAI.scriptNameToEnable = scriptNameToEnable2;
				}


			}
		}
	}
}
