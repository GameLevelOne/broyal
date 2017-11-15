using System.Collections;
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
	public BaseGame[] gamePanel;
	public PanelScoresManager scorePanel;
	public ScoreBoardManager scoreBoard;

	public int gameTime;
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
		nextGame = (gameMode == GameMode.BIDROYALE) ? 4 : (int) rumbleGame;
		round = (gameMode == GameMode.TRAINING) ? 1 : 0;
		countdownTimer = PlayerPrefs.GetInt ("TimeToGame",0);
		remainingPlayer = 0;
		royaleScores = new List<RoyaleScoreData> ();

		InitGame ();

		//GameTesting
//		gamePanel[0].InitGame(gameTime,round);
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
		gamePanel [nextGame].InitGame(gameTime,round);
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
					int timeToPopulateServerData = jsonData["timeToServerPopulateData"];
					scorePanel.InitScore(gameMode,round,score,auctionId,timeToPopulateServerData);
                },
                (error) =>
                {
					PopUpBackToHome();
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
    public void AddRoyaleScore(RoyaleScoreData data)
    {
        royaleScores.Add(data);
    }
    public RoyaleScoreData[] GetRoyaleScores()
    {
        return royaleScores.ToArray();
    }


    public void ExitGame()
    {
		if (notifPopUp.isActiveAndEnabled) 
			notifPopUp.Activate (false);
		
		if (gameMode == GameMode.TRAINING) {
			scorePanel.Activate (false);
			scorePanel.OnFinishOutro += LoadToHome;        
		} else {
			if (scoreBoard.isActiveAndEnabled) {
				scoreBoard.Activate (false);
				scoreBoard.OnFinishOutro += LoadToHome;        
			} else {
				gamePanel [nextGame].Activate (false);
			}
		}
    }

	public void PopUpBackToHome() {
		notifPopUp.ShowPopUp(LocalizationService.Instance.GetTextByKey ("Game.ERROR"));
		notifPopUp.OnFinishOutro += LoadToHomeFromNotif;
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
