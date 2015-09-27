using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MixLevels : MonoBehaviour {

	public AudioMixer masterMixer;

	public void SetSfxLvl(float sfxLvl){
		if(sfxLvl>-39f){
			masterMixer.SetFloat ("sfxVolume", sfxLvl);
		}
		else{
			masterMixer.SetFloat ("sfxVolume", -80f);
		}
	}

	public void SetMusicLvl(float musicLvl){
		if(musicLvl> -39f){
			masterMixer.SetFloat ("musicVolume", musicLvl);
		}
		else{
			masterMixer.SetFloat ("musicVolume", -80f);
		}
	}
}
