﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using BidRoyale.Core;

public class ProfilesManager : BasePage {
	public ConnectingPanel connectingPanel;

	public GameObject mainProfilePanel;
	public GameObject userProfilePanel;
	public GameObject petProfilePanel;
	public GameObject editProfilePanel;

	public NotificationPopUp notifPopUp;
	public EditPetNamePopUp editPetNamePopUp;
	public PetEquipPopUp petEquipPopUp;
	public ChangeUserNamePopUp changeUserNamePopUp;
	public ChangePasswordPopUp changePasswordPopUp;

	public int initProfileType;
	bool userLoaded;
	bool petLoaded;
	public BasePage prevPage;

	[Space(10)]
	[Header("USER PROFILE AREA")]
	public ImageLoader userPicture;
	public Text userNameLabel;
	public Text auctionParticipatedLabel;
	public Text auctionWonLabel;
	public Text[] highScoreLabel;
	public Text[] scoreDateLabel;

	[Space(10)]
	[Header("PET PROFILE AREA")]
	public ImageLoader petPicture;
	public Text petNameLabel;
	public Text petRankLabel;
	public Image petExpBar;
	public Text petExpLabel;
	public Text petSkillLabel;
	public Text petsOwnedLabel;
	public PetOwned[] petOwned;
	PetData petData;

	[Space(10)]
	[Header("EDIT PROFILE AREA")]
	public Sprite[] genderIcons = new Sprite[2]; //0=male, 1=female
	public ImageLoader editUserPicture;
	public Image genderButton;
	public Text editUserNameLabel;
	public Text editEmailField;
	public Text editPhoneField;
	public Text editFullNameField;
	public Dropdown provinceDrop;
	public Dropdown cityDrop;
	public GameObject provinceLoading;
	public GameObject cityLoading;
	public Text editAddressField;
	string currentProvince;
	string currentCity;
	List<string> provinceId;
	List<string> cityId;

	protected override void Init ()
	{
		userLoaded = false;
		petLoaded = false;
		if (initProfileType == 0) {
			ShowUserProfile ();
		} else {
			ShowPetProfile ();
		}
		base.Init ();
	}		

	public void ShowUserProfile() {
		LoadUserProfile ();
	}

