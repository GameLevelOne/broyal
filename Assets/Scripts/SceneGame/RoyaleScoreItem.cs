using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct RoyaleScoreData {
	public int round;
	public int answer;
	public bool correct;
	public int passed;
    public string ToString()
    {
        return "Data[Round: " + round + ", Answer: " + answer + ", Correct: " + correct + ", Passed: " + passed + "]";
    }
}

public class RoyaleScoreItem : MonoBehaviour {

	public Text roundLabel;
	public Sprite[] correctChest;
	public Sprite[] wrongChest;
	public Image[] chests;
	public Text passedLabel;


	public void InitItem(RoyaleScoreData scoreData) {
		roundLabel.text = "" + scoreData.round;
		chests [scoreData.answer].sprite = scoreData.correct ? correctChest[scoreData.answer] : wrongChest[scoreData.answer];
		chests [scoreData.answer].color = Color.white;
		passedLabel.text = scoreData.passed.ToString("N0");
	}
}
