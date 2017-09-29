using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class HeaderAreaManager : MonoBehaviour {
	public NavigationBarManager navigationBar;

	public GameObject headerWithPet;
	public GameObject headerNoPet;

	public GameObject panelLandingPage;
	public GameObject panelParentProfile;
	public GameObject panelUserProfile;
	public GameObject panelPetProfile;
	public GameObject panelSettings;
	public GameObject panelPetLevelUpPopup;
	public Text usernameText;
	public Text usernameProfileDisplay;
	public Text userStars;
	public Text petCurrExp;
	public Text petName;

	bool hasPet = false;

	int maxExp = 1000; //temp

	void Start(){
		headerWithPet.SetActive (false);
		headerNoPet.SetActive (false);
		GetCurrentUserProfile();
		GetCurrentUserPetProfile();
	}

	void OnEnable(){
		NumberCountUpEffect.OnCountUpFinished += OnCountFinished;
	}

	void OnDisable(){
		NumberCountUpEffect.OnCountUpFinished -= OnCountFinished;
	}

	void OnCountFinished ()
	{
		CheckPetLevelUp();
	}

	void CheckPetLevelUp(){
		int currExp = PlayerData.Instance.PetExp;

		if(currExp >= 1000){
			//pet level up
			panelPetLevelUpPopup.SetActive(true);
			panelPetLevelUpPopup.transform.GetChild(1).GetComponent<Text>().text = 
			"Congratulations! "+PlayerData.Instance.PetName+ " has leveled up!";
			petCurrExp.text = "0";
		}
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
//		navigationBar.CloseCurrentActivePanel();
		panelSettings.SetActive(true);
	}

	void GetCurrentUserProfile(){
		DBManager.API.GetUserProfile(
		(response)=>{
			JSONNode jsonData = JSON.Parse(response);
			PlayerData.Instance.Email = jsonData["email"];
			PlayerData.Instance.Gender = jsonData["gender"];
			PlayerData.Instance.PhoneNum = jsonData["phoneNumber"];
			PlayerData.Instance.StarsSpent = jsonData["starsSpent"];
			PlayerData.Instance.AvailableStars = jsonData["availableStars"];
			PlayerData.Instance.ProfilePic = jsonData["profilePicture"];
			UpdateDisplay();
		},
		(error)=>{
			Debug.Log("ERROR");
		}
		);
	}

	void GetCurrentUserPetProfile(){
		DBManager.API.GetUserPetProfile(
			(response)=>{
				JSONNode jsonData = JSON.Parse(response);
				string msg = jsonData["message"];
				if(!string.IsNullOrEmpty(msg)){
					hasPet=false;
				} else{
					PlayerData.Instance.PetName = jsonData["petName"];

				}

				UpdateDisplay();
			},
			(error)=>{
				Debug.Log("ERROR");
			}
		);
	}

	void UpdateDisplay ()
	{
		usernameText.text = usernameProfileDisplay.text = PlayerData.Instance.Username;
		userStars.text = PlayerData.Instance.AvailableStars.ToString ();
		if (hasPet) {
			petCurrExp.text = PlayerData.Instance.PetExp.ToString ();
			petName.text = PlayerData.Instance.PetName;
			headerWithPet.SetActive(true);
		} else{
			headerNoPet.SetActive(true);
		}
	}
}
