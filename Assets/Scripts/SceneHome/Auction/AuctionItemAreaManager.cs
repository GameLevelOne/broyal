using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionItemAreaManager : MonoBehaviour {
	public GameObject BGButtonDisabled;
	public GameObject BGButtonEnabled;
	public GameObject textEnter;
	public GameObject textJoin;

	public void UpdateJoinButton (){
		BGButtonEnabled.SetActive(false);
		BGButtonDisabled.SetActive(true);
		textEnter.SetActive(true);
		textJoin.SetActive(false);
	}
	
}
