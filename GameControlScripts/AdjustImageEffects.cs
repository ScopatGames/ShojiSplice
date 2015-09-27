using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class AdjustImageEffects : MonoBehaviour {

	[Header("Bloom:")]
	public bool adjustBloom;
	public bool bloomEnabled = true;
	public float bloomIntensity;
	public float bloomThreshold;
	private Bloom bloom;


	// Use this for initialization
	void Start () {
		if(adjustBloom){
			bloom = GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<Bloom>();
			bloom.bloomIntensity = bloomIntensity;
			bloom.bloomThreshold = bloomThreshold;
			if(bloom.enabled != bloomEnabled){
				bloom.enabled = bloomEnabled;
			}
		}
	}
	

}
