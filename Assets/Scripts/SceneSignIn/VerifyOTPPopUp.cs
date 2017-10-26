using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class VerifyOTPPopUp : PagesIntroOutro {

	public ConnectingPanel connectingPanel;
	public Text otpText;
	public InputField inputOTP;
	public GameObject sendAgainButton;
	public bool otpSuccess;
	string userName;

	public void ShowPopUp(string username) {
		userName = username;
		SetOTPText (true);
		otpSuccess = false;
		Activate (true);
	}
		
	public void ClickEnter() {
		connectingPanel.Connecting (true);
		DBManager.API.VerifyUser (userName, inputOTP.text,
			(response) => {
				connectingPanel.Connecting (false);
				otpSuccess = true;
				Activate (false);
			},
			(error) => {
				connectingPanel.Connecting (false);
				SetOTPText(false);
				JSONNode jsonData = JSON.Parse (error);
				string errorotp = jsonData ["errors"] ["verificationOTP"];
			}
		);
	}

	void SetOTPText(bool correct) {
		if (correct) {
			otpText.text = LocalizationService.Instance.GetTextByKey("Verify.PLEASE_ENTER");
			otpText.color = Color.white;
			sendAgainButton.SetActive (false);
		} else {
			otpText.text = LocalizationService.Instance.GetTextByKey ("Verify.WRONG_OTP");
			otpText.color = Color.red;
			sendAgainButton.SetActive (true);
		}
	}

	public void ClickSendAgain() {
		connectingPanel.Connecting (true);
		DBManager.API.GetResendOTP (userName, 
			(response) => {
				connectingPanel.Connecting (false);
				SetOTPText (true);
			},
			(error) => {
				connectingPanel.Connecting (false);
				SetOTPText(false);
				otpText.text = LocalizationService.Instance.GetTextByKey ("Verify.FAILED_OTP");
				otpSuccess = true;
				Activate (false);
			}
		);
	}

	public void ClickOutside() {
		Activate (false);
	}
}
