using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceGame : BaseGame {
	Color[] tileColors = new Color[2]{new Color(0.91f,0.29f,0.29f,1),new Color(0.07f,0.71f,0.83f,1)};

	public Transform tileArea;
	public int minNumber;
	public int maxNumber;
	public int displayNumber;
	List<int> sequence;

	public override void InitGame (int gameTime, int round)
	{
		List<int> number = new List<int> ();
		for (int i = minNumber; i <= maxNumber; i++) {
			number.Add (i);
		}
		List<Text> tiles = new List<Text> ();
		for (int i = 0; i < tileArea.childCount; i++) {
			tiles.Add (tileArea.GetChild(i).GetChild(0).GetComponent<Text>());
			tiles [i].text = "";
			tiles [i].transform.parent.GetComponent<Image>().color = tileColors[1];
		}
		sequence = new List<int> ();
		for (int i = 0; i < displayNumber; i++) {
			int numIdx = Random.Range (0,number.Count);
			int tileIdx = Random.Range (0,tiles.Count);

			tiles [tileIdx].text = "" + number [numIdx];
			sequence.Add (number [numIdx]);

			number.RemoveAt (numIdx);
			tiles.RemoveAt (tileIdx);
		}
		sequence.Sort ();

		base.InitGame (gameTime, round);
	}

	public void ClickTiles(GameObject tile) {
		Image tileBG = tile.GetComponent<Image> ();
		Text tileLabel = tile.transform.GetChild (0).GetComponent<Text> ();

		if (System.Convert.ToInt32 (tileLabel.text) == sequence [0]) {
			SoundManager.Instance.PlaySFX(SFXList.Button01);

			tileBG.color = tileColors [0];
			sequence.RemoveAt (0);

			if (sequence.Count == 0) {
				EndGame (true);
			}

		} else {
			SoundManager.Instance.PlaySFX(SFXList.Button02);
		}
	}

}
