using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using BidRoyale.Core;

public class HeaderAreaManager : MonoBehaviour {
	public ConnectingPanel connectingPanel;
	public NotificationPopUp notificationPopUp;
	public GameObject headerPetLoading;
	public GameObject headerWithPet;
	public GameObject headerNoPet;

	public Text usernameLabel;
	public Text userStarsLabel;
	public RectTransform starsHLG;
	public ImageLoader petImage;
	public Text petName;
	public Text petRank;
	public Text petExp;
	public Button petTrainButton;
	public Text petTrainLabel;
	int trainCountDown;
	int curStars;

	public PetData petData;

	public int userStars {
		get {
			return curStars;
		}
	}

	void OnEnable(){
		usernameLabel.text = DBManager.API.username;
		GetUserStars();
		GetPetProfile();
	}

	public void SetStars(int amount) {
		curStars = amount;
		if (amount < 1) {
			userStarsLabel.text = "-";
		} else {
			userStarsLabel.text = userStars.ToString ("N0");
		}
	}

	public void GetUserStars() {
		SetStars(-1);
		DBManager.API.GetUserStars(
			(response)=>{
				JSONNode jsonData = JSON.Parse(response);
				SetStars(jsonData["availableStars"].AsInt);
				LayoutRebuilder.ForceRebuildLayoutImmediate(starsHLG);
			},
			(error)=>{
				UserStarsError();
			}
		);
	}

	public void AnimateUserStars(int nextStars) {
		StopAllCoroutines ();
		StartCoroutine (CountToNextStars(nextStars));
	}

	IEnumerator CountToNextStars(int nextStars) {
		int deltaStars = (nextStars - curStars) / 20;
		while ((curStars != nextStars) && (deltaStars != 0)) {
			curStars += deltaStars;
			if (deltaStars > 0) {
				if (curStars > nextStars)
					curStars = nextStars;
			} else {
				if (curStars < nextStars)
					curStars = nextStars;
			}
			SetStars (curStars);
			yield return new WaitForSeconds (0.05f);
		}
		yield return null;
		curStars = nextStars;
	}

	void UserStarsError() {
		GetUserStars ();
	}

	public void GetPetProfile() {
		headerWithPet.SetActive (false);
		headerNoPet.SetActive (false);
		headerPetLoading.SetActive (true);
		DBManager.API.GetUserPetProfile(
			(response)=>{
				JSONNode jsonData = JSON.Parse(response);
				string msg = jsonData["message"];
				petData = new PetData();

				petData.InitHeader(jsonData["equippedPet"]["petName"],
					jsonData["equippedPet"]["petModelImage"],
					jsonData["equippedPet"]["petRank"],
					jsonData["equippedPet"]["petExp"].AsInt,
					jsonData["equippedPet"]["petNextRankExp"].AsInt
				);
				headerWithPet.SetActive (true);
				headerNoPet.SetActive (false);
				headerPetLoading.SetActive (false);
				UpdatePetData();
				petTrainButton.interactable = false;
				petTrainLabel.text = LocalizationService.Instance.GetTextByKey("Header.TRAIN");
				GetTrainingTime();
			},
			(error)=>{
				petData = null;
				headerWithPet.SetActive (false);
				headerNoPet.SetActive (true);
				headerPetLoading.SetActive (false);			
			}
		);
	}		

	void GetTrainingTime() {
		DBManager.API.CheckTrainingTime (
			(response) => {
				JSONNode jsonData = JSON.Parse(response);
				trainCountDown = jsonData["remainingSeconds"];
				CheckTrainCountDown();
			}, 
			(error) => {
//				Debug.Log("-------Error get:"+ error.Split('|')[0].Trim() +"muahahahah");
				JSONNode jsonData = JSON.Parse (error);
                if ((jsonData != null) && (jsonData["errors"] == "NO_TRAINING_PET"))
                {
					petTrainButton.interactable = true;
					petTrainLabel.text = LocalizationService.Instance.GetTextByKey("Header.TRAIN");
				} else if ((jsonData!=null) && (jsonData["errors"]=="TRAINING_FINISH") ) {
					petTrainButton.interactable = true;
					petTrainLabel.text = LocalizationService.Instance.GetTextByKey("Header.CLAIM");
				}
			}
		);
	}

	IEnumerator TrainCountDown()
	{
		while (trainCountDown > 0) {
			petTrainLabel.text = Utilities.SecondsToMinutes(trainCountDown);
			yield return new WaitForSeconds (1);
		}
		petTrainButton.interactable = true;
		petTrainLabel.text = LocalizationService.Instance.GetTextByKey("Header.CLAIM");
	}

	public void ProfileClicked(int profileType) {
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		ProfilesManager futurePage = (ProfilesManager)PagesManager.instance.GetPagesByName("PROFILES");
		futurePage.initProfileType = profileType;;
		futurePage.prevPage = PagesManager.instance.GetCurrentPage ();
		PagesManager.instance.CurrentPageOutro (futurePage);
			
    }

	public void SettingsClicked() {
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        SettingsManager futurePage = (SettingsManager)PagesManager.instance.GetPagesByName("SETTINGS");
		futurePage.prevPage = PagesManager.instance.GetCurrentPage ();
		PagesManager.instance.CurrentPageOutro (futurePage);
	}

	public void TrainClicked() {
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		connectingPanel.Connecting (true);
		if (petTrainLabel.text == LocalizationService.Instance.GetTextByKey ("Header.TRAIN")) {
			DBManager.API.StartTrainingTime (
				(response) => {
					petTrainButton.interactable = false;
					connectingPanel.Connecting (false);
					JSONNode jsonData = JSON.Parse(response);
					trainCountDown = jsonData["remainingSeconds"];
					CheckTrainCountDown();
				}, 
				(error) => {
					connectingPanel.Connecting (false);
					JSONNode jsonData = JSON.Parse (error);
					if (jsonData!=null) {
						notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
					} else {
						notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
					}
				}
			);

		} else {
			connectingPanel.Connecting (true);
			DBManager.API.ClaimPetTrainExp (
				(response) => {
					connectingPanel.Connecting (false);
					GetPetProfile();
				}, 
				(error) => {
					connectingPanel.Connecting (false);
					JSONNode jsonData = JSON.Parse (error);
					if (jsonData!=null) {
						notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
					} else {
						notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
					}
				}
			);
		}
	}

	void CheckTrainCountDown() {
		if (trainCountDown>0) {
			petTrainButton.interactable = false;
			petTrainLabel.text = Utilities.SecondsToMinutes(trainCountDown);
			StartCoroutine(TrainCountDown());
		} 
		else if (trainCountDown<0) {
			petTrainButton.interactable = true;
			petTrainLabel.text = LocalizationService.Instance.GetTextByKey("Header.CLAIM");
		} else {
			petTrainButton.interactable = true;
			petTrainLabel.text = LocalizationService.Instance.GetTextByKey("Header.TRAIN");
		}
	}

	public void GetPetClicked() {
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        BasePage futurePage = PagesManager.instance.GetPagesByName("SHOP");
		PagesManager.instance.CurrentPageOutro (futurePage);
	}

	void UpdatePetData()
	{
		petImage.LoadImageFromUrl (petData.imageUrl);
		petName.text = petData.name;
		petRank.text = LocalizationService.Instance.GetTextByKey ("Header.PET_RANK") + petData.rank;
		petExp.text = "EXP. " + petData.exp.ToString ("N0") + " / " + petData.nextRankExp.ToString ("N0");
	}
}
