using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelScoresManager : MonoBehaviour {
	public GameObject panelGameReady;
	public GameObject panelScore;
	public GameObject panelScoreboard;
	public GameObject panelLoadingBar;
	public Text textResult;

	bool rightAnswer = false;

	public bool RightAnswer{ set { rightAnswer=value;}}

	public void OnClickNext ()
	{
		if (rightAnswer) {
			textResult.text = "YOU PASSED!";
		} else {
			textResult.text = "YOU LOSE!";
		}

		panelScoreboard.SetActive(true);
		panelScore.SetActive(false);
	}

	public void OnClickNextToHome(){ //to be replaced with claim prize page (SceneHome)
		panelLoadingBar.SetActive(true);
		LoadingProgress.Instance.ChangeScene("SceneHome");
	}
	
}
