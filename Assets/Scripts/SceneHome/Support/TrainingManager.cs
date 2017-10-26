using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : BasePage {
	public LoadingProgress panelLoadingBar;

	public GameObject panelTypeTheCode;
	public GameObject panelMemoryGame;
	public GameObject panelColorPairing;
	public GameObject panelSequence;

	public NumberCountUpEffect textPetExp;

	public void OnClickOpenTrainingPanel(int type){
        SoundManager.Instance.PlaySFX(SFXList.Button01);

//		if (type == (int)TrainingType.TypeTheCode) {
//			//panelTypeTheCode.SetActive(true);
//			PlayerData.Instance.CurrentTrainingType = TrainingType.TypeTheCode;
//		} else if(type == (int)TrainingType.MemoryGame){
//			//panelMemoryGame.SetActive(true);
//			PlayerData.Instance.CurrentTrainingType = TrainingType.MemoryGame;
//		} else if(type == (int)TrainingType.ColorPairing){
//			//panelColorPairing.SetActive(true);
//			PlayerData.Instance.CurrentTrainingType = TrainingType.ColorPairing;
//		} else if(type == (int)TrainingType.Sequence){
//			//panelSequence.SetActive(true);
//			PlayerData.Instance.CurrentTrainingType = TrainingType.Sequence;
//		}
		PlayerPrefs.SetInt("RumbleGame",type);
		PlayerPrefs.SetInt("GameMode",(int)GameMode.TRAINING);
		panelLoadingBar.gameObject.SetActive(true);
//		panelLoadingBar.ChangeScene("SceneGame");
	}

	#region tempTouchBox

	public void OnClickCloseTrainingPanel (int type)
	{
		//TODO: adjust exp with the actual values

		int currExp = 0;
		int rewardExp = 0; 

		if (type == (int)RumbleGame.TYPETHECODE) {
			panelTypeTheCode.SetActive(false);
			rewardExp = 100;
		} else if(type == (int)RumbleGame.MEMORYGAME){
			panelMemoryGame.SetActive(false);
			rewardExp = 200;
		} else if(type == (int)RumbleGame.COLORPAIRING){
			panelColorPairing.SetActive(false);
			rewardExp = 100;
		} else if(type == (int)RumbleGame.SEQUENCE){
			panelSequence.SetActive(false);
			rewardExp = 200;
		}
		int totalExp = currExp + rewardExp;
//		PlayerData.Instance.PetExp = totalExp;
		textPetExp.DoAnimCountUp(currExp,totalExp);
	}

	#endregion
	
}
