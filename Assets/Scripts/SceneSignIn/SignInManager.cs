using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using SimpleJSON;

public class SignInManager : MonoBehaviour {
	public GameObject panelLoading;
	public GameObject panelForgotPassword;
	public GameObject panelSignUp;
	public GameObject panelPopupMsg;

	public Text popupMsg;

	string sceneLandingPage = "SceneHome";
	string signInUsername;
	string signInPassword;

	public void GetInputUsername (InputField obj){
		signInUsername = obj.text;
	}

	public void GetInputPassword (InputField obj){
		signInPassword = obj.text;
	}

	public void OnClickSignIn(){
		//go to loading scene
		CheckInputContents();
	}

	public void OnClickFBLogin (){
		FBManager.Instance.OnFBLogin();
	}

	public void OnClickForgotPassword(){
		this.gameObject.SetActive(false);
		panelForgotPassword.SetActive(true);
	}

	public void OnClickSignUp(){
		this.gameObject.SetActive(false);
		panelSignUp.SetActive(true);
	}

	void CheckInputContents ()
	{
		if (string.IsNullOrEmpty (signInUsername) || string.IsNullOrEmpty (signInPassword)) {
			DisplayMessage ("Please fill in all fields");
		} else {
			DoLogin();
		}
	}

	void DisplayMessage (string msgText){
		popupMsg.text = msgText;
		panelPopupMsg.SetActive(true);
	}

	void DoLogin(){
		DBManager.API.UserLogin(signInUsername,signInPassword,
			(response)=>{
				this.gameObject.SetActive(false);
				panelLoading.SetActive(true);
				LoadingProgress.Instance.ChangeScene(sceneLandingPage);
			},
			(error)=>{
				//JSONNode jsonData = JSON.Parse(error);
				//DisplayMessage(jsonData["errors"]["username"]);
				DisplayMessage("fail to login");
			}
		);
	}

	public void OnClickClosePopup(){
		panelPopupMsg.SetActive(false);
	}
}
