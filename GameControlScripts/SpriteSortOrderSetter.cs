using UnityEngine;
using System.Collections;

public class SpriteSortOrderSetter : MonoBehaviour {

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		SpriteSortOrderCounter counter = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<SpriteSortOrderCounter> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();

		if(spriteRenderer.sortingOrder == 0){
			//Set sorting order to the next in line...
			spriteRenderer.sortingOrder = counter.sortOrderCounter;
			//increment counter...
			counter.sortOrderCounter++;
		}
		else{
			//Use existing sorting order
		}
	}

	public void SetOrderToNegativeOne(){
		spriteRenderer.sortingOrder = -1;
	}
}
