using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPageManager : MonoBehaviour {

	public GameObject panelAuctionLobby;

	public void OnClickBid(){
		this.gameObject.SetActive(false);
		panelAuctionLobby.SetActive(true);
	}
}
