using UnityEngine;
using System.Collections;

public class RandomSetTrigger : MonoBehaviour {

	private Animator anim;
	private AnimatorControllerParameter[] acp;
	private int randomIndex;
	private AnimatorControllerParameterType paramType;

	// Randomly set a trigger for the Animator on the GameObject

	void Start () {
		anim = GetComponent<Animator> ();
		acp = anim.parameters;
		paramType = AnimatorControllerParameterType.Bool;
		while (paramType != AnimatorControllerParameterType.Trigger){
			randomIndex = Random.Range (0, acp.Length); //Random.Range is exclusive for the 2nd value
			paramType = acp[randomIndex].type;
		}
		anim.SetTrigger (acp [randomIndex].name);

	}

}
