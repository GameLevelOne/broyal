using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionScreenManager : BasePage {
	public Fader fader;
	public Animator panelAnimator;
	public GameObject panelPreGameScreen;
	public GameObject panelAuctionLobby;

	public int auctionIndex;

	string swipeLeft = "swipeLeft";
	string swipeRight = "swipeRight";

	public void OnClickBid(){
		fader.FadeOut();
	}

	public void OnClickBack(){		
		NextPage ("LOBBY");
	}

	public void OnClickNext(){
		panelAnimator.SetTrigger(swipeLeft);
	}

	public void OnClickPrev(){
		panelAnimator.SetTrigger(swipeRight);
	}

}
