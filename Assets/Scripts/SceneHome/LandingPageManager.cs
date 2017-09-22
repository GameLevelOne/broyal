using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class LandingPageManager : BasePage {
	public Fader fader;
	public GameObject loadingPanel;
	public GameObject panelAuctionLobby;
	public GameObject panelCompleteProfile;
	public ImageLoader bidRoyalPic;
	public ImageLoader bidRumblePic;

	void OnEnable(){
		fader.OnFadeOutFinished += OnFadeOutFinished;
	}

	void OnDisable(){
		fader.OnFadeOutFinished -= OnFadeOutFinished;
	}

	void OnFadeOutFinished ()
	{
		fader.FadeIn();
		panelAuctionLobby.SetActive(true);
		this.gameObject.SetActive(false);
	}

	void Start ()
	{
		if (FBManager.Instance != null) {
			if (FBManager.Instance.FBLogin) {
				Debug.Log ("fb");
				panelCompleteProfile.SetActive (true);
			}
		}

		fader.FadeIn ();
		fader.OnFadeInFinished += OnFadeInFinished;
	}

	void OnFadeInFinished()
	{
		fader.OnFadeInFinished -= OnFadeInFinished;

		loadingPanel.SetActive (true);
		DBManager.API.GetLandingAuctionData (
			(response) => {
				loadingPanel.SetActive(false);
				JSONNode jsonData = JSON.Parse(response);
				bidRoyalPic.LoadImageFromUrl(jsonData["bidRoyaleAuction"]["productImage"]);
				bidRumblePic.LoadImageFromUrl(jsonData["bidRumbleAuction"]["productImage"]);
			},
			(error) => {
				loadingPanel.SetActive(false);
				bidRoyalPic.SetError();
				bidRumblePic.SetError();
			}
		);
	}


	public void OnClickBid(){
		fader.FadeOut();
	}

}
