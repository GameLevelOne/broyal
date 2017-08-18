using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPageManager : MonoBehaviour {
	public GameObject panelAuctionLobby;
	public GameObject panelCompleteProfile;

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
		panelAuctionLobby.SetActive(true);
		this.gameObject.SetActive(false);
	}

}
