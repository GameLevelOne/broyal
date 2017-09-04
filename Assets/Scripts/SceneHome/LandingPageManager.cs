using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPageManager : MonoBehaviour {
	public Fader fader;
	public GameObject panelAuctionLobby;
	public GameObject panelCompleteProfile;

	void OnEnable(){
		Fader.OnFadeOutFinished += OnFadeOutFinished;
	}

	void OnDisable(){
		Fader.OnFadeOutFinished -= OnFadeOutFinished;
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
	}

	public void OnClickBid(){
		fader.FadeOut();
	}

}
