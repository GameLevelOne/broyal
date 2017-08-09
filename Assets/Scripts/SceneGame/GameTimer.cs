using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {
	public GameObject panelTimeOut;
	public GameObject chestBrownAnim;
	public GameObject chestRedAnim;
	public Text timerText;

	int brownChestContent = -1;
	int redChestContent = -1;

	Coroutine currCor = null;

	public void StartTimer(){
		currCor = StartCoroutine(StartCountdown());
		Debug.Log("start timer");
	}

	public void StopTimer (){
		StopCoroutine(currCor);
		Debug.Log("timer stopped");
	}

	void ShowChestAfterTimeOut(){
		
		chestBrownAnim.GetComponent<Animator>().SetTrigger("showResult");
		chestRedAnim.GetComponent<Animator>().SetTrigger("showResult");
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
