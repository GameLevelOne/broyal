using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode{
	TRAINING,
	BIDROYALE,
	BIDRUMBLE,
}

public enum RumbleGame{
	TYPETHECODE,
	MEMORYGAME,
	COLORPAIRING,
	SEQUENCE
}

public class SceneGameManager : MonoBehaviour {
	public GameObject panelGameReady;
	public GameObject panelTrainingReady;

	void Start () {
		CheckGameType();
	}

	void CheckGameType(){
		GameMode mode = (GameMode) PlayerPrefs.GetInt ("GameMode",0);
		if(mode == GameMode.BIDROYALE){
			panelGameReady.SetActive(true);
			panelTrainingReady.SetActive(false);
		} else if(mode == GameMode.BIDRUMBLE){
			panelGameReady.SetActive(true); //temp
			panelTrainingReady.SetActive(false);
		} else if(mode == GameMode.TRAINING){
			panelTrainingReady.SetActive(true);
			panelGameReady.SetActive(false);
		}
	}

}
