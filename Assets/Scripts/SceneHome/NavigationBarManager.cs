using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationBarManager : MonoBehaviour {
	public GameObject panelAuctionLobby;
	public GameObject panelHome;

	public void OnClickHome(){
		panelAuctionLobby.SetActive(false);
		panelHome.SetActive(true);
	}
	
}
