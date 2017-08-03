using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutManager : MonoBehaviour {

	public GameObject panelLandingPage;
	public GameObject panelAuctionLobby;

	public void OnClickBack(){ 
		panelLandingPage.SetActive(true);
		panelAuctionLobby.SetActive(false);
		this.gameObject.SetActive(false);
	}

}
