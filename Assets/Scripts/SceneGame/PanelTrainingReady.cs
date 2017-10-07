using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelTrainingReady : MonoBehaviour {
	public GameObject panelTypeTheCode;
	public GameObject panelMemoryGame;
	public GameObject panelColorPairing;
	public GameObject panelSequence;
	public Text countdownText;

	void OnEnable(){
		StartCoroutine(ReadySetGo());
	}

	void CheckTrainingType(){
		RumbleGame type = (RumbleGame) PlayerPrefs.GetInt ("RumbleGame",0);;

		if(type == RumbleGame.TYPETHECODE){
			panelTypeTheCode.SetActive(true);
		} else if(type == RumbleGame.MEMORYGAME){
			panelMemoryGame.SetActive(true);
		} else if(type == RumbleGame.COLORPAIRING){
			panelColorPairing.SetActive(true);
		} else if(type == RumbleGame.SEQUENCE){
			panelSequence.SetActive(true);
		}
	}

	IEnumerator ReadySetGo(){
		countdownText.text = "READY!";
		yield return new WaitForSeconds(1);
		countdownText.text = "GET SET!";
		yield return new WaitForSeconds(1);
		countdownText.text = "GO!";
		yield return new WaitForSeconds(1);
		CheckTrainingType();
		this.gameObject.SetActive(false);
	}
	

}
