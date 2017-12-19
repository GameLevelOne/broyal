using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : BasePage {
	public LoadingProgress panelLoadingBar;

	public void ClickTrainGame(int type){
        SoundManager.Instance.PlaySFX(SFXList.Button01);

		PlayerPrefs.SetInt("RumbleGame",type);
		PlayerPrefs.SetInt("GameMode",(int)GameMode.TRAINING);
		PlayerPrefs.SetInt("TimeToGame",6000);

		Activate (false);
		PagesManager.instance.headerNav.Activate (false);

		OnFinishOutro += OutroToLoad;
	}

	void OutroToLoad() {
		OnFinishOutro -= OutroToLoad;
		panelLoadingBar.gameObject.SetActive(true);
	}

	
}
