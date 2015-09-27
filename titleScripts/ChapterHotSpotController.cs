using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ChapterHotSpotController : MonoBehaviour {
	public int hotSpotIndex;
	public Text chapterTitleField;
	public Text chapterDescriptionField;
	public GameObject chapterGeometry;

	private PersistentData persistentData;
	private ChapterController chapterController;
	private ChapterGeometryController chapterGeometryController;
	private bool isClickable=false;



	// Use this for initialization
	void Start () {
		chapterController = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<ChapterController> ();
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
		if (hotSpotIndex < persistentData.chaptersUnlocked) {
			if(hotSpotIndex != -1){
				//-1 is the prologue and is already set...
				chapterGeometry = chapterController.chapterGeometry[hotSpotIndex];
			}
			chapterGeometryController = chapterGeometry.GetComponent<ChapterGeometryController>();
			isClickable = true;
		}
		else{
			chapterGeometry = chapterController.planetXes[hotSpotIndex];
			chapterGeometryController = chapterGeometry.GetComponent<ChapterGeometryController>();
			//GetComponent<EventTrigger>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChapterGeometryEnterEffects(){
		if(!chapterController.levelSelected){
			chapterGeometryController.ChapterEnterEffects ();
		}
	}

	public void ChapterGeometryExitEffects(){
		if(!chapterController.levelSelected){
			chapterGeometryController.ChapterExitEffects ();
		}
	}

	public void ChapterGeometryClickEffects(int scene){
		if(!chapterController.levelSelected && isClickable){
			chapterGeometryController.ChapterClickEffects (scene);
			chapterController.levelSelected = true;
		}
	}

	public void ChangeChapterTitles(){
		if(!chapterController.levelSelected){
			chapterTitleField.text = chapterGeometryController.chapterTitle;
			chapterDescriptionField.text = chapterGeometryController.chapterDescription;
		}
	}

	public void RemoveChapterTitles(){
		if(!chapterController.levelSelected){
			chapterTitleField.text = "";
			chapterDescriptionField.text = "";
		}
	}

}
