using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelScoresManager : MonoBehaviour {
	public GameObject panelGameReady;
	public GameObject panelGame;
	public GameObject panelScore;
	public GameObject panelScoreboard;
	public GameObject panelLoadingBar;
	public Text textResult;

	bool rightAnswer = false;
	string sceneGame = "SceneGame";

	public bool RightAnswer{ set { rightAnswer=value;}}

	void OnEnable(){
		if (rightAnswer) {
			textResult.text = "YOU PASSED!";
		} else {
			textResult.text = "YOU LOSE!";
		}
	}

	public void OnClickNext ()
	{
		if (rightAnswer) {
			panelScoreboard.SetActive (true);
			panelScore.SetActive(false);
		} else {
			panelLoadingBar.SetActive(true);
			LoadingProgress.Instance.ChangeScene(sceneGame);
		}

	}

	public void OnClickNextToHome(){ //to be replaced with claim prize page (SceneHome)
		panelLoadingBar.SetActive(true);
		LoadingProgress.Instance.ChangeScene("SceneHome");
	}
	
}
