using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeTheCode : MonoBehaviour {
	public PanelTrainingScores panelScore;
	public Keyboard keyboard;
	public Text countdownText;
	public Text problemText;
	public Transform parentAnswer;
	public GameObject answerObj;
	public GameObject overlay;

	Text[] answerObjArray = new Text[8];
	char[] stringAnswer = new char[8];
	char[] stringProblem = new char[8];

	int totalChar = 8;
	int totalInput = 0;

	bool gameOver = false;
	bool isWinning = false;

	float gameTimer = 0f;
	float gameTimeLimit = 6f;

	void OnEnable(){
		Keyboard.OnInputChar += HandleOnInputChar;
	}

	void OnDisable(){
		Keyboard.OnInputChar -= HandleOnInputChar;
	}

	void HandleOnInputChar ()
	{
		//backspace = 0;
		stringAnswer[totalInput] = keyboard.currInput;

		if(keyboard.currInput != '0'){
			answerObjArray[totalInput].text = keyboard.currInput.ToString();
			totalInput++;
		} else{
			if(totalInput<=0){
				totalInput=0;
			} else{
				totalInput--;
				answerObjArray [totalInput].text = string.Empty;
			}
		}

		CheckAnswer();
//		if(totalInput == totalChar){
//			overlay.SetActive(true);
//		} else{
//			overlay.SetActive(false);
//		}
	}

	void Start(){
		GenerateProblem();
		GenerateAnswerBox();
		StartCoroutine(GameTimer());
		StartCoroutine(CountPlayTime());
	}

	void GenerateProblem(){
		string temp = "";
		for(int i=0;i<totalChar;i++){
			char c = (char)('A'+Random.Range(0,26));
			stringProblem[i]=c;
			temp+=c;
		}
		problemText.text = temp;
	}

	void GenerateAnswerBox(){
		for(int i=0;i<totalChar;i++){
			GameObject obj = Instantiate(answerObj,parentAnswer,false);
			obj.transform.localPosition = new Vector3(-250+i*70,0,0);
			answerObjArray[i]=obj.GetComponent<Text>();
		}
	}

	void CheckAnswer ()
	{
		int totalTrue = 0;
		for (int i = 0; i < totalChar; i++) {
			Debug.Log (stringProblem [i] + " vs "+ stringAnswer [i]);
			if (stringProblem [i] == stringAnswer [i]) {
				totalTrue++;
				answerObjArray [i].color = Color.white;
			} else {
				answerObjArray [i].color = Color.red;
			}
		}
		if(totalTrue == totalChar){
			gameOver = true;
			isWinning = true;
			GameOver();
		}

	}

	void GameOver(){
		overlay.SetActive(true);
		if(isWinning){
			Debug.Log(gameTimer);
			StopAllCoroutines();
			panelScore.gameObject.SetActive(true);
			panelScore.SetScoreText(gameTimer.ToString());

		} else{
			gameTimer=0f;
			StartCoroutine(WaitForReset());
		}
	}

	void ResetAnswer(){
		for(int i=0;i<totalChar;i++){
			stringAnswer[i]=' ';
			answerObjArray[i].text = "";
			answerObjArray[i].color = Color.white;
		}
		GenerateProblem ();
		overlay.SetActive(false);
		totalInput=0;
		countdownText.text = "06";
		StopAllCoroutines ();
		StartCoroutine(GameTimer());
	}

	IEnumerator WaitForReset(){
		yield return new WaitForSeconds(2);
		ResetAnswer();
	}

	IEnumerator GameTimer(){
		countdownText.text = "06";
		for(int i=5;i>=0;i--){
			yield return new WaitForSeconds(1);
			countdownText.text = "0"+i.ToString();
		}
//		SoundManager.Instance.PlaySFX(SFXList.TimeUp);
		gameOver = true;
		overlay.SetActive(true);
		GameOver();
	}

	IEnumerator CountPlayTime(){
		while(gameTimer < gameTimeLimit){
			gameTimer += Time.deltaTime;
			yield return null;
		}
	}

}
