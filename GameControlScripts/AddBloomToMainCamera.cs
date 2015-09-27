using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class AddBloomToMainCamera : MonoBehaviour {
	public bool activateOnStart;
	public float bloomIntensity;
	public float bloomThreshold;
	/*public enum BloomScreenBlendMode
	{
		Screen = 0,
		Add = 1,
	}*/
	public BloomScreenBlendMode bloomScreenBlendMode = BloomScreenBlendMode.Add;

	private GameObject mainCamera;



	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag (Tags.mainCamera);
		if (activateOnStart) {
			AddBloomComponent();		
		}
	
	}
	

	public void AddBloomComponent () {
		mainCamera.AddComponent<Bloom> ();
		Bloom bloom = mainCamera.GetComponent<Bloom> ();
		bloom.bloomIntensity = bloomIntensity;
		bloom.bloomThreshold = bloomThreshold;
		//bloom.screenBlendMode = bloomScreenBlendMode; Not working
	}

	public void RemoveBloomComponent(){
	
	}
}
