using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using BidRoyale.Core;

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
    public Image charaIcon;
    public Sprite[] charaSprite;
    public Button buttonNext;
	public Text buttonText;

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
    int passNumber;
	RumbleScoreData[] rumbleScoreData;

	public void InitScore(GameMode _gameMode,int _round,float _score, int _auctionId = 0, int _timeToPopulateServer = 0, int _choice = 0) {
		auctionId = _auctionId;
		gameMode = _gameMode;
		score = _score;
		round = _round;
        choice = _choice;
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
				if (score == 0) {
					bottomLabel.text = LocalizationService.Instance.GetTextByKey ("Game.YOU_FAILED");
					buttonNext.interactable = true;
				} else {
					bottomLabel.text = LocalizationService.Instance.GetTextByKey ("Game.YOU_PASSED");
                    buttonNext.interactable = false;
                }
			} else {
				topLabel.text = LocalizationService.Instance.GetTextByKey ("Game.YOUR_SCORE_IS");
				bottomLabel.text = score.ToString ("00.0000");
				buttonNext.interactable = true;
			}
		}

        if (gameMode == GameMode.BIDROYALE)
        {
            charaIcon.sprite = charaSprite[0];
        }
        else
        {
            charaIcon.sprite = charaSprite[1];
        }

		ShowTimerOnButton ();
		Activate (true);
	}

	new protected void OnEnable() {
		base.OnEnable ();
		if (gameMode != GameMode.TRAINING) {
            buttonNext.interactable = false;
			StartCoroutine (WaitingForServer ());
        }
	}

	IEnumerator WaitingForServer() {
		Debug.Log ("Delay to populate server: "+timeToPopulateServer);
		while (timeToPopulateServer > 0) {
			timeToPopulateServer -= Time.deltaTime;
			ShowTimerOnButton ();
			yield return null;
		}
		timeToPopulateServer = 0;
		ShowTimerOnButton ();
		DBManager.API.GetPassingUserResult(auctionId,round,
			(response) =>
			{
				JSONNode jsonData = JSON.Parse(response);
				rank = jsonData["rank"].AsInt;
				status = jsonData["status"];
				winner = jsonData["winner"].AsBool;
				timeToNextGameRound = jsonData["timeToNextGameRound"].AsInt;
				if (gameMode==GameMode.BIDRUMBLE) {

                    rumbleScoreData = new RumbleScoreData[jsonData["scoreBoard"].Count];
                    for (int i = 0; i < jsonData["scoreBoard"].Count; i++)
                    {
						rumbleScoreData[i] = new RumbleScoreData();
                        int rRank = jsonData["scoreBoard"][i]["rank"].AsInt;
						rumbleScoreData[i].rank = rRank;
                        string rUser = jsonData["scoreBoard"][i]["username"];
                        rumbleScoreData[i].username = rUser;
                        float rScore = (float)(jsonData["scoreBoard"][i]["score"].AsDouble / 1000000000f);
						rumbleScoreData[i].score = rScore;

                        Debug.Log(rumbleScoreData[i].ToString());
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
					passNumber = jsonData["scoreBoard"][round-1]["passed"].AsInt;
                    RoyaleScoreData data = new RoyaleScoreData();
                    data.round = round;
                    data.passed = passNumber;
                    //data.answer = choice;
                    //data.correct = (status=="PASS");
                    data.answer = choice;
                    data.correct = true;
                    Debug.Log(data.ToString());
                    gameManager.AddRoyaleScore(data);
				}
				if ((winner)  || (status!="PASS")) {
					buttonNext.interactable = true;
                    if (gameMode == GameMode.BIDRUMBLE)
                    {
                        scoreBoard.InitScoreBoard(rumbleScoreData);
                    }
                    else
                    {
                        scoreBoard.InitScoreBoard(gameManager.GetRoyaleScores());
                    }
				} else {
                    StopAllCoroutines();
					StartCoroutine(DelayNextRound(1f));
				}
			},
			(error) =>
			{
				gameManager.PopUpBackToHome(error);
			}
		);
	}

	void ShowTimerOnButton() {
		if (timeToPopulateServer <= 0) {
			buttonText.text = LocalizationService.Instance.GetTextByKey ("Game.NEXT");
		} else {
			buttonText.text = Utilities.SecondsToMinutes ((int)timeToPopulateServer);
		}
	}

	IEnumerator DelayNextRound(float secs) {
		timeToNextGameRound -= (int)(secs * 1000f);
		yield return new WaitForSeconds (secs);
		Activate (false);
        int remPlayers = (gameMode == GameMode.BIDRUMBLE) ? rumbleScoreData.Length : passNumber;
		gameManager.NextRound (timeToNextGameRound,remPlayers);
	}

	public void NextClicked() {
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        if (gameMode == GameMode.TRAINING)
        {
			gameManager.ExitGame ();
		} else {
			Debug.Log ("Close result and show scoreboard");
			Activate (false);
			OnFinishOutro += ShowScoreBoard;
		}
	}

	void ShowScoreBoard() {
		OnFinishOutro -= ShowScoreBoard;
		scoreBoard.Activate (true);
	}
}
