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
		TrainingType type = PlayerData.Instance.CurrentTrainingType;

		if(type == TrainingType.TypeTheCode){
			panelTypeTheCode.SetActive(true);
		} else if(type == TrainingType.MemoryGame){
			panelMemoryGame.SetActive(true);
		} else if(type == TrainingType.ColorPairing){
			panelColorPairing.SetActive(true);
		} else if(type == TrainingType.Sequence){
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
