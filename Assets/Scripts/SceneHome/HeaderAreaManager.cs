using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class HeaderAreaManager : MonoBehaviour {
	public NavigationBarManager navigationBar;

	public GameObject panelLandingPage;
	public GameObject panelParentProfile;
	public GameObject panelUserProfile;
	public GameObject panelPetProfile;
	public GameObject panelSettings;
	public Text usernameText;

	string username;

	void Start(){
		GetCurrentUsername();
	}

	public void OnClickUserProfile(){
		navigationBar.CloseCurrentActivePanel();
		panelParentProfile.SetActive(true);
		panelUserProfile.SetActive(true);
		panelPetProfile.SetActive(false);
	}

	public void OnClickPetProfile(){
		navigationBar.CloseCurrentActivePanel();
		panelParentProfile.SetActive(true);
		panelPetProfile.SetActive(true);
		panelUserProfile.SetActive(false);
	}

	public void OnClickSettings(){
		navigationBar.CloseCurrentActivePanel();
		panelSettings.SetActive(true);
	}

	void GetCurrentUsername(){
		DBManager.API.GetUserProfile(
		(response)=>{
			JSONNode jsonData = JSON.Parse(response);
			usernameText.text = jsonData["username"];
		},
		(error)=>{
			Debug.Log("ERROR");
		}
		);
	}
}
