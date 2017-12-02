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
	public ForgotPasswordManager panelForgotPassword;
	public SignUpManager panelSignUp;
	public NotificationPopUp notificationPopUp;
	public VerifyOTPPopUp verifyOTPPopUp;

	public InputField userNameInput;
	public InputField passwordInput;

	string signInUsername;
	string signInPassword;

	void Start() {
		SoundManager.Instance.SetVolume (PlayerPrefs.GetFloat("SoundVolume",1f));
		fader.SetFaderActive (true);
		OnFinishIntro += CheckPreviousSignIn;
	}
		
	protected override void Init ()
	{
		userNameInput.text = "";
		passwordInput.text = "";
		base.Init ();
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

	public void ClickEnter(){
		//go to loading scene
        SoundManager.Instance.PlaySFX(SFXList.Button01);

		signInUsername = userNameInput.text;
		signInPassword = passwordInput.text;

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
		CloseAndGoToNextPage (panelForgotPassword);
	}

	public void ClickSignUp(){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		CloseAndGoToNextPage (panelSignUp);
	}

	void DoLogin(bool fadeIn){
		connectingPanel.Connecting (true);
        PlayerPrefs.SetInt("GameMode", 0);
        //Debug.Log ("------Do Login");
		DBManager.API.UserLogin(signInUsername,signInPassword,
			(response)=>{
				connectingPanel.Connecting (false);
				Activate(false);
				if (fadeIn) {
					fader.OnFadeInFinished+= FadeInToLoading;
					fader.FadeIn();				
				} else {
					OnFinishOutro += FadeOutToLoading;
				}
			},
			(error)=>{
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse (error);
				if (jsonData!=null) {
					notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
					if (jsonData["errors"]=="USER_NOT_VERIFIED") {
						notificationPopUp.OnFinishOutro += AfterNotVerified;
					}
				} else {
					notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
				}
			}
		);
	}

	void AfterNotVerified() {
		notificationPopUp.OnFinishOutro -= AfterNotVerified;
		verifyOTPPopUp.OnFinishOutro += OnVerifyClosed;
		verifyOTPPopUp.ShowPopUp(signInUsername);
	}

	void OnVerifyClosed() {
		verifyOTPPopUp.OnFinishOutro -= OnVerifyClosed;
		if (verifyOTPPopUp.otpSuccess) {
			DoLogin (false);
		}
	}

	void FadeOutToLoading() {
		OnFinishOutro -= FadeOutToLoading;
		panelLoading.gameObject.SetActive(true);
	}
	void FadeInToLoading() {
		fader.OnFadeInFinished-= FadeInToLoading;
		panelLoading.gameObject.SetActive(true);
	}
		
}
