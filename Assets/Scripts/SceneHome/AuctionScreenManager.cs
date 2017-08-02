using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionScreenManager : MonoBehaviour {
	public PreGameManager preGameManager;
	GameObject panelPreGameScreen;

	public void OnClickBid(){
		panelPreGameScreen = preGameManager.gameObject;
		panelPreGameScreen.SetActive(true);
		this.gameObject.SetActive(false);
		preGameManager.StartCountdown();
	}
}
