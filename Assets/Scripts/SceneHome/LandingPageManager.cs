using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class LandingPageManager : MonoBehaviour {
	public Fader fader;
	public GameObject loadingPanel;
	public GameObject panelAuctionLobby;
	public GameObject panelCompleteProfile;
	public Text bidRoyalTagline;
	public Text bidRumbleTagline;

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
		DBManager.API.GetAuctionLandingData (
			(response) => {
				loadingPanel.SetActive(false);
				JSONNode jsonData = JSON.Parse(response);
				bidRoyalTagline.text = jsonData["bidRoyalAuction"]["productName"];
				bidRumbleTagline.text = jsonData["bidRumbleAuction"]["productName"];
			},
			(error) => {
				Debug.Log("masuk sini?");
				loadingPanel.SetActive(false);
			}
		);
	}

	public void OnClickBid(){
		fader.FadeOut();
	}

}
