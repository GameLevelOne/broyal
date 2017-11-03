using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct RoyaleScoreData {
	public int round;
	public int answer;
	public int passed;
}

public class RoyaleScoreItem : MonoBehaviour {

	public Text roundLabel;
	public Sprite[] correctChest;
	public Image[] chests;
	public Text passedLabel;


	public void InitItem(RoyaleScoreData scoreData) {
		roundLabel.text = "" + scoreData.round;
		chests [scoreData.answer].sprite = correctChest[scoreData.answer];
		chests [scoreData.answer].color = Color.white;
		passedLabel.text = scoreData.passed.ToString("N0");
	}
}
