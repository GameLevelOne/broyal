using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class PanelScoresManager : PagesIntroOutro {
	public SceneGameManager gameManager;
	public ScoreBoardManager scoreBoard;

	public GameObject simpleInfo;
	public Text topLabel;
	public Text bottomLabel;
	public GameObject detailedInfo;
	public Text roundLabel;
	public Text scoreLabel;
	public GameObject resultInfo;
	public Text rankLabel;
	public Text statusLabel;
	public GameObject waitingInfo;
	public Button buttonNext;

	int auctionId;
	bool passed;
	GameMode gameMode;
	int round;
	float score;
	float timeToPopulateServer;
	int rank;
	string status;
	bool winner;
	int timeToNextGameRound;
	int choice;
	RumbleScoreData[] rumbleScoreData;

	public void InitScore(GameMode _gameMode,int _round,float _score, int _auctionId = 0, int _timeToPopulateServer = 0, int _choice = 0) {
		auctionId = _auctionId;
		gameMode = _gameMode;
		score = _score;
		round = _round;
		timeToPopulateServer = _timeToPopulateServer / 1000f;
		buttonNext.interactable = false;
		if (gameMode == GameMode.BIDRUMBLE) {
			detailedInfo.SetActive (true);
			simpleInfo.SetActive (false);

			roundLabel.text = LocalizationService.Instance.GetTextByKey ("Game.ROUND") + " " + round;
			scoreLabel.text = score.ToString ("00.0000");
			resultInfo.SetActive (false);
			waitingInfo.SetActive (true);

		} else {
			detailedInfo.SetActive (false);
			simpleInfo.SetActive (true);
			if (gameMode == GameMode.BIDROYALE) {
				topLabel.text = LocalizationService.Instance.GetTextByKey ("Game.ROUND") + " " + round;
				if (score = 0) {
					bottomLabel.text = LocalizationService.Instance.GetTextByKey ("Game.YOU_FAILED");
					buttonNext.interactable = true;
				} else {
					bottomLabel.text = LocalizationService.Instance.GetTextByKey ("Game.YOU_PASSED");
				}
			} else {
				topLabel.text = LocalizationService.Instance.GetTextByKey ("Game.YOUR_SCORE_IS");
				bottomLabel.text = score.ToString ("00.0000");
				buttonNext.interactable = true;
			}
		}

		Activate (true);
	}

	new protected void OnEnable() {
		base.OnEnable ();
		if (gameMode == GameMode.BIDRUMBLE) {
			StartCoroutine (WaitingForServer ());
		}
	}

	IEnumerator WaitingForServer() {
		yield return new WaitForSeconds (timeToPopulateServer);
		DBManager.API.GetPassingUserResult(auctionId,round,
			(response) =>
			{
				JSONNode jsonData = JSON.Parse(response);
				rank = jsonData["rank"].AsInt;
				status = jsonData["status"];
				winner = jsonData["winner"].AsBool;
				timeToNextGameRound = jsonData["timeToNextGameRound"].AsInt;
				rumbleScoreData = new RumbleScoreData[jsonData["scoreBoard"].Count];
				if (gameMode==GameMode.BIDRUMBLE) {

					for (int i=0;i<jsonData["scoreBoard"].Count;i++) {
						rumbleScoreData[i] = new RumbleScoreData();
						rumbleScoreData[i].rank = jsonData["scoreBoard"][i]["rank"].AsInt;
						rumbleScoreData[i].username = jsonData["scoreBoard"][i]["username"];
						rumbleScoreData[i].score = jsonData["scoreBoard"][i]["score"].AsInt / 1000000000f;
					}

					rankLabel.text = rank.ToString("N0");
					if (status=="PASS") {
						statusLabel.text = LocalizationService.Instance.GetTextByKey ("Game.YOU_PASSED");
					} else {
						statusLabel.text = LocalizationService.Instance.GetTextByKey ("Game.YOU_FAILED");
					}

					waitingInfo.SetActive(false);
					resultInfo.SetActive(true);
				} else {

//					mulai disiniiiii, 
//					jangan lupa bikin data itemnya di SceneGameManager
//					untuk scoreboard

				}
				if ((winner)  || (status!="PASS")) {
					buttonNext.interactable = true;
					scoreBoard.InitScoreBoard(rumbleScoreData);
				} else {
					StartCoroutine(DelayNextRound(1f));
				}
			},
			(error) =>
			{
			}
		);
	}

	IEnumerator DelayNextRound(float secs) {
		timeToNextGameRound -= (int)(secs * 1000f);
		yield return new WaitForSeconds (secs);
		Activate (false);
		gameManager.NextRound (timeToNextGameRound,rumbleScoreData.Length);
	}

	public void NextClicked() {
		if (gameMode == GameMode.TRAINING) {
			gameManager.ExitGame ();
		} else {
			Activate (false);
			OnFinishOutro += ShowScoreBoard;
		}
	}

	void ShowScoreBoard() {
		OnFinishOutro -= ShowScoreBoard;
		scoreBoard.Activate (true);
	}
}
