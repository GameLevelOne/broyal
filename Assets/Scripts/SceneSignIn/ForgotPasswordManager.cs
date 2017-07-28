using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgotPasswordManager : MonoBehaviour {
	public GameObject panelForgotPass1;
	public GameObject panelForgotPass2;
	public GameObject panelSignIn;

	string username;
	string otp;
	string newPass1;
	string newPass2;

	public void GetInputUsername(InputField obj){
		username = obj.text;
	}

	public void GetInputOTP (InputField obj){
		otp = obj.text;
	}

	public void GetInputNewPass1 (InputField obj){ //new password
		newPass1 = obj.text;
	}

	public void GetInputNewPass2(InputField obj){ //confirm password
		newPass2 = obj.text;
	}

	public void OnClickSend(){ //forgot 1 -> goto OTP page
		panelForgotPass1.SetActive(false);
		panelForgotPass2.SetActive(true);
	}

	public void OnClickEnter(){ //forgot 2 -> goto sign in page
		panelForgotPass2.SetActive(false);
		panelSignIn.SetActive(true);
	}
}
