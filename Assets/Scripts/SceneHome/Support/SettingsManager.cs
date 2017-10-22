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
        SoundManager.Instance.PlaySFX(SFXList.Button02);
        PagesManager.instance.CurrentPageOutro(prevPage);
	}

	public void OnClickNotif (bool optionOn)
	{
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        buttonNotifOn.GetComponent<Image>().enabled = optionOn;
		buttonNotifOff.GetComponent<Image> ().enabled = !optionOn;
	}

	public void OnClickSound (bool optionOn){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        buttonSoundOn.GetComponent<Image>().enabled = optionOn;
		buttonSoundOff.GetComponent<Image> ().enabled = !optionOn;
	}

	public void OnClickLanguage (bool optionEN){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        buttonLangEN.GetComponent<Image>().enabled = optionEN;
		buttonLangID.GetComponent<Image> ().enabled = !optionEN;
		if (optionEN) {
			LocalizationService.Instance.Localization = "English";
		} else {
			LocalizationService.Instance.Localization = "Bahasa";
		}
	}

	public void OnClickSubscribe (bool optionSubscribe){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        buttonSubscribe.GetComponent<Image>().enabled = optionSubscribe;
		buttonUnsubscribe.GetComponent<Image> ().enabled = !optionSubscribe;
	}

	public void OnClickPrivacyPolicy(){
        SoundManager.Instance.PlaySFX(SFXList.Button01);

	}

	public void OnClickTandC(){
        SoundManager.Instance.PlaySFX(SFXList.Button01);

	}

	public void OnClickAboutUs (){
        SoundManager.Instance.PlaySFX(SFXList.Button01);

	}

	public void OnClickTutorial(){
        SoundManager.Instance.PlaySFX(SFXList.Button01);

	}

	public void OnClickLogout(){

	}
}
