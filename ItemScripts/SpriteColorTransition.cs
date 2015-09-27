using UnityEngine;
using System.Collections;

public class SpriteColorTransition : MonoBehaviour {

	public Color startColor;
	public Color endColor;
	public float transitionTime;

	private SpriteRenderer spriteRenderer;
	private float t = 0f; //lerp control variable

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.color = startColor;
	}
	

	void Update () {
		spriteRenderer.color = Color.Lerp (startColor, endColor, t);
		if(t<1f){
			t += Time.deltaTime/transitionTime;
		}
	}
}
