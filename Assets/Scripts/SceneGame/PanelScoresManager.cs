using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScoresManager : MonoBehaviour {
	public GameObject panelScore;
	public GameObject panelScoreboard;
	public GameObject panelLoadingBar;

	public void OnClickNextScoreboard (){
		panelScoreboard.SetActive(true);
		panelScore.SetActive(false);
	}

	public void OnClickNextToHome(){ //to be replaced with claim prize page (SceneHome)
		panelLoadingBar.SetActive(true);
		LoadingProgress.Instance.ChangeScene("SceneHome");
	}
	
}
