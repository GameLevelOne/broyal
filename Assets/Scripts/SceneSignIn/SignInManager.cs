﻿using System.Collections;
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
	public PopupManager panelPopupMsg;

	string sceneLandingPage = "SceneHome";
	string signInUsername;
	string signInPassword;

	PanelsFromSignIn nextPanel;

	void Awake() {
		//fader.FadeIn ();
        SoundManager.Instance.PlayBGM(BGMList.BGMMenu01);
	}

	void OnEnable(){
		fader.OnFadeOutFinished += OnFadeOutFinished;
	}

	void OnDisable(){
		fader.OnFadeOutFinished -= OnFadeOutFinished;
	}

	void OnFadeOutFinished ()
	{
		if (nextPanel == PanelsFromSignIn.Game) {
			panelLoading.SetActive (true);
			panelLoading.GetComponent<LoadingProgress>().ChangeScene(sceneLandingPage);
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
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		CheckInputContents();
	}

	public void OnClickFBLogin (){
		FBManager.Instance.OnFBLogin();
        SoundManager.Instance.PlaySFX(SFXList.Button01);
    }

	public void OnClickForgotPassword(){
		nextPanel = PanelsFromSignIn.Password;
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        fader.FadeOut();
	}

	public void OnClickSignUp(){
		nextPanel = PanelsFromSignIn.SignUp;
        SoundManager.Instance.PlaySFX(SFXList.Button01);
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
		panelPopupMsg.gameObject.SetActive(true);
		panelPopupMsg.SetText(msgText);
		panelPopupMsg.OpenPanel();
	}

	void DoLogin(){
		DBManager.API.UserLogin(signInUsername,signInPassword,
			(response)=>{
				DBManager.API.username = signInUsername;
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
