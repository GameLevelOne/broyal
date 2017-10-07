using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SunCubeStudio.Localization;

public class SettingsManager : BasePage {
	public GameObject panelLandingPage;
	public GameObject buttonNotifOn;
	public GameObject buttonNotifOff;
	public GameObject buttonSoundOn;
	public GameObject buttonSoundOff;
	public GameObject buttonLangEN;
	public GameObject buttonLangID;
	public GameObject buttonSubscribe;
	public GameObject buttonUnsubscribe;
	public BasePage prevPage;

	protected override void Init (){
		base.Init ();
		OnClickNotif(true);
		OnClickSound(true);
		OnClickLanguage(LocalizationService.Instance.Localization == "English");
		OnClickSubscribe(true);
	}

	public void OnClickClose (){
//		panelLandingPage.SetActive(true);
		PagesManager.instance.CurrentPageOutro(prevPage);
	}

	public void OnClickNotif (bool optionOn)
	{
		buttonNotifOn.GetComponent<Image> ().enabled = optionOn;
		buttonNotifOff.GetComponent<Image> ().enabled = !optionOn;
	}

	public void OnClickSound (bool optionOn){
		buttonSoundOn.GetComponent<Image> ().enabled = optionOn;
		buttonSoundOff.GetComponent<Image> ().enabled = !optionOn;
	}

	public void OnClickLanguage (bool optionEN){
		buttonLangEN.GetComponent<Image> ().enabled = optionEN;
		buttonLangID.GetComponent<Image> ().enabled = !optionEN;
		if (optionEN) {
			LocalizationService.Instance.Localization = "English";
		} else {
			LocalizationService.Instance.Localization = "Bahasa";
		}
	}

	public void OnClickSubscribe (bool optionSubscribe){
		buttonSubscribe.GetComponent<Image> ().enabled = optionSubscribe;
		buttonUnsubscribe.GetComponent<Image> ().enabled = !optionSubscribe;
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
