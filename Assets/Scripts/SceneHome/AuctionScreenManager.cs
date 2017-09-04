using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionScreenManager : MonoBehaviour {
	public Fader fader;
	public Animator panelAnimator;
	public GameObject panelPreGameScreen;
	public GameObject panelAuctionLobby;

	string swipeLeft = "swipeLeft";
	string swipeRight = "swipeRight";

	void OnEnable(){
		fader.OnFadeOutFinished += OnFadeOutFinished;
	}

	void OnDisable(){
		fader.OnFadeOutFinished -= OnFadeOutFinished;
	}

	void OnFadeOutFinished(){
		PreGameManager preGameManager = panelPreGameScreen.GetComponent<PreGameManager>();
		panelPreGameScreen.SetActive(true);
		this.gameObject.SetActive(false);
		preGameManager.StartCountdown();
		fader.FadeIn();
	}

	public void OnClickBid(){
		fader.FadeOut();
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
