using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct RumbleScoreData {
	public int rank;
	public string username;
	public float score;

    public string ToString()
    {
        return "Data[Rank: " + rank + ", User: " + username + ", Score: " + score + "]";
    }
}

public class RumbleScoreItem : MonoBehaviour {

	public Text nameLabel;
	public Text timeLabel;

	public void InitItem(RumbleScoreData scoreData) {
		nameLabel.text = "" + scoreData.rank + ". " + scoreData.username;
		timeLabel.text = scoreData.score.ToString ("00.0000");
	}
}
