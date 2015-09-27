using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InitializeBackgroundImage : MonoBehaviour {

	public Sprite backgroundImage;
	public Color backgroundImageColor;

	private Image sourceImage;

	void Start () {
		sourceImage = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponentInChildren<Image> ();
		sourceImage.sprite = backgroundImage;
		sourceImage.color = backgroundImageColor;
	}
	

}
