using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameManager : MonoBehaviour {
	public GameObject panelGameReady;
	public GameObject panelTrainingReady;

	void Start () {
		CheckGameType();
	}

	void CheckGameType(){
		if(PlayerData.Instance.CurrentGameType == GameType.BidRoyale){
			panelGameReady.SetActive(true);
			panelTrainingReady.SetActive(false);
		} else if(PlayerData.Instance.CurrentGameType == GameType.BidRumble){
			panelGameReady.SetActive(true); //temp
			panelTrainingReady.SetActive(false);
		} else if(PlayerData.Instance.CurrentGameType == GameType.Training){
			panelTrainingReady.SetActive(true);
			panelGameReady.SetActive(false);
		}
	}

}
