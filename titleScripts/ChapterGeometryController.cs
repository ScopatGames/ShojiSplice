using UnityEngine;
using System.Collections;

public class ChapterGeometryController : MonoBehaviour {

	public GameObject amplifyGlowGameObject;
	public Transform effectsLocation;
	public GameObject effects;
	public string chapterTitle;
	public string chapterDescription;

	private LoadOnClick loadOnClick;

	void Start(){
		loadOnClick = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponent<LoadOnClick> ();
	}

	public void ChapterClickEffects(int scene){
		Instantiate (effects, effectsLocation.position, effectsLocation.rotation);
		StartCoroutine (DelayedSceneChange (scene));
	}

	public void ChapterEnterEffects(){
		amplifyGlowGameObject.SetActive (true);
	}

	public void ChapterExitEffects(){
		amplifyGlowGameObject.SetActive (false);
	}

	IEnumerator DelayedSceneChange(int scene){
		yield return new WaitForSeconds(3.16f);
		loadOnClick.LoadScene (scene);
	}
}
