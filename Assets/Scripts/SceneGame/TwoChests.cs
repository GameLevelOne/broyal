﻿using System.Collections;
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

	int curChoice;
	int auctionId;
	int round;

	public override void InitGame(int gameTime, int round) {
		chestBrownButton.SetInteger ("ChestState",0);
		chestRedButton.SetInteger ("ChestState",0);
		resultPanel.SetInteger ("ChestResult",0);
		resultPanel.gameObject.SetActive (false);
		curChoice = -1;
		auctionId = PlayerPrefs.GetInt("GameAuctionId", 0);
		round = PlayerPrefs.GetInt("GameRound", 0);

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
			StartCoroutine (DelayEnd (2f, false));
		} else {
			connectingPanel.Connecting (true);
			DBManager.API.SubmitBidRoyaleResult(auctionId,round,curChoice,
				(response) =>
				{
					connectingPanel.Connecting (false);
					Debug.Log ("Exit Royale");
					JSONNode jsonData = JSON.Parse(response);
					int timeToPopulateServerData = jsonData["timeToPopulateServerData"];
					bool win = (jsonData["correctAnswer"].AsInt == curChoice) ? true : false;

					resultPanel.gameObject.SetActive (true);
					resultPanel.SetInteger ("ChestResult",win ? 1 : 2);
					StartCoroutine (DelayEnd (2f, win, timeToPopulateServerData));
				},
				(error) =>
				{
					connectingPanel.Connecting (false);
					gameManager.PopUpBackToHome();
				}
			);
		}
	}

	IEnumerator DelayEnd(float secs, bool win,int timeToPopulateServerData=0) {
		yield return new WaitForSeconds (secs);
		gameManager.EndRoyale (win, timeToPopulateServerData, curChoice);
	}


	protected override void EndGame(bool finish) {
		CheckResult ();
	}
}
