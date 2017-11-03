﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public enum GameMode{
	BIDROYALE,
	BIDRUMBLE,
	TRAINING
}

public enum RumbleGame{
	MEMORYGAME,
	COLORPAIRING,
	TYPETHECODE,
	SEQUENCE
}

public class SceneGameManager : MonoBehaviour {
	public LoadingProgress loadingPanel;
	public PanelGameReady panelGameReady;
    public NotificationPopUp notifPopUp;
    public PagesIntroOutro[] gamePanel;
	public PanelScoresManager scorePanel;
	public ScoreBoardManager scoreBoard;

	public List<RoyaleScoreData> royaleScores;

	GameMode gameMode;
	RumbleGame rumbleGame;
    int auctionId;
	int nextGame;
	int round;
	int countdownTimer;
	int remainingPlayer;

	void Start () {
        auctionId = PlayerPrefs.GetInt("GameAuctionId", 0);
		gameMode = (GameMode) PlayerPrefs.GetInt ("GameMode",0);
		rumbleGame = (RumbleGame) PlayerPrefs.GetInt ("RumbleGame",0);
		if (gameMode == GameMode.BIDROYALE) {
			nextGame = 4;
		} else {
			nextGame = (int) rumbleGame;
		}
		round = 1;
		countdownTimer = PlayerPrefs.GetInt ("TimeToGame",0);
		remainingPlayer = 0;
		royaleScores = new List<RoyaleScoreData> ();
		InitGame ();
	}

	void InitGame(){
		panelGameReady.ReadyGame (gameMode, round, countdownTimer, remainingPlayer);
		panelGameReady.OnFinishOutro += LoadNextGame;
	}

	public void NextRound(int _countdownTimer, int _remainingPlayer) {
		round++;
		countdownTimer = _countdownTimer;
		remainingPlayer = _remainingPlayer;
		InitGame ();
	}

	void LoadNextGame() {
		panelGameReady.OnFinishOutro -= LoadNextGame;
		PlayerPrefs.SetInt ("GameRound",round);
		gamePanel [nextGame].Activate (true);
	}

	public void EndGame(float score) {
		Debug.Log ("End Game ("+score.ToString("0.0000")+"s)");
		if (gameMode == GameMode.TRAINING) {
			StartCoroutine(DelayExit (1f,score));
			Debug.Log ("Exit training");
		} else if (gameMode == GameMode.BIDRUMBLE) {
            DBManager.API.SubmitBidRumbleResult(auctionId,round,score,
                (response) =>
                {
					Debug.Log ("Exit Rumble");
                    JSONNode jsonData = JSON.Parse(response);
					int timeToPopulateServerData = jsonData["timeToPopulateServerData"];
					scorePanel.InitScore(gameMode,round,score,auctionId,timeToPopulateServerData);
                },
                (error) =>
                {
                    notifPopUp.ShowPopUp(LocalizationService.Instance.GetTextByKey ("Game.ERROR"));
					notifPopUp.OnFinishOutro += LoadToHomeFromNotif;
                }
            );
        }
	}

	IEnumerator DelayExit(float secs, float lastScore) {
		yield return new WaitForSeconds (secs);
		gamePanel [nextGame].Activate (false);
		scorePanel.InitScore(gameMode,round,lastScore);
	}


	public void EndRoyale(bool win, int timeToPopulateServerData=0, int choice=0) {
		gamePanel [nextGame].Activate (false);
		scorePanel.InitScore(gameMode,round,(win ? 0 : 1),auctionId,timeToPopulateServerData,choice);
	}


    public void ExitGame()
    {
		if (gameMode == GameMode.TRAINING) {
			scorePanel.Activate (false);
			scorePanel.OnFinishOutro += LoadToHome;        
		} else {
			scoreBoard.Activate (false);
			scoreBoard.OnFinishOutro += LoadToHome;        
		}
    }

	void LoadToHomeFromNotif() {
		notifPopUp.OnFinishOutro -= LoadToHomeFromNotif;
		gamePanel [nextGame].Activate (false);
		loadingPanel.gameObject.SetActive(true);
	}

    void LoadToHome()
    {
		if (gameMode == GameMode.TRAINING) {
			scorePanel.OnFinishOutro -= LoadToHome;        
		} else {
			scoreBoard.OnFinishOutro -= LoadToHome;        
		}
        loadingPanel.gameObject.SetActive(true);
    }
}
