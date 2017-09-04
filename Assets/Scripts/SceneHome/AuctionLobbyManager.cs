using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuctionLobbyManager : MonoBehaviour {
	public Fader fader;
	public AuctionItemAreaManager auctionItemAreaManager;
	public GameObject panelAuctionScreen;
	public GameObject panelProductDetail;
	public GameObject panelClaimConfirmation;
	public Transform buttonJoin;

	void OnEnable(){
		fader.OnFadeOutFinished += OnFadeOutFinished;
	}

	void OnDisable(){
		fader.OnFadeOutFinished -= OnFadeOutFinished;
	}

	void OnFadeOutFinished ()
	{
		fader.FadeIn();
		panelAuctionScreen.SetActive(true);
		this.gameObject.SetActive(false);
	}



	public void OnClickEnter(){
		fader.FadeOut();
	}

	public void OnClickJoin (){
		auctionItemAreaManager.UpdateJoinButton();
	}

	public void OnClickClaim(){
		panelClaimConfirmation.SetActive(true);
	}

	public void OnClickProductDetail(){
		panelProductDetail.SetActive(true);
	}

	public void OnClickCloseProductDetail(){
		panelProductDetail.SetActive(false);
	}
}
