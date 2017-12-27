using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class TwoChests : BaseGame {
	public ConnectingPanel connectingPanel;

	public Animator chestBrownButton;
	public Animator chestRedButton;
	public Animator resultPanel;
	public Image correctChest;
	public Image wrongChest;

	public Sprite[] correctChestSprite; 
	public Sprite[] wrongChestSprite;

    int correctAnswer;
	int curChoice;
	int auctionId;
	int round;

	public override void InitGame(int gameTime, int _round) {
		chestBrownButton.SetInteger ("ChestState",0);
		chestRedButton.SetInteger ("ChestState",0);
		resultPanel.SetInteger ("ChestResult",0);
		resultPanel.gameObject.SetActive (false);
		curChoice = -1;
		auctionId = PlayerPrefs.GetInt("GameAuctionId", 0);
		round = _round;

		base.InitGame(gameTime,round);
	}		

	public void OnClickChest (int chest)
	{
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		if (chest==0) {
			chestBrownButton.SetInteger ("ChestState",1);
			chestRedButton.SetInteger ("ChestState",-1);
		} else {
			chestBrownButton.SetInteger ("ChestState",-1);
			chestRedButton.SetInteger ("ChestState",1);
		}
		curChoice = chest;

	}
		
	void CheckResult() {
		if (curChoice < 0) {
			resultPanel.gameObject.SetActive (true);
			resultPanel.SetInteger ("ChestResult",3);
			StartCoroutine (DelayEnd (2f, false, 0));
		} else {
			connectingPanel.Connecting (true);
			DBManager.API.SubmitBidRoyaleResult(auctionId,round,curChoice,
				(response) =>
				{
					connectingPanel.Connecting (false);
					Debug.Log ("Exit Royale");
					JSONNode jsonData = JSON.Parse(response);
					int timeToPopulateServerData = jsonData["timeToServerPopulateData"];
                    correctAnswer = jsonData["correctAnswer"].AsInt;
					bool win = (correctAnswer == curChoice) ? true : false;

					resultPanel.gameObject.SetActive (true);
					resultPanel.SetInteger ("ChestResult",win ? 1 : 2);
                    if (win)
                    {
                        SoundManager.Instance.PlaySFX(SFXList.ChestOpen);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySFX(SFXList.ChestWrong);
                    }
					StartCoroutine (DelayEnd (2f, win, timeToPopulateServerData-2000));
				},
				(error) =>
				{
					connectingPanel.Connecting (false);
					gameManager.PopUpBackToHome(error);
				}
			);
		}
	}

	IEnumerator DelayEnd(float secs, bool win,int timeToPopulateServerData) {
		yield return new WaitForSeconds (secs);
		gameManager.EndRoyale (win, timeToPopulateServerData, correctAnswer);
	}


	protected override void EndGame(bool finish) {
		CheckResult ();
	}
}
