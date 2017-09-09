using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelGameReady : MonoBehaviour {
	public GameObject panelGame;
	public GameObject panelRemainingPlayers;
	public Text textRemainingPlayers;
	public Text textReady;
	public Text textRound;

	int totalPlayers = 1000;
	int roundCount = 0;

	public int TotalPlayers { 
		set { totalPlayers = value;}
		get	{ return totalPlayers;}
	}

	public int RoundCount { 
		get	{ return roundCount;}
	}

	void OnEnable(){
		StartCountdown();
	}

	public void StartCountdown ()
	{
		if (totalPlayers > 0) {
			roundCount++;
			SetTextDisplay(roundCount,totalPlayers);
			StartCoroutine (ReadySetGo ());
		}
	}

	public bool CheckNextRound (){
		return true;
	}

	void SetTextDisplay(int roundCount, int playerCount){
		textRound.text = "Round "+roundCount.ToString();
		textRemainingPlayers.text = playerCount.ToString();
	}

	IEnumerator ReadySetGo (){
		yield return new WaitForSeconds(3);
		textReady.gameObject.SetActive(true);
		panelRemainingPlayers.SetActive(false);
		yield return new WaitForSeconds(1);
		textReady.text = "GET SET!";
		yield return new WaitForSeconds(1);
		textReady.text = "GO!";
		yield return new WaitForSeconds(1);
		this.gameObject.SetActive(false);
//		SoundManager.Instance.PlaySFX(SFXList.GameStart);
		panelGame.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
