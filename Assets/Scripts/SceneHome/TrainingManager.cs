using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrainingType{
	TypeTheCode,MemoryGame,ColorPairing,Sequence
}

public class TrainingManager : MonoBehaviour {
	public GameObject panelTypeTheCode;
	public GameObject panelMemoryGame;
	public GameObject panelColorPairing;
	public GameObject panelSequence;

	public NumberCountUpEffect textPetExp;

	public void OnClickOpenTrainingPanel(int type){
		if (type == (int)TrainingType.TypeTheCode) {
			panelTypeTheCode.SetActive(true);
		} else if(type == (int)TrainingType.MemoryGame){
			panelMemoryGame.SetActive(true);
		} else if(type == (int)TrainingType.ColorPairing){
			panelColorPairing.SetActive(true);
		} else if(type == (int)TrainingType.Sequence){
			panelSequence.SetActive(true);
		}
	}

	#region tempTouchBox

	public void OnClickCloseTrainingPanel (int type)
	{
		//TODO: adjust exp with the actual values

		int currExp = PlayerData.Instance.PetExp;
		int rewardExp = 0; 

		if (type == (int)TrainingType.TypeTheCode) {
			panelTypeTheCode.SetActive(false);
			rewardExp = 100;
		} else if(type == (int)TrainingType.MemoryGame){
			panelMemoryGame.SetActive(false);
			rewardExp = 200;
		} else if(type == (int)TrainingType.ColorPairing){
			panelColorPairing.SetActive(false);
			rewardExp = 100;
		} else if(type == (int)TrainingType.Sequence){
			panelSequence.SetActive(false);
			rewardExp = 200;
		}
		int totalExp = currExp + rewardExp;
		PlayerData.Instance.PetExp = totalExp;
		textPetExp.DoAnimCountUp(currExp,totalExp);
	}

	#endregion
	
}
