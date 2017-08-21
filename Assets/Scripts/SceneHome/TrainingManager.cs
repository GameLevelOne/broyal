using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour {
	public GameObject panelTypeTheCode;
	public GameObject panelMemoryGame;
	public GameObject panelColorPairing;
	public GameObject panelSequence;

	public void OnClickTypeTheCode(){
		panelTypeTheCode.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void OnClickMemoryGame (){
		panelMemoryGame.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void OnClickColorPairing(){
		panelColorPairing.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void OnClickSequence(){
		panelSequence.SetActive(true);
		this.gameObject.SetActive(false);
	}
	
}
