using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {
	public GameObject panelLandingPage;
	public GameObject buttonNotifOn;
	public GameObject buttonNotifOff;
	public GameObject buttonSoundOn;
	public GameObject buttonSoundOff;
	public GameObject buttonLangEN;
	public GameObject buttonLangID;
	public GameObject buttonSubscribe;
	public GameObject buttonUnsubscribe;

	void OnEnable (){
		TempInit();
	}

	void TempInit (){
		OnClickNotif(true);
		OnClickSound(true);
		OnClickLanguage(true);
		OnClickSubscribe(true);
	}

	public void OnClickClose (){
		panelLandingPage.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void OnClickNotif (bool optionOn)
	{
		if (optionOn) {
			buttonNotifOn.GetComponent<Image> ().enabled = false;
			buttonNotifOff.GetComponent<Image> ().enabled = true;
		} else {
			buttonNotifOn.GetComponent<Image> ().enabled = true;
			buttonNotifOff.GetComponent<Image> ().enabled = false;
		}
	}

	public void OnClickSound (bool optionOn){
		if (optionOn) {
			buttonSoundOn.GetComponent<Image> ().enabled = false;
			buttonSoundOff.GetComponent<Image> ().enabled = true;
		} else {
			buttonSoundOn.GetComponent<Image> ().enabled = true;
			buttonSoundOff.GetComponent<Image> ().enabled = false;
		}
	}

	public void OnClickLanguage (bool optionEN){
		if (optionEN) {
			buttonLangEN.GetComponent<Image> ().enabled = false;
			buttonLangID.GetComponent<Image> ().enabled = true;
		} else {
			buttonLangEN.GetComponent<Image> ().enabled = true;
			buttonLangID.GetComponent<Image> ().enabled = false;
		}
	}

	public void OnClickSubscribe (bool optionSubscribe){
		if (optionSubscribe) {
			buttonSubscribe.GetComponent<Image> ().enabled = false;
			buttonUnsubscribe.GetComponent<Image> ().enabled = true;
		} else {
			buttonSubscribe.GetComponent<Image> ().enabled = true;
			buttonUnsubscribe.GetComponent<Image> ().enabled = false;
		}
	}

	public void OnClickPrivacyPolicy(){

	}

	public void OnClickTandC(){

	}

	public void OnClickAboutUs (){

	}

	public void OnClickTutorial(){

	}

	public void OnClickLogout(){

	}
}
