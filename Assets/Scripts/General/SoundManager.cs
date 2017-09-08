using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMList{
	BGMAuction,
	BGMMenu01,
	BGMMenu02
}

public enum SFXList{
	AuctionWin,
	Button01,
	Button02,
	CardClose,
	CardMatch,
	CardOpen,
	ChestOpen,
	ChestWrong,
	Correct,
	Gain,
	GameStart,
	Incorrect,
	Knock,
	Pop,
	RankUp,
	SuddenDeath,
	TimeUp
}

public class SoundManager : MonoBehaviour {
	private static SoundManager instance;
	public static SoundManager Instance{get{return instance;}}

	public AudioClip[] BGM;
	public AudioClip[] SFX;

	AudioSource audioSource;

	void Awake(){
		if(instance != null && instance !=this){
			Destroy(this.gameObject);
		} else{
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
		audioSource = GetComponent<AudioSource>();
	}

	public void PlayBGM  (BGMList bgm){
		audioSource.clip = BGM[(int)bgm];
		audioSource.Play();
	}

	public void PlaySFX (SFXList sfx){
		audioSource.PlayOneShot(SFX[(int)sfx]);
	}

	public void StopPlay(){
		audioSource.Stop();
	}

	public void SetVolume(float volume){
		audioSource.volume = volume;
	}
}
