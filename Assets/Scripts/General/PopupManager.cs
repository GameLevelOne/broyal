using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour {
	public Animator popupAnimator;
	public Text msgText;
	string panelOpen = "panelOpen";
	string panelClose = "panelClose";

	public void SetText(string text){
		msgText.text = text;
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
