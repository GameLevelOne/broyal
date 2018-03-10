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
    public ConnectingPanel connectingPanel;
    public PanelGameReady panelGameReady;
    public NotificationPopUp notifPopUp;
	public BaseGame[] gamePanel;
	public PanelScoresManager scorePanel;
	public ScoreBoardManager scoreBoard;
    public VideoAdsManager videoPanel;

	public int gameTime;
	public int royaleGameTime;
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
		round = (gameMode == GameMode.TRAINING) ? 0 : 1;
		countdownTimer = PlayerPrefs.GetInt ("TimeToGame",0);
		remainingPlayer = 0;
		royaleScores = new List<RoyaleScoreData> ();

		InitGame ();

		//GameTesting
//		gamePanel[0].InitGame(gameTime,round);
	}

	void InitGame(){
        SoundManager.Instance.PlaySFX(SFXList.GameStart,true);
		if (round==1) {
			Debug.Log ("---------Check Eligible for time-----------");
			connectingPanel.Connecting (true);
			DBManager.API.GetEligibleToEnterGame (auctionId,
				(response) => {
					connectingPanel.Connecting (false);
					JSONNode jsonData = JSON.Parse (response);
					countdownTimer = jsonData ["timeToFirstGameRound"].AsInt;
					panelGameReady.ReadyGame (gameMode, round, countdownTimer, remainingPlayer);
					panelGameReady.OnFinishOutro += LoadNextGame;
				},
				(error) => {
					connectingPanel.Connecting (false);
					JSONNode jsonData = JSON.Parse (error);
					if (jsonData != null) {
						notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("Error." + jsonData ["errors"]));
					} else {
						notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("General.SERVER_ERROR"));
					}
					notifPopUp.OnFinishOutro += LoadToHomeFromNotif;
				}
			);
		} else {
			panelGameReady.ReadyGame (gameMode, round, countdownTimer, remainingPlayer);
			panelGameReady.OnFinishOutro += LoadNextGame;
		}
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
		gamePanel [nextGame].InitGame(nextGame==4 ? royaleGameTime : gameTime,round);
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
                    gamePanel[nextGame].Activate(false);
                    scorePanel.InitScore(gameMode, round, score, auctionId, timeToPopulateServerData);
                },
                (error) =>
                {
					PopUpBackToHome(error);
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
		scorePanel.InitScore(gameMode,round,(win ? 1 : 0),auctionId,timeToPopulateServerData,choice);
	}
    public void AddRoyaleScore(RoyaleScoreData data)
    {
        Debug.Log("Adding Score Data Round: "+data.round+ ", Answer: "+data.answer+", Correct: "+data.correct+", Passed: "+data.passed);
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
            scorePanel.Activate(false);
            scorePanel.OnFinishOutro += LoadToHome;
//            videoPanel.Activate(true);
//            videoPanel.OnFinishOutro += LoadToHome;
        }
        else
        {
			if (scoreBoard.gameObject.activeSelf) {
				scoreBoard.Activate (false);
				scoreBoard.OnFinishOutro += LoadToHome;
//                videoPanel.Activate(true);
//                videoPanel.OnFinishOutro += LoadToHome;        
			} else if (scorePanel.gameObject.activeSelf) {
				scorePanel.Activate(false);
				scorePanel.OnFinishOutro += LoadToHome;
			} else {
				gamePanel [nextGame].Activate (false);
			}
		}
    }

	public void PopUpBackToHome(string error) {
		JSONNode jsonData = JSON.Parse (error);
		if (jsonData!=null) {
			notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
		} else {
			notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
		}
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
            //videoPanel.OnFinishOutro -= LoadToHome;
        }
        else
        {
//            videoPanel.OnFinishOutro -= LoadToHome;        
			scoreBoard.OnFinishOutro -= LoadToHome;
		}
        loadingPanel.gameObject.SetActive(true);
    }

}
