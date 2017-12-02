using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class LandingPageManager : BasePage {
	public Fader fader;
	public AuctionLobbyManager auctionLobby;
	public GameObject panelCompleteProfile;
	public ImageLoader bidRoyalPic;
	public ImageLoader bidRumblePic;

	void Start ()
	{
//		LocalizationService.Instance.Localization = "English";
        SoundManager.Instance.PlayBGM(BGMList.BGMMenu02);
		if (FBManager.Instance != null) {
			if (FBManager.Instance.FBLogin) {
				Debug.Log ("fb");
				panelCompleteProfile.SetActive (true);
			}
		}
        GameMode gameMode = (GameMode)PlayerPrefs.GetInt("GameMode", 0);
        if (gameMode != GameMode.TRAINING)
        {
            fader.FadeIn();
        }
	}

	protected override void Init ()
	{
		base.Init ();
		bidRoyalPic.SetLoading();
		bidRumblePic.SetLoading();
		DBManager.API.GetLandingAuctionData (
			(response) => {
				JSONNode jsonData = JSON.Parse(response);
				bidRoyalPic.LoadImageFromUrl(jsonData["bidRoyaleAuction"]["productImage"]);
				bidRumblePic.LoadImageFromUrl(jsonData["bidRumbleAuction"]["productImage"]);

				GetComponent<Animator>().ResetTrigger ("Intro");
				GetComponent<Animator>().ResetTrigger ("Outro");
			},
			(error) => {
				bidRoyalPic.SetError();
				bidRumblePic.SetError();
			}
		);
	}

	public void BidRoyaleClicked()
	{
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		auctionLobby.auctionMode = AuctionMode.BIDROYALE;
		auctionLobby.auctionIndex = 0;
		NextPage ("LOBBY");
	}

	public void BidRumbleClicked()
	{
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        auctionLobby.auctionMode = AuctionMode.BIDRUMBLE;
		auctionLobby.auctionIndex = 0;
		NextPage ("LOBBY");
	}

}
