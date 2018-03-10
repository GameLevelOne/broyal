using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HangingNotification : MonoBehaviour {

	public Text bodyLabel;
	public Animator anim;
	public GameObject container;

	void Start() {
		container.SetActive (false);
	}

	public void ShowNotification(string s) {
		bodyLabel.text = s;
		container.SetActive (true);
		anim.SetInteger ("Restart",0);
	}

	public void FinishShowing() {
		anim.SetInteger ("Restart",1);
		container.SetActive (false);
	}

	public void StartShowing() {
		anim.SetInteger ("Restart",1);
	} 
}
