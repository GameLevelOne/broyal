using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using BidRoyale.Core;

public class HeaderAreaManager : MonoBehaviour {
	public GameObject headerPetLoading;
	public GameObject headerWithPet;
	public GameObject headerNoPet;

	public Text usernameLabel;
	public Text userStarsLabel;
	public RectTransform starsHLG;
	public int userStars;
	public Text petExp;
	public Text petName;
	public Text petRank;
	public Button petTrainButton;
	public Text petTrainLabel;
	int trainCountDown;

	public PetData petData;

	void OnEnable(){
		usernameLabel.text = DBManager.API.username;
		GetUserStars();
		GetPetProfile();
	}
		
	void GetUserStars() {
		userStarsLabel.text = "-";
		DBManager.API.GetUserStars(
			(response)=>{
				JSONNode jsonData = JSON.Parse(response);
				userStars = jsonData["availableStars"].AsInt;
				userStarsLabel.text = userStars.ToString ("N0");
				LayoutRebuilder.ForceRebuildLayoutImmediate(starsHLG);
			},
			(error)=>{
				UserStarsError();
			}
		);
	}

	void UserStarsError() {
		GetUserStars ();
	}

	void GetPetProfile() {
		headerWithPet.SetActive (false);
		headerNoPet.SetActive (false);
		headerPetLoading.SetActive (true);
		DBManager.API.GetUserPetProfile(
			(response)=>{
				JSONNode jsonData = JSON.Parse(response);
				string msg = jsonData["message"];
				if(!string.IsNullOrEmpty(msg)){
					petData = null;
					headerWithPet.SetActive (false);
					headerNoPet.SetActive (true);
					headerPetLoading.SetActive (false);
				} else{
					petData = new PetData();

					petData.InitHeader(jsonData["petName"],
						jsonData["petModelImage"],
						jsonData["petRank"],
						jsonData["petExp"].AsInt,
						jsonData["petNextRankExp"].AsInt
					);

					headerWithPet.SetActive (true);
					headerNoPet.SetActive (false);
					headerPetLoading.SetActive (false);
					petTrainButton.enabled = false;
					petTrainLabel.text = LocalizationService.Instance.GetTextByKey("Header.TRAIN");
					GetTrainingTime();
				}

			},
			(error)=>{
				PetProfileError();
			}
		);
	}		

	void PetProfileError() {
		GetPetProfile ();
	}

	void GetTrainingTime() {
		DBManager.API.CheckTrainingTime (
			(response) => {
				JSONNode jsonData = JSON.Parse(response);
				trainCountDown = jsonData["remainingSeconds"];
				if (trainCountDown>0) {
					petTrainButton.enabled = false;
					petTrainLabel.text = Utilities.SecondsToMinutes(trainCountDown);
					StartCoroutine(TrainCountDown());
				} else if (trainCountDown<0) {
					petTrainButton.enabled = true;
					petTrainLabel.text = LocalizationService.Instance.GetTextByKey("Header.CLAIM");
				} else {
					petTrainButton.enabled = true;
					petTrainLabel.text = LocalizationService.Instance.GetTextByKey("Header.TRAIN");
				}
			}, 
			(error) => {
			}
		);
	}

	IEnumerator TrainCountDown()
	{
		while (trainCountDown > 0) {
			petTrainLabel.text = Utilities.SecondsToMinutes(trainCountDown);
			yield return new WaitForSeconds (1);
		}
		petTrainButton.enabled = true;
		petTrainLabel.text = LocalizationService.Instance.GetTextByKey("Header.CLAIM");
	}

	public void ProfileClicked(int profileType) {
	}

	public void SettingsClicked() {
		SettingsManager futurePage = (SettingsManager) PagesManager.instance.GetPagesByName("SETTINGS");
		futurePage.prevPage = PagesManager.instance.GetCurrentPage ();
		PagesManager.instance.CurrentPageOutro (futurePage);
	}

	public void TrainClicked() {
		petTrainButton.enabled = false;
		if (petTrainLabel.text == LocalizationService.Instance.GetTextByKey ("Header.TRAIN")) {
		} else {
		}
	}

	public void GetPetClicked() {
		BasePage futurePage = PagesManager.instance.GetPagesByName("SHOP");
		PagesManager.instance.CurrentPageOutro (futurePage);
	}
}
