using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class SignInManager : MonoBehaviour {
	public GameObject panelLoading;
	public GameObject panelForgotPassword;
	public GameObject panelSignUp;

	string sceneLandingPage = "SceneLandingPage";
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
		this.gameObject.SetActive(false);
		panelLoading.SetActive(true);
		LoadingProgress.Instance.ChangeScene(sceneLandingPage);
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

}
