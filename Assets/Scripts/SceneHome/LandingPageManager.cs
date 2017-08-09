using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPageManager : MonoBehaviour {

	public GameObject panelAuctionLobby;
	public GameObject panelCompleteProfile;

	void Start ()
	{
		Debug.Log(FBManager.Instance.FBLogin);
		if (FBManager.Instance.FBLogin) {
			Debug.Log("fb");
			panelCompleteProfile.SetActive(true);
		}
	}

	public void OnClickBid(){
		this.gameObject.SetActive(false);
		panelAuctionLobby.SetActive(true);
	}
}
