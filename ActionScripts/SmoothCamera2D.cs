using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {

	public float lagTime = 12f;
	//public float targetLagTime;
	//[HideInInspector] public float lagChangeTransitionTime;
	public Transform player;
	public Transform cursor;
	public float maxLookOutDistance = 8.5f;
	private float shakeFactor = 0f;
	private float shakeDecreaseFactor = 15f;
	private Vector3 shake = Vector3.zero;
	//public float t=1f;
	private float initialLagTime;

	void Start(){
		//targetLagTime = lagTime;
		//t = 1f;
	}

	void FixedUpdate () {
		if (player) {

			Vector3 from = transform.position;
			float halfDistance = (cursor.position-player.position).magnitude*0.5f;
			float lookOutDistance = 0.0f;
			if(halfDistance > maxLookOutDistance)
				lookOutDistance = maxLookOutDistance;
			else
				lookOutDistance = halfDistance;
			Vector3 to = player.position+player.forward*lookOutDistance;
			to.y = transform.position.y;
			transform.position -= (from - to) * lagTime * Time.deltaTime;
			if(shakeFactor > 0f){
				transform.position += shake*shakeFactor;
				shakeFactor -= Time.deltaTime*shakeDecreaseFactor;
			}
			else{
				shakeFactor =0f;
			}
		}
	}

	public void shakeCamera(float shakeMagnitude, Vector3 shakeSourcePosition){
		//shake2 = Random.insideUnitCircle*shakeMagnitude;
		//shake3 = new Vector3 (shake2.x, 0f, shake2.y);
		if(player){
			shake = (Vector3.Normalize(player.position - shakeSourcePosition))*shakeMagnitude;
		}
		shakeFactor = 1f;

	}

	public void ChangeLag (float secondsTransition, float targetLagTime){
			initialLagTime = lagTime;
			StartCoroutine (FadeLag (secondsTransition, initialLagTime, targetLagTime));
		}

	IEnumerator FadeLag(float seconds, float initialValue, float finalValue){
		
		float i = 0.0f;
		float step = 1.0f / seconds;
		while (i <= 1.0f){
			i += Time.deltaTime * step;
			lagTime = Mathf.Lerp(initialValue, finalValue, i);

			yield return null;
		}
	}

}
