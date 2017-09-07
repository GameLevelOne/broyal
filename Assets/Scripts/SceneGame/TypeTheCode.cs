using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeTheCode : MonoBehaviour {
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

		if(totalInput == totalChar){
			CheckAnswer();
			overlay.SetActive(true);
		} else{
			overlay.SetActive(false);
		}
	}

	void Start(){
		GenerateProblem();
		GenerateAnswerBox();
		StartCoroutine(GameTimer());
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
			if (stringProblem [i] == stringAnswer [i]) {
				totalTrue++;
			} else {
				answerObjArray [i].color = Color.red;
			}
		}

		if (!gameOver) {
			if (totalTrue == totalChar) {
				Debug.Log ("You win!");
			} else {
				StartCoroutine (WaitForReset ());
			}
		}
	}

	void ResetAnswer(){
		for(int i=0;i<totalChar;i++){
			stringAnswer[i]=' ';
			answerObjArray[i].text = "";
			answerObjArray[i].color = Color.white;
		}
		overlay.SetActive(false);
		totalInput=0;
	}

	IEnumerator WaitForReset(){
		yield return new WaitForSeconds(1);
		ResetAnswer();
	}

	IEnumerator GameTimer(){
		for(int i=5;i>=0;i--){
			yield return new WaitForSeconds(1);
			countdownText.text = "0"+i.ToString();
		}
		gameOver = true;
		overlay.SetActive(true);
	}

}
