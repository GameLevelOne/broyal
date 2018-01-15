using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeTheCode : BaseGame {
	public Text problemLabel;

	public Text[] answerLabel;
	string answer;
	string problem;

	public override void InitGame (int gameTime, int round)
	{
		problem = "";
		answer = "";
		for(int i=0;i<answerLabel.Length;i++){
			char c = (char)('A'+Random.Range(0,26));
            answerLabel[i].color = Color.white;
            answerLabel[i].text = "";
            problem += c;
		}
		problemLabel.text = problem;

		base.InitGame (gameTime, round);
	}

	public void ClickKeyboard(string c) {
		int curIdx = answer.Length;
		if (c == "BACK") {
			SoundManager.Instance.PlaySFX(SFXList.Button02);
			if (curIdx > 0) {
				answerLabel [curIdx-1].text = "";
				answer = answer.Substring (0, curIdx - 1);
			}
		} else {
			SoundManager.Instance.PlaySFX(SFXList.Button01);
			if (curIdx < answerLabel.Length) {

				if (problem [curIdx] == c[0]) {
					answerLabel [curIdx].color = Color.white;
				} else {
					answerLabel [curIdx].color = Color.red;
				}

				answerLabel [curIdx].text = c;
				answer += c;

				//Check Answer
				if (problem == answer) {
					EndGame (true);
				}
			}
		}
	}
		
}
