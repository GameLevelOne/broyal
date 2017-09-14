using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPreviousSession : MonoBehaviour {
	public Fader fader;
	public LoadingProgress panelLoading;

	string sceneHome = "SceneHome";

	bool hasLoggedIn = false;

	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteAll();
		CheckPreviousSignIn();
	}

	void OnEnable(){
		fader.OnFadeInFinished += HandleOnFadeInFinished;
	}

	void OnDisable(){
		fader.OnFadeInFinished -= HandleOnFadeInFinished;
	}

	void HandleOnFadeInFinished ()
	{
		if(hasLoggedIn){
			panelLoading.ChangeScene(sceneHome);
		}
	}

	void CheckPreviousSignIn(){
		string prevUsername = PlayerPrefs.GetString("PlayerData/Username","");
		string prevPassword = PlayerPrefs.GetString("PlayerData/Password","");

		if(string.IsNullOrEmpty(prevUsername) && string.IsNullOrEmpty(prevPassword)){
			hasLoggedIn = false;
			panelLoading.gameObject.SetActive(false);
			fader.FadeIn();
		}else{
			DoLogin(prevUsername,prevPassword);
		}
	} 

	void DoLogin(string signInUsername,string signInPassword){
		DBManager.API.UserLogin(signInUsername,signInPassword,
			(response)=>{
				PlayerData.Instance.Username = signInUsername;
				PlayerData.Instance.Password = signInPassword;
				LoginResult(true);
			},
			(error)=>{
				//JSONNode jsonData = JSON.Parse(error);
				LoginResult(false);
			}
		);
	}

	void LoginResult(bool loggedIn){
		hasLoggedIn = loggedIn;
		if(hasLoggedIn)
			panelLoading.gameObject.SetActive(true);
		fader.FadeIn();
	}
}
