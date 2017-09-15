using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelTrainingScores : MonoBehaviour {
	public Text textTime;
	public LoadingProgress loadingBar;

	public void SetScoreText(string text){
		textTime.text = text;
	}

	public void OnClickNext(){
		loadingBar.gameObject.SetActive(true);
		loadingBar.ChangeScene("SceneHome");
	}
	
}
