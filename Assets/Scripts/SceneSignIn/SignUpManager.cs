using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

enum MessageType{
	RegisterSuccess,
	RegisterError,
	VerifySuccess,
	VerifyError
}

public class SignUpManager : AppInitPages {
	public Fader fader;
	public ConnectingPanel connectingPanel;
	public LoadingProgress panelLoading;
	public NotificationPopUp notificationPopUp;
	public VerifyOTPPopUp verifyOTPPopUp;
	public SignInManager panelSignIn;
	public TextManager textManager;

	public InputField usernameInput;
	public InputField emailInput;
	public InputField phoneInput;
	public InputField passwordInput;
	public InputField confirmInput;
	public Toggle maleInput;
	public Toggle femaleInput;
	public Toggle subscribeInput;
	public Toggle tncInput;

	string signUpUsername;
	string signUpEmail;
	string signUpPhone;
	string signUpPassword;
	string signUpConfirmPassword;

	int genderOption; // 1 = male, 2 = female

	bool subscribe;
	bool tnc;

	protected override void Init ()
	{
		usernameInput.text = "";
		emailInput.text = "";
		phoneInput.text = "";
		passwordInput.text = "";
		confirmInput.text = "";
		maleInput.isOn = false;
		femaleInput.isOn = false;
		subscribeInput.isOn = true;
		tncInput.isOn = false;
		base.Init ();
	}

	public void ClickBack() {
		SoundManager.Instance.PlaySFX(SFXList.Button02);
		CloseAndGoToNextPage (panelSignIn);

	}

	public void ClickContinue (){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		GetAllFields ();
        CheckInputContents();
	}

	void GetAllFields() {
		signUpUsername = usernameInput.text;
		signUpEmail = emailInput.text;
		signUpPhone = phoneInput.text;
		signUpPassword = passwordInput.text;
		signUpConfirmPassword = confirmInput.text;

		genderOption = 0;
		if (maleInput.isOn) {
			genderOption = 1;
		} else if (femaleInput.isOn) {
			genderOption = 2;
		}

		subscribe = subscribeInput.isOn;
		tnc = tncInput.isOn;

	}

	void CheckInputContents () //missing check username/email/phonenumber
	{
		if ((string.IsNullOrEmpty (signUpUsername)) || (string.IsNullOrEmpty (signUpEmail)) || (string.IsNullOrEmpty (signUpPhone))
			|| (string.IsNullOrEmpty (signUpPassword)) || (string.IsNullOrEmpty (signUpConfirmPassword)) ) {
			notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("SignUp.FILL_ALL"));
		} else if (!IsEmailAddress (signUpEmail)) {
			notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("SignUp.ERROR_INCORRECT_EMAIL"));
		} else if (signUpPassword != signUpConfirmPassword) {
			notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("SignUp.ERROR_PASSWORD_MATCH"));
		} else if (genderOption == 0) {
			notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("SignUp.ERROR_GENDER"));
		} else if (!tnc) {
			notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("SignUp.ERROR_AGREE_TNC"));
		} else {
			DoRegister ();
		}
	}

	bool IsEmailAddress(string address) {
		int at = address.IndexOf ("@");
		int dot = address.IndexOf (".");

		if ((at >= 0) && (dot >= 0) && (at < dot))
			return true;
		else
			return false;
	}

	void DoRegister(){
		connectingPanel.Connecting(true);
		DBManager.API.UserRegistration(true,genderOption,signUpUsername,signUpPassword,signUpPhone,signUpEmail,
			(response)=>{
				connectingPanel.Connecting(false);
				Debug.Log("Register Success");
				verifyOTPPopUp.OnFinishOutro += OnVerifyClosed;
				verifyOTPPopUp.ShowPopUp(signUpUsername);
			},
			(error)=>{
				connectingPanel.Connecting(false);
				Debug.Log("Register Failed");
				JSONNode jsonData = JSON.Parse (error);
				if (jsonData!=null) {
					notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
				} else {
					notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
				}
			}
		);	
	}

	void OnVerifyClosed() {
		verifyOTPPopUp.OnFinishOutro -= OnVerifyClosed;
		if (verifyOTPPopUp.otpSuccess) {
			DoLogin ();
		} else {
			CloseAndGoToNextPage (panelSignIn);
		}
	}

	void DoLogin(){
		connectingPanel.Connecting (true);
        PlayerPrefs.SetInt("GameMode", 0);
		DBManager.API.UserLogin(signUpUsername,signUpPassword,
			(response)=>{
				DBManager.API.UpdateFCMToken(
					(response2) => {
						connectingPanel.Connecting (false);
						Activate(false);
						fader.OnFadeOutFinished+= FadeOutToLoading;
						fader.FadeOut();				
					},
					(error2) => {
						connectingPanel.Connecting (false);
						JSONNode jsonData = JSON.Parse (error2);
						if (jsonData!=null) {
							notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
						} else {
							notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
						}
						CloseAndGoToNextPage (panelSignIn);
					}
				);

			},
			(error)=>{
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse (error);
				if (jsonData!=null) {
					notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
				} else {
					notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
				}
				CloseAndGoToNextPage (panelSignIn);
			}
		);
	}
	void FadeOutToLoading() {
		fader.OnFadeOutFinished-= FadeOutToLoading;
		panelLoading.gameObject.SetActive(true);
	}

	public void ClickTermsConditions() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		//Activate (false);
		textManager.showType = 2;
		textManager.Activate (true);
	}

}
