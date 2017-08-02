using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelGameReady : MonoBehaviour {
	public GameObject panelGame;
	public Text textReady;

	void Start(){
		StartCoroutine(ReadySetGo());
	}

	IEnumerator ReadySetGo (){
		yield return new WaitForSeconds(1);
		textReady.text = "GET SET!";
		yield return new WaitForSeconds(1);
		textReady.text = "GO!";
		yield return new WaitForSeconds(1);
		panelGame.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
