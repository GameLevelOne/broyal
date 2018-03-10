using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SunCubeStudio.Localization;
using SimpleJSON;

public class SettingsManager : BasePage {
	public ConnectingPanel connectingPanel;
	public NotificationPopUp notifPopUp;
	public TutorialCarrouselPopUp tutorialCarrouselPopUp;
	public TextManager textPanel;
	public Image buttonNotifOn;
	public Image buttonNotifOff;
	public Image buttonSoundOn;
	public Image buttonSoundOff;
	public Image buttonLangEN;
	public Image buttonLangID;
	public Image buttonSubscribe;
	public Image buttonUnsubscribe;
	public Image buttonDebugOn;
	public Image buttonDebugOff;
	public GameObject debugArea;
	public BasePage prevPage;

	protected override void Init (){
		base.Init ();
		OnClickNotif(PlayerPrefs.GetInt ("Notifications",0)==1);
		OnClickSound(PlayerPrefs.GetFloat ("SoundVolume",1f)==1f);
		OnClickLanguage(LocalizationService.Instance.Localization == "English");
		OnClickSubscribe(PlayerPrefs.GetInt ("Subscribe",0)==1);
		ClickDebug(PlayerPrefs.GetInt ("ShowDebugConsole",0)==1);
		debugArea.SetActive (DBManager.API.config.debugMode);
	}

	public void OnClickClose (){
        SoundManager.Instance.PlaySFX(SFXList.Button02);
        PagesManager.instance.CurrentPageOutro(prevPage);
	}

	public void OnClickNotif (bool optionOn)
	{
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        buttonNotifOn.enabled = optionOn;
		buttonNotifOff.enabled = !optionOn;
		PlayerPrefs.SetInt ("Notifications",(optionOn ? 1 : 0));
	}

	public void OnClickSound (bool optionOn){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        buttonSoundOn.enabled = optionOn;
		buttonSoundOff.enabled = !optionOn;

		SoundManager.Instance.SetVolume (optionOn ? 1f : 0f);
		PlayerPrefs.SetFloat ("SoundVolume",(optionOn ? 1f : 0f));
	}

	public void OnClickLanguage (bool optionEN){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        buttonLangEN.enabled = optionEN;
		buttonLangID.enabled = !optionEN;
		if (optionEN) {
			LocalizationService.Instance.Localization = "English";
		} else {
			LocalizationService.Instance.Localization = "Bahasa";
		}
	}

	public void OnClickSubscribe (bool optionSubscribe){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        buttonSubscribe.enabled = optionSubscribe;
		buttonUnsubscribe.enabled = !optionSubscribe;
		connectingPanel.Connecting (true);
		if (optionSubscribe) {
			DBManager.API.SubscribeUser (
				(response) => {
					connectingPanel.Connecting (false);
					PlayerPrefs.SetInt ("Subscribe",1);
				}, 
				(error) => {
					connectingPanel.Connecting (false);
					JSONNode jsonData = JSON.Parse (error);
					if (jsonData!=null) {
						notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
					} else {
						notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
					}
				}
			);		
		} else {
			DBManager.API.UnsubscribeUser (
				(response) => {
					connectingPanel.Connecting (false);
					PlayerPrefs.SetInt ("Subscribe",0);
				}, 
				(error) => {
					connectingPanel.Connecting (false);
					JSONNode jsonData = JSON.Parse (error);
					if (jsonData!=null) {
						notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
					} else {
						notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
					}
				}
			);		
		}
	}

	public void ClickDebug(bool optionOn) {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		buttonDebugOn.enabled = optionOn;
		buttonDebugOff.enabled = !optionOn;
		PlayerPrefs.SetInt ("ShowDebugConsole",(optionOn ? 1 : 0));
		DBManager.API.debugConsole.Hide ();
	}

	public void ClickTextPages(int type){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		textPanel.showType = type;
		NextPage ("TEXTVIEW");
	}


	public void ClickTutorial(){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		tutorialCarrouselPopUp.Activate (true);

	}

	public void OnClickLogout(){
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		connectingPanel.Connecting (true);
		DBManager.API.UserLogout (
			(response) => {
				PlayerPrefs.DeleteKey ("LastUserLogin");
				Application.LoadLevel ("SceneSignIn");
			}, 
			(error) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse (error);
				if (jsonData!=null) {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
				} else {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
				}
			}
		);
	}
}
