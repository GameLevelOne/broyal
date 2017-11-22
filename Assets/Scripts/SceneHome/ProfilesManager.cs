using System.Collections;
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
	public Text editAddressField;

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
//					for (int i=0;i<jsonData["playedGames"].Count;i++) {
//						highScoreLabel[i].text = jsonData["highScore"];
//						scoreDateLabel[i].text = Utilities.StringLongToDateTime(jsonData["scoreDate"]);
//					}

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
					256,778,
//					jsonData["equippedPet"]["petExp"].AsInt,
//					jsonData["equippedPet"]["petNextRankExp"].AsInt,
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
				notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("General.SERVER_ERROR"));
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
			|| (provinceDrop.captionText.text=="") || (cityDrop.captionText.text=="")) {
			notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("SignUp.FILL_ALL"));
		} else {
			connectingPanel.Connecting (true);
			DBManager.API.UpdateUserProfile ((genderButton.sprite==genderIcons[0]) ? 0 : 1,
				editPhoneField.text,
				editEmailField.text,
				editFullNameField.text,
				provinceDrop.captionText.text,
				"123",
				cityDrop.captionText.text,
				"456",
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


//
//	public void OnClickUserProfile (){
//		panelUserProfile.SetActive(true);
//		panelPetProfile.SetActive(false);
//	}
//
//	public void OnClickPetProfile(){
//		panelUserProfile.SetActive(false);
//		panelPetProfile.SetActive(true);
//	}
//
//	public void OnClickBack(){
////		navigationBar.CloseCurrentActivePanel();
////		navigationBar.BackToHome();
//		panelLandingPage.SetActive(true);
//		this.gameObject.SetActive(false);
//	}
//
//	public void OnClickEditProfile(){
//		ChangeEditProfileDisplay();
//		panelEditProfile.SetActive(true);
//	}
//
//	public void OnClickCancelEditProfile(){
//		panelEditProfile.SetActive(false);
//	}
//
//	public void OnClickEditUsername(){
//		panelEditUsername.SetActive(true);
//	}
//
//	public void OnClickCancelEditUsername(){
//		panelEditUsername.SetActive(false);
//	}
//
//	public void OnClickEditPassword(){
//		panelEditPassword.SetActive(true);
//	}
//
//	public void OnClickCancelEditPassword(){
//		panelEditPassword.SetActive(false);
//	}
//
//	public void OnClickEditPetName(){
//		panelEditPetName.SetActive(true);
//	}
//
//	public void OnClickCancelEditPetName(){
//		panelEditPetName.SetActive(false);
//	}
//
//	public void OnClickChangeGender (Image obj)
//	{
//		if (obj.sprite == genderIcons [0]) {
//			obj.sprite = genderIcons [1];
//			gender = 1;
//		} else {
//			obj.sprite = genderIcons[0];
//			gender = 0;
//		}
//		Debug.Log("currentGender:"+gender);
//	}
//
//	public void OnClickSubmitEditProfile(){
//		DoEditUserProfile();
//	}
//
//	public void OnClickSubmitChangeUsername (){
//		DoChangeUsername();
//	}
//
//	public void OnClickSubmitChangePassword(){
//		DoChangePassword();
//	}
//
//	public void GetUsername (InputField obj){
//		username = obj.text;
//	}
//
//	public void GetCurrentPassword (InputField obj){
//		currPassword = obj.text;
//	}
//
//	public void GetNewPassword1 (InputField obj){
//		newPassword1 = obj.text;
//	}
//
//	public void GetNewPassword2(InputField obj){
//		newPassword2 = obj.text;
//	}
//
//	public void GetInputEmail(InputField obj){
//		email = obj.text;
//	}
//
//	public void GetInputPhone (InputField obj){
//		phoneNum = obj.text;
//	}
//
//	public void GetFullName (InputField obj){
//		fullName = obj.text;
//	}
//
//	public void GetAddress (InputField obj){
//		address = obj.text;
//	}
//
//	public void GetProvince (Dropdown obj)
//	{
//		if (obj.value == -1) {
//			province = null;
//		} else {
//			province = obj.options [obj.value].text;
//		}
//	}
//
//	public void GetCity (Dropdown obj)
//	{
//		if (obj.value == -1) {
//			city = null;
//		} else {
//			city = obj.options [obj.value].text;
//		}
//	}
//
//	public void GetPetName (InputField obj){
//		petName = obj.text;
//	}
//
//	void DoEditUserProfile ()
//	{
//		if (string.IsNullOrEmpty (email) || string.IsNullOrEmpty (phoneNum) || string.IsNullOrEmpty (address) ||
//		   string.IsNullOrEmpty (province) || string.IsNullOrEmpty (city)) {
//		   	Debug.Log("please fill all fields");
//		} else{
//			DBManager.API.UpdateUserProfile(gender,phoneNum,email,address,province,city,
//			(response)=>{
//				Debug.Log("update user success");	
//				panelEditProfile.SetActive(false);
//			},
//			(error)=>{	
//				Debug.Log("update user fail");
//			}
//			);
//		}
//	}
//
//	void DoChangeUsername ()
//	{
//		if (string.IsNullOrEmpty (username)) {
//			Debug.Log ("please fill the field");
//		} else {
//			DBManager.API.UpdateUserName(username,
//			(response)=>{
//				Debug.Log("username changed successfully");
//				panelEditUsername.SetActive(false);
//				DBManager.API.username = username;
//				ChangeEditProfileDisplay();
//			},
//			(error)=>{
//				JSONNode jsonData = JSON.Parse(error);
//				Debug.Log("Can only change username once");
//			}
//			);
//		}
//	}
//
//	void DoChangePassword ()
//	{
//		if (string.IsNullOrEmpty (currPassword) || string.IsNullOrEmpty (newPassword1) || string.IsNullOrEmpty (newPassword2)) {
//			Debug.Log ("please fill all fields");
//		} else {
//			DBManager.API.UserChangePassword(currPassword,newPassword1,newPassword2,
//			(response)=>{
//				Debug.Log("password changed successfully");
//				panelEditPassword.SetActive(false);
//			},
//			(error)=>{
//				JSONNode jsonData = JSON.Parse(error);
//				Debug.Log(jsonData["message"]);
//			}
//			);
//		}
//	}
//
//	void ChangeEditProfileDisplay(){
////		fieldUsername.text = PlayerData.Instance.Username;
////		fieldEmail.text = PlayerData.Instance.Email;
////		fieldPhone.text = PlayerData.Instance.PhoneNum;
//	}
}
