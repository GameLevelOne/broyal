using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class ForgotPasswordManager : AppInitPages {
	public ConnectingPanel connectingPanel;
	public NotificationPopUp notificationPopUp;

	public SignInManager panelSignIn;
	public GameObject panelSendOTP;
	public GameObject panelEnterOTP;

	public InputField userNameInput;
	public InputField otpInput;
	public InputField passwordInput;
	public InputField confirmInput;

	string username;
	string otp;
	string newPassword;
	string confirmPassword;

	protected override void Init ()
	{
		panelSendOTP.SetActive (true);
		panelEnterOTP.SetActive (false);

		userNameInput.text = "";
		otpInput.text = "";
		passwordInput.text = "";
		confirmInput.text = "";

		base.Init ();
	}

	public void ClickSend(){ //forgot 1 -> goto OTP page
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		username = userNameInput.text;
        DoRequestForgotPassword();
	}

	public void ClickEnter(){ //forgot 2 -> goto sign in page
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		otp = otpInput.text;
		newPassword = passwordInput.text;
		confirmPassword = confirmInput.text;
        DoResetPassword();
	}
		
	public void ClickSendBack() {
		SoundManager.Instance.PlaySFX(SFXList.Button02);
		CloseAndGoToNextPage (panelSignIn);
	}
	public void ClickEnterBack() {
		SoundManager.Instance.PlaySFX(SFXList.Button02);
		Init ();
	}

	void DoRequestForgotPassword (){		
		if(!string.IsNullOrEmpty(username)){
			connectingPanel.Connecting (true);
			DBManager.API.UserForgotPassword(username,
				(response)=>{
					JSONNode jsonData = JSON.Parse(response);
					connectingPanel.Connecting (false);
					panelSendOTP.SetActive(false);
					panelEnterOTP.SetActive(true);
				},
				(error)=>{
					connectingPanel.Connecting (false);
					notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Verify.FAILED_OTP"));
				}
			);
		} else{
			notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("ForgotPassword.PLEASE_INPUT"));
		}
	}

	void DoResetPassword ()
	{
		if ((string.IsNullOrEmpty (otp)) || (string.IsNullOrEmpty (newPassword)) || string.IsNullOrEmpty (confirmPassword)) {
			notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("SignUp.FILL_ALL"));
		} else if (newPassword != confirmPassword) {
			notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("SignUp.ERROR_PASSWORD_MATCH"));
		} else {
			connectingPanel.Connecting (true);
			DBManager.API.UserResetPassword (username, otp, newPassword, confirmPassword,
				(response) => {
//					Debug.Log ("password reset success");
					connectingPanel.Connecting (false);
					notificationPopUp.OnFinishOutro += AfterReset;
					notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("ForgotPassword.CHANGE_SUCCESS"));
				},
				(error) => {
					connectingPanel.Connecting (false);
					notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("ForgotPassword.FAILED_PASSWORD"));
//					JSONNode jsonData = JSON.Parse (error);
//					Debug.Log (jsonData ["confirmPassword"]);
//					Debug.Log (jsonData ["verificationOtp"]);
				}
			);
		}
	}

	void AfterReset() {
		notificationPopUp.OnFinishOutro -= AfterReset;
		CloseAndGoToNextPage (panelSignIn);
	}
}
