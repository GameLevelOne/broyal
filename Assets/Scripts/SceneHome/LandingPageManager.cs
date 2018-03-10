using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class LandingPageManager : BasePage {
	public Fader fader;
	public TutorialCarrouselPopUp tutorialCarrouselPopUp;
	public AuctionLobbyManager auctionLobby;
//	public GameObject panelCompleteProfile;
	public ImageLoader bidRoyalPic;
	public ImageLoader bidRumblePic;
	public GameObject newsIcon;
	public Text newsNumber;

	void Start ()
	{
//		LocalizationService.Instance.Localization = "English";
        SoundManager.Instance.PlayBGM(BGMList.BGMMenu02);
		if (FBManager.Instance != null) {
			if (FBManager.Instance.FBLogin) {
//				Debug.Log ("fb");
//				panelCompleteProfile.SetActive (true);
			}
		}
        GameMode gameMode = (GameMode)PlayerPrefs.GetInt("GameMode", 0);
        if (gameMode != GameMode.TRAINING)
        {
            fader.FadeIn();
        }

		int firstTime = PlayerPrefs.GetInt ("FirstTime",1);
		if (firstTime == 1) {
			tutorialCarrouselPopUp.Activate (true);
			PlayerPrefs.SetInt ("FirstTime",0);
		}
	}

	protected override void Init ()
	{
		base.Init ();
		bidRoyalPic.SetLoading();
		bidRumblePic.SetLoading();

//		GameObject g = GameObject.FindGameObjectWithTag ("HangingNotification"); 
//
//		if (g != null) {
//			g.GetComponent<HangingNotification> ().ShowNotification ("Testing");
//		}

		DBManager.API.GetLandingAuctionData (
			(response) => {
				JSONNode jsonData = JSON.Parse(response);
				bidRoyalPic.LoadImageFromUrl(jsonData["bidRoyaleAuction"]["productImage"]);
				bidRumblePic.LoadImageFromUrl(jsonData["bidRumbleAuction"]["productImage"]);

				int news = jsonData["noOfUnReadNews"].AsInt;
				if (news<1) {
					newsIcon.SetActive(false);
				} else if (news>99) {
					newsIcon.SetActive(true);
					newsNumber.text = "99+";
				} else {
					newsIcon.SetActive(true);
					newsNumber.text = news.ToString();
				}

				GetComponent<Animator>().ResetTrigger ("Intro");
				GetComponent<Animator>().ResetTrigger ("Outro");
			},
			(error) => {
				bidRoyalPic.SetError();
				bidRumblePic.SetError();
				newsIcon.SetActive(false);
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

    void OnApplicationPause(bool isPaused)
    {
        if (!isPaused)
        {
            Init();
        }
    }

}
