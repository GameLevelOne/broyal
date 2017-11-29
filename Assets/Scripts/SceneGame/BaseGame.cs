using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseGame : PagesIntroOutro {
	public SceneGameManager gameManager;
	public GameObject overlay;
	public Text overlayLabel;

	public Text timeLabel;
	public Text roundLabel;

	float timeToEndGame;
	float score;
	protected bool stopGame;

	public virtual void InitGame(int gameTime, int round) {
		timeToEndGame = gameTime;
		score = gameTime;
		timeLabel.text = timeToEndGame.ToString ("00");
		if (round == 0) {
			roundLabel.text = "-";
		} else {
			roundLabel.text = LocalizationService.Instance.GetTextByKey ("Game.ROUND") + " " + round;
		}
		overlay.SetActive (false);
		stopGame = false;
		Activate (true);
	}

	new protected void OnEnable() {
		base.OnEnable ();
		Debug.Log ("Game - "+name+" Started!!!");
		StartCoroutine (TimerCountdown ());
	}

	IEnumerator TimerCountdown() {
		while ((timeToEndGame > 0) && (!stopGame)) {
			timeToEndGame -= Time.deltaTime;
			timeLabel.text = timeToEndGame.ToString ("00");
			yield return null;
		}
		if (timeToEndGame < 0)
			timeToEndGame = 0;
		EndGame (false);
	}


	protected virtual void EndGame(bool finish) {
		stopGame = finish;
		Debug.Log ("Game - "+name+" Ended!!!");
		score -= timeToEndGame;
		StopAllCoroutines();
		overlay.SetActive(true);
		if (stopGame) {
			overlayLabel.text = LocalizationService.Instance.GetTextByKey ("Game.CONGRATULATIONS");
		} else {
			overlayLabel.text = LocalizationService.Instance.GetTextByKey ("Game.TIMES_UP");
		}

		gameManager.EndGame (score);	
	
	}
}
