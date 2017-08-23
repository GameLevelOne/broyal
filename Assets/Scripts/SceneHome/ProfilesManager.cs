using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class ProfilesManager : MonoBehaviour {
	public NavigationBarManager navigationBar;

	public Sprite[] genderIcons = new Sprite[2]; //0=male, 1=female

	public GameObject panelAuctionLobby;
	public GameObject panelUserProfile;
	public GameObject panelPetProfile;
	public GameObject panelLandingPage;
	public GameObject panelEditProfile;
	public GameObject panelEditUsername;
	public GameObject panelEditPassword;
	public GameObject panelEditPetName;
	public GameObject iconGender;

	public Text fieldUsername;
	public InputField fieldEmail;
	public InputField fieldPhone;
	public InputField fieldAddress;

	string username;
	string currPassword;
	string newPassword1;
	string newPassword2;
	string email;
	string phoneNum;
	string fullName;
	string province;
	string city;
	string address;
	string petName;
	int gender;

	public void OnClickUserProfile (){
		panelUserProfile.SetActive(true);
		panelPetProfile.SetActive(false);
	}

	public void OnClickPetProfile(){
		panelUserProfile.SetActive(false);
		panelPetProfile.SetActive(true);
	}

	public void OnClickBack(){
		navigationBar.CloseCurrentActivePanel();
		navigationBar.BackToHome();
		panelLandingPage.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void OnClickEditProfile(){
		ChangeEditProfileDisplay();
		panelEditProfile.SetActive(true);
	}

	public void OnClickCancelEditProfile(){
		panelEditProfile.SetActive(false);
	}

	public void OnClickEditUsername(){
		panelEditUsername.SetActive(true);
	}

	public void OnClickCancelEditUsername(){
		panelEditUsername.SetActive(false);
	}

	public void OnClickEditPassword(){
		panelEditPassword.SetActive(true);
	}

	public void OnClickCancelEditPassword(){
		panelEditPassword.SetActive(false);
	}

	public void OnClickEditPetName(){
		panelEditPetName.SetActive(true);
	}

	public void OnClickCancelEditPetName(){
		panelEditPetName.SetActive(false);
	}

	public void OnClickChangeGender (Image obj)
	{
		if (obj.sprite == genderIcons [0]) {
			obj.sprite = genderIcons [1];
			gender = 1;
		} else {
			obj.sprite = genderIcons[0];
			gender = 0;
		}
		Debug.Log("currentGender:"+gender);
	}

	public void OnClickSubmitEditProfile(){
		DoEditUserProfile();
	}

	public void OnClickSubmitChangeUsername (){
		DoChangeUsername();
	}

	public void OnClickSubmitChangePassword(){
		DoChangePassword();
	}

	public void GetUsername (InputField obj){
		username = obj.text;
	}

	public void GetCurrentPassword (InputField obj){
		currPassword = obj.text;
	}

	public void GetNewPassword1 (InputField obj){
		newPassword1 = obj.text;
	}

	public void GetNewPassword2(InputField obj){
		newPassword2 = obj.text;
	}

	public void GetInputEmail(InputField obj){
		email = obj.text;
	}

	public void GetInputPhone (InputField obj){
		phoneNum = obj.text;
	}

	public void GetFullName (InputField obj){
		fullName = obj.text;
	}

	public void GetAddress (InputField obj){
		address = obj.text;
	}

	public void GetProvince (Dropdown obj)
	{
		if (obj.value == -1) {
			province = null;
		} else {
			province = obj.options [obj.value].text;
		}
	}

	public void GetCity (Dropdown obj)
	{
		if (obj.value == -1) {
			city = null;
		} else {
			city = obj.options [obj.value].text;
		}
	}

	public void GetPetName (InputField obj){
		petName = obj.text;
	}

	void DoEditUserProfile ()
	{
		if (string.IsNullOrEmpty (email) || string.IsNullOrEmpty (phoneNum) || string.IsNullOrEmpty (address) ||
		   string.IsNullOrEmpty (province) || string.IsNullOrEmpty (city)) {
		   	Debug.Log("please fill all fields");
		} else{
			DBManager.API.UpdateUserProfile(gender,phoneNum,email,address,province,city,
			(response)=>{
				Debug.Log("update user success");	
				panelEditProfile.SetActive(false);
			},
			(error)=>{	
				Debug.Log("update user fail");
			}
			);
		}
	}

	void DoChangeUsername ()
	{
		if (string.IsNullOrEmpty (username)) {
			Debug.Log ("please fill the field");
		} else {
			DBManager.API.UpdateUserName(username,
			(response)=>{
				Debug.Log("username changed successfully");
				panelEditUsername.SetActive(false);
				PlayerData.Instance.Username = username;
				ChangeEditProfileDisplay();
			},
			(error)=>{
				JSONNode jsonData = JSON.Parse(error);
				Debug.Log("Can only change username once");
			}
			);
		}
	}

	void DoChangePassword ()
	{
		if (string.IsNullOrEmpty (currPassword) || string.IsNullOrEmpty (newPassword1) || string.IsNullOrEmpty (newPassword2)) {
			Debug.Log ("please fill all fields");
		} else {
			DBManager.API.UserChangePassword(currPassword,newPassword1,newPassword2,
			(response)=>{
				Debug.Log("password changed successfully");
			},
			(error)=>{
				JSONNode jsonData = JSON.Parse(error);
				Debug.Log(jsonData["message"]);
			}
			);
		}
	}

	void ChangeEditProfileDisplay(){
		fieldUsername.text = PlayerData.Instance.Username;
		fieldEmail.text = PlayerData.Instance.Email;
		fieldPhone.text = PlayerData.Instance.PhoneNum;
	}
}
