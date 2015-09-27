using UnityEngine;
using System.Collections;

public class InitializeBackgroundMusic : MonoBehaviour {

	public AudioClip levelMusic;
	public float levelMusicVolume;
	private AudioSource source;
	private AudioClip currentMusic;
	private float initialVolume;
	private float volume;



	void Start () {
		source = GameObject.FindGameObjectWithTag (Tags.mainCamera).GetComponent<AudioSource> ();
		currentMusic = source.clip;
		initialVolume = source.volume;
		if(currentMusic == null){
			source.volume = levelMusicVolume;
			source.clip = levelMusic;
			source.Play ();
		}

		else if (currentMusic != levelMusic) {
			//Change current music...
			StartCoroutine(ChangeMusic(levelMusic, levelMusicVolume));
		}
		else if (currentMusic == levelMusic && source.volume != levelMusicVolume){
			//Change music volume...
			StartCoroutine (FadeMusic (0.5f, initialVolume, levelMusicVolume));
		}
	}

	IEnumerator ChangeMusic(AudioClip audioClip, float vol){
		yield return StartCoroutine (FadeMusic (0.5f, initialVolume, 0.0f));
		source.volume = vol;
		source.clip = audioClip;
		source.Play ();
	}
	
	IEnumerator FadeMusic(float seconds, float initialValue, float finalValue){

		float i = 0.0f;
		float step = 1.0f / seconds;
		while (i <= 1.0f){
			i += Time.deltaTime * step;
			volume = Mathf.Lerp(initialVolume, finalValue, i);
			source.volume = volume;
			yield return null;
		}
	}

	public void ChangeMusicPublic(AudioClip audioClip, float vol){
		StartCoroutine(ChangeMusic (audioClip, vol));
	}
}
