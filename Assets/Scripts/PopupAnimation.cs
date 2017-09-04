using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAnimation : MonoBehaviour {
	Animator popupAnimator;
	string panelOpen = "panelOpen";
	string panelClose = "panelClose";

	// Use this for initialization
	void Start () {
	}

	void OnEnable(){
		popupAnimator = GetComponent<Animator>();
		OpenPanel();
	}

	public void OpenPanel(){
		popupAnimator.SetTrigger(panelOpen);
	}

	public void ClosePanel(){
		popupAnimator.SetTrigger(panelClose);
	}

	public void OnClosePanel(){
		Debug.Log("asd");
		this.gameObject.SetActive(false);
	}
}
