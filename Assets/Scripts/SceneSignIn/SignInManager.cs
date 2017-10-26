using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using SimpleJSON;

public class SignInManager : AppInitPages {
	public Fader fader;
	public ConnectingPanel connectingPanel;
	public LoadingProgress panelLoading;
	public GameObject panelForgotPassword;
	public GameObject panelForgotPassword2;
	public SignUpManager panelSignUp;
	public NotificationPopUp notificationPopUp;

	string signInUsername;
	string signInPassword;

	void Start() {
		fader.SetFaderActive (true);
		OnFinishIntro += CheckPreviousSignIn;
	}

	void CheckPreviousSignIn(){
		OnFinishIntro -= CheckPreviousSignIn;
//		Debug.Log ("CheckSignIn");
		signInUsername = PlayerPrefs.GetString("LastUserLogin","");
		signInPassword = PlayerPrefs.GetString("LastUserPassword","");

		if (string.IsNullOrEmpty(signInUsername)) {
			SoundManager.Instance.PlayBGM(BGMList.BGMMenu01);
			fader.FadeIn();
		}else{
			DoLogin(true);
		}
	} 

	public void GetInputUsername (InputField obj){
		signInUsername = obj.text;
	}

	public void GetInputPassword (InputField obj){
		signInPassword = obj.text;
	}

	public void ClickEnter(){
		//go to loading scene
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		if (string.IsNullOrEmpty (signInUsername) || string.IsNullOrEmpty (signInPassword)) {
			notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("SignIn.PLEASE_ENTER"));
		} else {
			DoLogin(false);
		}
	}

	public void ClickFBLogin (){
		FBManager.Instance.OnFBLogin();
        SoundManager.Instance.PlaySFX(SFXList.Button01);
    }

	public void ClickForgotPassword(){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
	}

	public void ClickSignUp(){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		CloseAndGoToNextPage (panelSignUp);
	}

	void DoLogin(bool fadeIn){
		connectingPanel.Connecting (true);
//		Debug.Log ("------Do Login");
		DBManager.API.UserLogin(signInUsername,signInPassword,
			(response)=>{
				connectingPanel.Connecting (false);
				Activate(false);
				if (fadeIn) {
					fader.OnFadeInFinished+= FadeInToLoading;
					fader.FadeIn();				
				} else {
					fader.OnFadeOutFinished+= FadeOutToLoading;
					fader.FadeOut();				
				}
			},
			(error)=>{
//				Debug.Log ("------Do Login Fail");
				connectingPanel.Connecting (false);
				notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("SignIn.WRONG"));
			}
		);
	}

	void FadeOutToLoading() {
		fader.OnFadeOutFinished-= FadeOutToLoading;
		panelLoading.gameObject.SetActive(true);
	}
	void FadeInToLoading() {
		fader.OnFadeInFinished-= FadeInToLoading;
		panelLoading.gameObject.SetActive(true);
	}
		
}
