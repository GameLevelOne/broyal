using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelGameReady : PagesIntroOutro {
	public Text roundLabel;
	public Text titleLabel;
	public Text readyLabel;
	public GameObject panelInfo;
	public GameObject playerInfo;
	public Text infoTitle;
	public Text infoLabel;
	public Text gameDescription;
	public Text countDownLabel;

	public float totalCountdown;
	public float countdown;

	public void ReadyGame(GameMode gameMode, int round, int countDownTimer, int remainingPlayer=0) {
        Debug.Log("Ready "+gameMode.ToString()+" Round: "+round);
		if (gameMode == GameMode.BIDROYALE) {
			titleLabel.text = LocalizationService.Instance.GetTextByKey ("Game.BID_ROYALE");
		} else if (gameMode == GameMode.BIDRUMBLE) {
			titleLabel.text = LocalizationService.Instance.GetTextByKey ("Game.BID_RUMBLE");
		} else {
			titleLabel.text = LocalizationService.Instance.GetTextByKey ("Game.TRAINING");
		}

		panelInfo.SetActive (true);
		if (remainingPlayer == 0) {
			roundLabel.text = LocalizationService.Instance.GetTextByKey ("Game.ENTERING");
			playerInfo.SetActive (false);
			gameDescription.gameObject.SetActive (true);
		} else {
			roundLabel.text = LocalizationService.Instance.GetTextByKey ("Game.ROUND") + " " + round;
			playerInfo.SetActive (true);
			gameDescription.gameObject.SetActive (false);
			infoLabel.text = remainingPlayer.ToString("N0");
		}
			
		totalCountdown = countDownTimer/1000f;
		countdown = 0f;
		ReadySetGo ();

		Activate (true);
	}

	void ReadySetGo() {
		float remaining = totalCountdown - countdown;
		countDownLabel.text = ((int)(remaining - 3f)).ToString ("D0");
		if (remaining >= 3f) {
			readyLabel.gameObject.SetActive (false);
			panelInfo.gameObject.SetActive (true);
			countDownLabel.gameObject.SetActive (true);
		} else {			
			readyLabel.gameObject.SetActive (true);
			panelInfo.gameObject.SetActive (false);
			countDownLabel.gameObject.SetActive (false);
			if (remaining < 1f) {
				readyLabel.text = LocalizationService.Instance.GetTextByKey ("Game.GO");
			} else if (remaining < 2f) {
				readyLabel.text = LocalizationService.Instance.GetTextByKey ("Game.GET_SET");
			} else {
				readyLabel.text = LocalizationService.Instance.GetTextByKey ("Game.READY");
			} 
		}
	}

	new protected void OnEnable() {
		base.OnEnable();
        StopAllCoroutines();
		StartCoroutine (StartCountdown ());
	}

	IEnumerator StartCountdown ()
	{
        Debug.Log("TimeToNextGameRound: "+totalCountdown);
		while (countdown < totalCountdown) {
			yield return null;
			countdown += Time.deltaTime;
			ReadySetGo ();
		}
        yield return null;
		Activate (false);
	}


}
