using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGameManager : MonoBehaviour {
	public Text textCountdown;
	public GameObject panelVideoAds; //to be replaced with actual ads
	public GameObject panelLoadingBar;

	string sceneGame = "SceneGame";

	public void StartCountdown(){
		StartCoroutine(RunCountdown());
	}

	public void OnCloseAds(){
		panelLoadingBar.SetActive(true);
		panelLoadingBar.GetComponent<LoadingProgress>().ChangeScene(sceneGame);
	}

	IEnumerator RunCountdown ()
	{
		for (int i = 4; i >= 1; i--) {
			yield return new WaitForSeconds (1);
			textCountdown.text = i.ToString();
		}
		yield return new WaitForSeconds(1);
		panelVideoAds.SetActive(true);
	}
}
