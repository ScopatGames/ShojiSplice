using UnityEngine;
using System.Collections;

public class InstantiateSpritePrefab : MonoBehaviour {
	public GameObject prefabToInstantiate;
	public Vector3 transformOffset = new Vector3(0f, 0f, 0f);
	public Vector3 rotationOffset = new Vector3(0f, 0f, 0f);
	public bool useLastSpriteFrame = false;
	public bool destroyParentGameObject = false;

	[Header("Sprite order: 0 for on top")]
	public int spriteOrder = 0;


	void Start () {
	
	}

	public void InstantiateAtLocRot(){
		GameObject go = Instantiate(prefabToInstantiate, (transform.position+transformOffset), transform.rotation*Quaternion.Euler(rotationOffset)) as GameObject;
		if(useLastSpriteFrame){
			SpriteRenderer instantiatedSR = go.GetComponent<SpriteRenderer>();
			SpriteRenderer currentSR = GetComponent<SpriteRenderer>();
			instantiatedSR.sprite = currentSR.sprite;
			instantiatedSR.color = currentSR.color;
			go.transform.localScale = transform.localScale;
			instantiatedSR.sortingOrder = spriteOrder;
		}
		if(destroyParentGameObject){
			Destroy (gameObject);
		}
	}
}
