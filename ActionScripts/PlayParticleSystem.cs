using UnityEngine;
using System.Collections;

public class PlayParticleSystem : MonoBehaviour {

	[Header("Bypass play on start...")]
	public bool doNotPlay;

	[Header("Target GameObject for Post-enabled Event")]
	public GameObject targetGO;
	public string scriptNameToEnable;

	private ParticleSystem ps;

	// Use this for initialization
	void OnEnable () {
		ps = GetComponent<ParticleSystem> ();
		if (!doNotPlay) {
			PlayPS ();		
		}

		//If post-action event exists, execute it...
		if(targetGO != null){
			(targetGO.GetComponent(scriptNameToEnable) as MonoBehaviour).enabled = true;
		}
	}
	
	public void PlayPS(){
		ps.Play ();
	}
}
