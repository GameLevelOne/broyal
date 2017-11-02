using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public PagesIntroOutro[] gamePanel;

	GameMode gameMode;
	RumbleGame rumbleGame;
	int nextGame;
	int round;
	int countdownTimer;
	int remainingPlayer;

	void Start () {
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
		InitGame ();
	}

	void InitGame(){
		panelGameReady.ReadyGame (gameMode, round, countdownTimer, remainingPlayer);
		panelGameReady.OnFinishOutro += LoadNextGame;
	}

	void LoadNextGame() {
		panelGameReady.OnFinishOutro -= LoadNextGame;
		gamePanel [nextGame].Activate (true);
	}

	public string GetRound() {
		return round.ToString ("00");
	}

	public void EndGame(float score, float timeToPopulate) {
		if (gameMode == GameMode.TRAINING) {
			loadingPanel.gameObject.SetActive (true);
		} else {
		}
	}
}
