using UnityEngine;
using System.Collections;

public class EnableScriptByGameobjectName : MonoBehaviour {

	public string gameobjectName;
	public string scriptNameToEnable;
	private GameObject goToEnable;

	void OnEnable(){
		goToEnable = GameObject.Find (gameobjectName);
		(goToEnable.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
	}
}
