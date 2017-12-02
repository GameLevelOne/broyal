using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BidRoyale.Core;

public class ScoreBoardManager : PagesIntroOutro {
	public SceneGameManager gameManager;
	public GameObject rumbleBoard;
	public GameObject rumbleItem;
	public Transform rumbleContainer;
	public GameObject royaleBoard;
	public GameObject royaleItem;
	public Transform royaleContainer;

	public void InitScoreBoard(RumbleScoreData[] scoreData) {
		Debug.Log ("Init Score board Rumble");
		rumbleBoard.SetActive (true);
		royaleBoard.SetActive (false);
		Utilities.ClearChildren (rumbleContainer);

		for (int i = 0; i < scoreData.Length; i++) {
			GameObject g = Instantiate (rumbleItem,rumbleContainer);
			RumbleScoreItem rsi = g.GetComponent<RumbleScoreItem> ();
			rsi.InitItem (scoreData [i]);
		}
	}

	public void InitScoreBoard(RoyaleScoreData[] scoreData) {
		Debug.Log ("Init Score board Royale");
		rumbleBoard.SetActive (false);
		royaleBoard.SetActive (true);
		Utilities.ClearChildren (royaleContainer);

		for (int i = 0; i < scoreData.Length; i++) {
			GameObject g = Instantiate (royaleItem,royaleContainer);
			RoyaleScoreItem rsi = g.GetComponent<RoyaleScoreItem> ();
			rsi.InitItem (scoreData [i]);
		}
	}

	public void NextClick() {
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        gameManager.ExitGame();
	}

}
