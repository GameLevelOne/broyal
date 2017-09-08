using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAnimation : MonoBehaviour {
	public Animator popupAnimator;
	string panelOpen = "panelOpen";
	string panelClose = "panelClose";

	// Use this for initialization
	void Start () {
	}

	public void OpenPanel(){
		gameObject.SetActive (true);
		popupAnimator.SetTrigger(panelOpen);
	}

	public void ClosePanel(){
		popupAnimator.SetTrigger(panelClose);
	}

	public void OnClosePanel(){
		gameObject.SetActive(false);
	}
}
