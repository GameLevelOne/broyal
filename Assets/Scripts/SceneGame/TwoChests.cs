using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum ChestSprites{
	BrownEmpty,BrownFilled,RedEmpty,RedFilled
}

public class TwoChests : MonoBehaviour {
	public GameObject panelResult;
	public PanelGameReady panelGameReady;
	public GameObject panelScores;

	public GameObject panelTimeOut;
	public GameObject chestBrownButton;
	public GameObject chestRedButton;
	public Text timerText;

	public Text resultText;

	public Image chestBigImage;
	public Sprite[] chestSprite = new Sprite[4]; 

	List<int> scores = new List<int>();

	Coroutine currCor = null;

	int currentPlayer = 0;
	int treasureNo = 0;

	string triggerResultRight = "resultRight";
	string triggerResultWrong = "resultWrong";

	void Start ()
	{
//		for (int i = 0; i < totalPlayers; i++) {
//			scores.Add(0);
//		}
		
	}

	void OnEnable(){
		StartTimer ();
	}

	public void OnClickChest (int chest)
	{
		StopTimer();

		treasureNo = Random.Range (0, 2);

		if (treasureNo == chest) {
			panelScores.GetComponent<PanelScoresManager>().RightAnswer=true;
			UpdateResultDisplay(true,chest);	
		} else {
			panelScores.GetComponent<PanelScoresManager>().RightAnswer=false;
			UpdateResultDisplay(false,chest);
		}
	}

	public void OnClickResult(){
		panelScores.SetActive(true);
		this.gameObject.SetActive(false);
	}

	void UpdateResultDisplay (bool rightAnswer, int chestColor)
	{
		Animator resultAnim = panelResult.GetComponent<Animator> ();
		if (rightAnswer) {
			if (chestColor == 0) {
				chestBigImage.sprite = chestSprite [(int)ChestSprites.BrownFilled];
			} else if (chestColor == 1) {
				chestBigImage.sprite = chestSprite [(int)ChestSprites.RedFilled];
			}
			resultAnim.SetTrigger (triggerResultRight);
			resultText.text = "CONGRATULATIONS!";
		} else {
			if (chestColor == 0) {
				chestBigImage.sprite = chestSprite [(int)ChestSprites.BrownEmpty];
			} else if (chestColor == 1) {
				chestBigImage.sprite = chestSprite [(int)ChestSprites.RedEmpty];
			}
			resultAnim.SetTrigger (triggerResultWrong);
			resultText.text = "OOPS! TOO BAD...";
		}
		panelResult.GetComponent<Image> ().enabled = true;
	}

	void NextRound ()
	{
		scores.RemoveAll(t => t == 0);
		Debug.Log("Reset round, current players: "+scores.Count);
		currentPlayer = 0;
	}

	static bool ScoreIsZero (int i)
	{
		if (i == 0) {
			return true;
		} else 
		return false;
	}

	void StartTimer(){
		currCor = StartCoroutine(StartCountdown());
		Debug.Log("start timer");
	}

	void StopTimer (){
		StopCoroutine(currCor);
		Debug.Log("timer stopped");
	}

	void ShowChestAfterTimeOut ()
	{
		if (treasureNo == 0) {
			chestBrownButton.transform.GetChild (0).GetComponent<Image> ().sprite = chestSprite [(int)ChestSprites.BrownFilled];
			chestRedButton.transform.GetChild (0).GetComponent<Image> ().sprite = chestSprite [(int)ChestSprites.RedEmpty];
		} else {
			chestBrownButton.transform.GetChild(0).GetComponent<Image>().sprite = chestSprite[(int)ChestSprites.BrownEmpty];
			chestRedButton.transform.GetChild(0).GetComponent<Image>().sprite = chestSprite[(int)ChestSprites.RedFilled];
		}

		chestBrownButton.GetComponent<Animator>().SetTrigger("showResult");
		chestRedButton.GetComponent<Animator>().SetTrigger("showResult");
	}

	IEnumerator StartCountdown ()
	{
		for (int i = 9; i >= 0; i--) {
			yield return new WaitForSeconds(1);
			timerText.text = "0"+i.ToString();
		}
		ShowChestAfterTimeOut();
		yield return new WaitForSeconds(2);
		panelTimeOut.SetActive(true);
	}
}