	public void LoadUserProfile(bool fromCancel=false) {
		if (fromCancel) 
			SoundManager.Instance.PlaySFX(SFXList.Button02);
		else 
			SoundManager.Instance.PlaySFX(SFXList.Button01);
		mainProfilePanel.SetActive (true);
		userProfilePanel.SetActive (true);
		petProfilePanel.SetActive (false);
		editProfilePanel.SetActive (false);
		for (int i=0;i<4;i++) {
			highScoreLabel[i].text = "-";
			scoreDateLabel[i].text = "-";
		}
		if (!userLoaded) {
			connectingPanel.Connecting (true);
			DBManager.API.GetUserProfile (
				(response) => {
					connectingPanel.Connecting (false);
					JSONNode jsonData = JSON.Parse(response);
					userPicture.LoadImageFromUrl(jsonData["profilePicture"]);
					userNameLabel.text = jsonData["username"];
					auctionParticipatedLabel.text = jsonData["noOfAuctionPartitipated"];
					auctionWonLabel.text = jsonData["noOfAuctionWon"];
					for (int i=0;i<jsonData["playedGames"].Count;i++) {
						int rumbleGame = jsonData ["playedGames"][i]["bidRumbleGameType"].AsInt - 1;
						highScoreLabel[rumbleGame].text = ((float)jsonData["score"].AsInt / 1000000000f).ToString("0.0000");
						scoreDateLabel[rumbleGame].text = Utilities.StringLongToDateTime(jsonData["dateCreated"]).ToString("MMM dd, yy");
					}

					editUserPicture.LoadImageFromUrl(jsonData["profilePicture"]);
					if (jsonData["gender"]=="Male")
						genderButton.sprite = genderIcons[0];
					else 
						genderButton.sprite = genderIcons[1];
					editUserNameLabel.text = jsonData["username"];
					editEmailField.text = jsonData["email"];
					editPhoneField.text = jsonData["phoneNumber"];
					editFullNameField.text = jsonData["completeName"];
					editAddressField.text = jsonData["streetAddress"];
					currentProvince = jsonData["province"];
					currentCity = jsonData["city"];
					userLoaded = true;
				}, 
				(error) => {
					connectingPanel.Connecting (false);
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("General.SERVER_ERROR"));
				}
			);
		}
	}

	public void ShowPetProfile() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		userProfilePanel.SetActive (false);
		petProfilePanel.SetActive (true);
		if (!petLoaded) {
			LoadPetData ();
		}
	}

	public void EditProfile() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		mainProfilePanel.SetActive (false);
		userProfilePanel.SetActive (false);
		petProfilePanel.SetActive (false);
		editProfilePanel.SetActive (true);

		provinceDrop.gameObject.SetActive (false);
		provinceLoading.SetActive (true);
		cityDrop.gameObject.SetActive (false);
		cityLoading.SetActive (true);
		DBManager.API.GetProvinceList (
			(response) => {
				JSONNode jsonData = JSON.Parse(response);
				provinceId = new List<string>();
				provinceDrop.ClearOptions();
				List<string> provinceList = new List<string>();
				int curVal = 0;
				for (int i=0;i<jsonData["rajaongkir"]["results"].Count;i++) {
					provinceId.Add(jsonData["rajaongkir"]["results"][i]["province_id"]);
					provinceList.Add(jsonData["rajaongkir"]["results"][i]["province"]);
					if (jsonData["rajaongkir"]["results"][i]["province"]==currentProvince) {
						curVal = i;
					}
				}
				provinceDrop.AddOptions(provinceList);
				provinceDrop.gameObject.SetActive (true);
				provinceDrop.value = curVal;
				StartCoroutine(RefreshDropValue());
				provinceLoading.SetActive (false);
				LoadCityList();
			},
			(error) => {
			}
		);
	}

	IEnumerator RefreshDropValue() {
		yield return null;
		provinceDrop.RefreshShownValue();
		cityDrop.RefreshShownValue();
	}

	public void LoadCityList() {
		cityDrop.gameObject.SetActive (false);
		cityLoading.SetActive (true);
		DBManager.API.GetCityList (provinceId[provinceDrop.value],
			(response) => {
				JSONNode jsonData = JSON.Parse(response);
				cityId = new List<string>();
				cityDrop.ClearOptions();
				List<string> cityList = new List<string>();
				int curVal = 0;
				for (int i=0;i<jsonData["rajaongkir"]["results"].Count;i++) {
					cityId.Add(jsonData["rajaongkir"]["results"][i]["city_id"]);
					cityList.Add(jsonData["rajaongkir"]["results"][i]["city_name"]);
					if (jsonData["rajaongkir"]["results"][i]["city_name"]==currentCity) {
						curVal = i;
					}
				}
				cityDrop.AddOptions(cityList);
				cityDrop.gameObject.SetActive (true);
				cityDrop.value = curVal;
				StartCoroutine(RefreshDropValue());
				cityLoading.SetActive (false);
			},
			(error) => {
			}
		);
	}


	public void CloseClick() {
		SoundManager.Instance.PlaySFX(SFXList.Button02);
		PagesManager.instance.CurrentPageOutro(prevPage);
	}

	public void LoadPetData(System.Action onComplete = null) {
		connectingPanel.Connecting (true);
		mainProfilePanel.SetActive (true);
		userProfilePanel.SetActive (false);
		petProfilePanel.SetActive (true);
		editProfilePanel.SetActive (false);
		DBManager.API.GetUserPetProfile (
			(response) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse(response);
				petData = new PetData();
				petData.InitProfile(
					jsonData["equippedPet"]["petName"],
					jsonData["equippedPet"]["petId"],
					jsonData["equippedPet"]["petModelImage"],
					"",
					jsonData["equippedPet"]["petDescription"],
					jsonData["equippedPet"]["petRank"],
					jsonData["equippedPet"]["petExp"].AsInt,
					jsonData["equippedPet"]["petNextRankExp"].AsInt,
					jsonData["equippedPet"]["petSkill"]
				);

				petPicture.LoadImageFromUrl(petData.imageUrl);
				petNameLabel.text = petData.name;
				petRankLabel.text = LocalizationService.Instance.GetTextByKey ("Header.PET_RANK") + petData.rank;
				petExpBar.fillAmount = ((float)petData.exp/(float)petData.nextRankExp);
				petExpLabel.text = petData.exp.ToString("N0") + " / " + petData.nextRankExp.ToString("N0");
				petSkillLabel.text = petData.skillDescription;
				petsOwnedLabel.text = LocalizationService.Instance.GetTextByKey ("Profile.PETS_OWNED") + " " + jsonData["allPetList"].Count + " / " + 5;
				for (int i=0;i<5;i++) {
					if (i<jsonData["allPetList"].Count) {
						PetData po = new PetData();
						po.InitHeader(
							jsonData["allPetList"][i]["petName"],
							jsonData["allPetList"][i]["petModelImage"],
							jsonData["allPetList"][i]["petRank"],
							0,0
						);
						petOwned[i].InitData(po,jsonData["allPetList"][i]["equipped"].AsBool);
					} else {
						petOwned[i].InitData(null,false);
					}
				}
				petLoaded = true;
				if (onComplete != null)
					onComplete ();
			}, 
			(error) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse (error);
				if ( (jsonData!=null) && (jsonData["errors"]=="HAVE_NO_PET")) {
					ShowUserProfile();
				} else {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
				}
			}
		);
	}

	public void EditPetNameClick() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		editPetNamePopUp.InitPrice (500);
		editPetNamePopUp.Activate (true);
	}

	public void ChangeGenderClick() {
		if (genderButton.sprite == genderIcons [0])
			genderButton.sprite = genderIcons [1];
		else
			genderButton.sprite = genderIcons [0];
	}

	public void ChangeUserNameClick() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		changeUserNamePopUp.Activate (true);
	}

	public void ChangePasswordClick() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		changePasswordPopUp.Activate (true);
	}

	public void SaveEditClick() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		if ((editEmailField.text=="") || (editPhoneField.text=="") || (editFullNameField.text=="") || (editAddressField.text=="")
			|| (provinceDrop.captionText.text=="") || (cityDrop.captionText.text=="") || (provinceLoading.activeSelf) || (cityLoading.activeSelf)) {
			notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("SignUp.FILL_ALL"));
		} else {
			connectingPanel.Connecting (true);
			DBManager.API.UpdateUserProfile ((genderButton.sprite==genderIcons[0]) ? 0 : 1,
				editPhoneField.text,
				editEmailField.text,
				editFullNameField.text,
				provinceDrop.captionText.text,
				provinceId[provinceDrop.value],
				cityDrop.captionText.text,
				cityId[cityDrop.value],
				editAddressField.text,
				(response) => {
					connectingPanel.Connecting (false);
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("Profile.SUCCESS_UPDATE"));
				}, 
				(error) => {
					connectingPanel.Connecting (false);
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("General.SERVER_ERROR"));
				}
			);

		}
	}

	public void CancelEditClick() {
		LoadUserProfile (true);
	}
}
