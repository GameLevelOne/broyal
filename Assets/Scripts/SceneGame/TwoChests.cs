using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoChests : MonoBehaviour {
	public Text textRightAns;
	public Text textWrongAns;

	List<int> scores = new List<int>();
	int totalPlayers = 10;
	int currentPlayer = 0;

	void Start ()
	{
		for (int i = 0; i < totalPlayers; i++) {
			scores.Add(0);
		}
	}

	public void OnClickChest (int chest)
	{
		if (currentPlayer < scores.Count) {
			currentPlayer++;
			int treasureNo = Random.Range (1, 3);

			if (treasureNo == chest) {
				Debug.Log ("Torejaa hakken!");
				UpdateScore (currentPlayer, 1);
			} else {
				Debug.Log ("Hazure!");
				UpdateScore (currentPlayer, 0);
			}
		} else {
			NextRound();
		}
	}

	void UpdateScore (int playerNo, int score)
	{
		scores [playerNo - 1] = score;
		if (score == 1) {
			textRightAns.text += "\nPlayer " + playerNo;
		} else {
			textWrongAns.text += "\nPlayer " + playerNo;
		}
	}

	void NextRound ()
	{
		scores.RemoveAll(t => t == 0);
		Debug.Log("Reset round, current players: "+scores.Count);
		currentPlayer = 0;
		textRightAns.text = "";
		textWrongAns.text = "";
	}

	static bool ScoreIsZero (int i)
	{
		if (i == 0) {
			return true;
		} else 
		return false;
	}
}
