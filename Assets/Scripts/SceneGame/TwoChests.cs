using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class TwoChests : PagesIntroOutro {
	public SceneGameManager gameManager;
	public ConnectingPanel connectingPanel;

	public Animator chestBrownButton;
	public Animator chestRedButton;
	public Animator resultPanel;
	public Image correctChest;
	public Image wrongChest;
	public Text timerText;

	public Sprite[] correctChestSprite; 
	public Sprite[] wrongChestSprite; 

	int curChoice;
	int auctionId;
	int round;

	void InitGame() {
		chestBrownButton.SetInteger ("ChestState",0);
		chestRedButton.SetInteger ("ChestState",0);
		resultPanel.SetInteger ("ChestResult",0);
		resultPanel.gameObject.SetActive (false);
		curChoice = -1;
		auctionId = PlayerPrefs.GetInt("GameAuctionId", 0);
		round = PlayerPrefs.GetInt("GameRound", 0);
	}

	void OnEnable(){
		InitGame ();
		StartCoroutine(StartCountdown());
	}

	public void OnClickChest (int chest)
	{
		if (chest==0) {
			chestBrownButton.SetInteger ("ChestState",1);
			chestRedButton.SetInteger ("ChestState",-1);
		} else {
			chestBrownButton.SetInteger ("ChestState",-1);
			chestRedButton.SetInteger ("ChestState",1);
		}
		curChoice = chest;

	}
		
	IEnumerator StartCountdown ()
	{
		for (int i = 6; i >= 0; i--) {
			yield return new WaitForSeconds(1);
			timerText.text = "0"+i.ToString();
		}
		CheckResult();
	}

	void CheckResult() {
		if (curChoice < 0) {
			resultPanel.gameObject.SetActive (true);
			resultPanel.SetInteger ("ChestResult",3);
			StartCoroutine (DelayEnd (2f, false));
		} else {
			connectingPanel.Connecting (true);
			DBManager.API.SubmitBidRoyaleResult(auctionId,round,curChoice,
				(response) =>
				{
					Debug.Log ("Exit Rumble");
					JSONNode jsonData = JSON.Parse(response);
					int timeToPopulateServerData = jsonData["timeToPopulateServerData"];
					bool win = (jsonData["correctAnswer"].AsInt == curChoice) ? true : false;

					resultPanel.gameObject.SetActive (true);
					resultPanel.SetInteger ("ChestResult",win ? 1 : 2);
					StartCoroutine (DelayEnd (2f, win, timeToPopulateServerData));
				},
				(error) =>
				{
//					notifPopUp.ShowPopUp(LocalizationService.Instance.GetTextByKey ("Game.ERROR"));
//					notifPopUp.OnFinishOutro += LoadToHomeFromNotif;
					Activate(false);
					gameManager.loadingPanel.gameObject.SetActive(true);
				}
			);
		}
	}

	IEnumerator DelayEnd(float secs, bool win,int timeToPopulateServerData=0) {
		yield return new WaitForSeconds (secs);
		gameManager.EndRoyale (win, timeToPopulateServerData, curChoice);
	}
}
