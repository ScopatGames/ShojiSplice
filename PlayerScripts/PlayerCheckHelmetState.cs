using UnityEngine;
using System.Collections;

public class PlayerCheckHelmetState : MonoBehaviour {
	public bool startSceneWithHelmetEquipped = false;

	private PersistentData persistentData;
	private Animator playerHeadAnimator;
	private bool currentValue;
	private string triggerValue;


	void Start () {
		//initialize persistentData...
		persistentData = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<PersistentData> ();
		//initialize player head animator...
		playerHeadAnimator = GetComponent<Animator> ();
		currentValue = playerHeadAnimator.GetBool ("helmetEquipped");

	
		switch (persistentData.playerHelmetEquipped){
		
		//if persistentData helmet state is set to 0 ("default")
		case 0:
			//then set the helmet state based on the starting scene state...
			if(startSceneWithHelmetEquipped && !currentValue){
				playerHeadAnimator.SetTrigger ("equipHelmetNoEffects");
			}
			else if(!startSceneWithHelmetEquipped && currentValue){
				playerHeadAnimator.SetTrigger ("removeHelmet");
			}
			break;
		
		//if persistentData helmet state is set to 1 ("not equipped");
		case 1:
			if(currentValue){
				playerHeadAnimator.SetTrigger ("removeHelmet");
			}
			break;
		
		//if persistentData helmet state is set to 2 ("equipped");
		case 2:
			if(!currentValue){
				playerHeadAnimator.SetTrigger ("equipHelmetNoEffects");
			}
			break;
		default:
			break;
		}



	}
	

}
