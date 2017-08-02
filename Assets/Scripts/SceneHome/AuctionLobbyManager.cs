using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionLobbyManager : MonoBehaviour {
	public GameObject panelAuctionScreen;
	public GameObject panelProductDetail;

	public void OnClickEnter(){
		panelAuctionScreen.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void OnClickProductDetail(){
		panelProductDetail.SetActive(true);
	}

	public void OnClickCloseProductDetail(){
		panelProductDetail.SetActive(false);
	}
}
