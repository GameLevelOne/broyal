using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using SimpleJSON;

public enum PanelsFromSignIn{
	Game,SignUp,Password
}

public class SignInManager : MonoBehaviour {
	public Fader fader;
	public GameObject panelLoading;
	public GameObject panelForgotPassword;
	public GameObject panelForgotPassword2;
	public GameObject panelSignUp;
	public GameObject panelPopupMsg;

	public Text popupMsg;

	string sceneLandingPage = "SceneHome";
	string signInUsername;
	string signInPassword;

	PanelsFromSignIn nextPanel;

	void OnEnable(){
		Fader.OnFadeOutFinished += OnFadeOutFinished;
	}

	void OnDisable(){
		Fader.OnFadeOutFinished -= OnFadeOutFinished;
	}

	void OnFadeOutFinished ()
	{
		Debug.Log("asd");
		if (nextPanel == PanelsFromSignIn.Game) {
			panelLoading.SetActive (true);
			LoadingProgress.Instance.ChangeScene (sceneLandingPage);
			this.gameObject.SetActive(false);
		} else if(nextPanel == PanelsFromSignIn.Password){
			this.gameObject.SetActive(false);
			panelForgotPassword2.SetActive(false);
			panelForgotPassword.SetActive(true);
			fader.FadeIn();
		} else if(nextPanel == PanelsFromSignIn.SignUp){
			this.gameObject.SetActive(false);
			panelForgotPassword2.SetActive(false);
			panelSignUp.SetActive(true);
			fader.FadeIn();
		}
	}

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
		nextPanel = PanelsFromSignIn.Password;
		fader.FadeOut();
	}

	public void OnClickSignUp(){
		nextPanel = PanelsFromSignIn.SignUp;
		fader.FadeOut();
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
				nextPanel = PanelsFromSignIn.Game;
				fader.FadeOut();
			},
			(error)=>{
				//JSONNode jsonData = JSON.Parse(error);
				//DisplayMessage(jsonData["errors"]["username"]);
				DisplayMessage("fail to login");
			}
		);
	}
}
