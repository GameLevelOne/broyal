using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using SimpleJSON;

public class SignInManager : AppInitPages {
	public Fader fader;
	public ConnectingPanel connectingPanel;
	public UniWebView uniWebView;
	public LoadingProgress panelLoading;
	public ForgotPasswordManager panelForgotPassword;
	public SignUpManager panelSignUp;
	public NotificationPopUp notificationPopUp;
	public VerifyOTPPopUp verifyOTPPopUp;

	public InputField userNameInput;
	public InputField passwordInput;

	string signInUsername;
	string signInPassword;

	bool webViewReady = false;

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
			if (signInUsername.StartsWith ("FB_")) {
				if (FBManager.Instance.FBLogin) {
					ClickFBLogin (true);
				} else {
					fader.FadeIn ();
				};
			} else {
				DoLogin(true);
			}
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

	public void ClickFBLogin (bool fadeIn){
		connectingPanel.Connecting (true);
		FBManager.Instance.OnFBLogin(
			(response) => {
				FBData fbData = FBManager.Instance.fbData;
				DBManager.API.UserSocialLogin(fbData.firstName,fbData.lastName,fbData.gender,fbData.accessToken,fbData.id,fbData.email,
					(response2)=>{
						connectingPanel.Connecting (false);
						Activate(false);
						if (fadeIn) {
							fader.OnFadeInFinished+= FadeInToLoading;
							fader.FadeIn();				
						} else {
							OnFinishOutro += FadeOutToLoading;
						}
					},
					(error2)=>{
						connectingPanel.Connecting (false);
						JSONNode jsonData = JSON.Parse (error2);
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
			},
			(error) => {
				connectingPanel.Connecting (false);
				if (fadeIn) {
					fader.FadeIn();
				} else {
					JSONNode jsonData = JSON.Parse (error);
					if (jsonData["errors"]!="CANCELLED") {
						notificationPopUp.ShowPopUp (jsonData["errors"]);
					}

					int debugIndex = DBManager.API.debugConsole.SetRequest("FB LOGIN");
					DBManager.API.debugConsole.SetResult(jsonData["errors"],debugIndex);
				}
			}
		);
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

				if (fadeIn) 
					DBManager.API.RestartApp();
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

	public void LoadWebView() {
//		if (webViewReady) {
//			uniWebView.OnWebViewShouldClose += ShouldClose;
//			uniWebView.Show(true);
//		} else {
//			connectingPanel.Connecting (true); 
//			uniWebView.Load ();
//			uniWebView.OnLoadComplete += LoadComplete;
//		}
//
	}

	void LoadComplete(UniWebView webView, bool success, string errorMessage) {
		webView.OnLoadComplete -= LoadComplete;
		connectingPanel.Connecting (false); 
		if (success) {
			webView.OnWebViewShouldClose += ShouldClose;
			webView.Show(true);
			webViewReady = true;
		} else {
			Debug.Log("Something wrong in webview loading: " + errorMessage);
		}
	}

	bool ShouldClose(UniWebView webView) {
		webView.OnWebViewShouldClose -= ShouldClose;
		webView.Hide (true);
		return false;
	}

		
}
