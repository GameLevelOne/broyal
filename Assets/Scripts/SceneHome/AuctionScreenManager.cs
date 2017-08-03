using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionScreenManager : MonoBehaviour {
	public Animator panelAnimator;
	public GameObject panelPreGameScreen;
	public GameObject panelAuctionLobby;

	string swipeLeft = "swipeLeft";
	string swipeRight = "swipeRight";

	public void OnClickBid(){
		PreGameManager preGameManager = panelPreGameScreen.GetComponent<PreGameManager>();
		panelPreGameScreen.SetActive(true);
		this.gameObject.SetActive(false);
		preGameManager.StartCountdown();
	}

	public void OnClickBack(){
		panelAuctionLobby.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void OnClickNext(){
		panelAnimator.SetTrigger(swipeLeft);
	}

	public void OnClickPrev(){
		panelAnimator.SetTrigger(swipeRight);
	}

}
