using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

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
		DoRequestForgotPassword();
		//panelForgotPass1.SetActive(false);
		//panelForgotPass2.SetActive(true);
	}

	public void OnClickEnter(){ //forgot 2 -> goto sign in page
		DoResetPassword();
	}

	void DoRequestForgotPassword (){
		if(!string.IsNullOrEmpty(username)){
			DBManager.API.UserForgotPassword(username,
			(response)=>{
				JSONNode jsonData = JSON.Parse(response);
				Debug.Log(jsonData["message"]);
				panelForgotPass1.SetActive(false);
				panelForgotPass2.SetActive(true);
			},
			(error)=>{
				Debug.Log("ERROR");
			}
			);
		} else{
			Debug.Log("field is empty");
		}
	}

	void DoResetPassword ()
	{
		if (string.IsNullOrEmpty (otp) || string.IsNullOrEmpty (newPass1) || string.IsNullOrEmpty (newPass2)) {
			Debug.Log ("please fill in all fields");
		} else if (newPass1 != newPass2) {
			Debug.Log ("passwords not match");
		} else {
			DBManager.API.UserResetPassword (username, otp, newPass1, newPass2,
				(response) => {
					Debug.Log ("password reset success");
					panelForgotPass2.SetActive(false);
					panelSignIn.SetActive(true);
				},
				(error) => {
					JSONNode jsonData = JSON.Parse (error);
					Debug.Log (jsonData ["confirmPassword"]);
					Debug.Log (jsonData ["verificationOtp"]);
				}
			);
		}
	}
}
