﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

enum MessageType{
	RegisterSuccess,
	RegisterError,
	VerifySuccess,
	VerifyError
}

public class SignUpManager : MonoBehaviour {
	public GameObject panelSignIn;
	public GameObject panelPopupMsg;
	public GameObject panelPopupVerify;

	public Text popupMsg;

	public Image[] genderRadioButtons = new Image[2];
	public Image checkBoxNewsletter;
	public Image checkBoxTnC;

	public Sprite[] radioButtonOnOff = new Sprite[2]; //0 = off, 1 = on
	public Sprite[] checkBoxOnOff = new Sprite[2]; //0 = off, 1 = on

	string signUpUsername;
	string signUpEmail;
	string signUpPhone;
	string signUpPassword;
	string signUpConfirmPassword;
	string signUpOTP;

	MessageType currRegistrationStatus = MessageType.RegisterError;

	int genderOption = 0; // 1 = male, 2 = female

	bool tickNewsletter = false;
	bool tickTnC = false;

	#region InputFields

	public void GetInputUsername(InputField obj){
		signUpUsername = obj.text;
	}

	public void GetInputEmail(InputField obj){
		signUpEmail = obj.text;
	}

	public void GetInputPhone(InputField obj){
		signUpPhone = obj.text;
	}

	public void GetInputPassword(InputField obj){
		signUpPassword = obj.text;
	}

	public void GetInputConfirmPassword(InputField obj){
		signUpConfirmPassword = obj.text;
	}

	public void GetInputGenderOption(int opt){
		genderOption = opt;
		UpdateGenderOptionDisplay();
	}

	public void GetInputNewsletter ()
	{
		tickNewsletter = !tickNewsletter;
		UpdateNewsletterDisplay();
	}

	public void GetInputTnC(){
		tickTnC = !tickTnC;
		UpdateTnCDisplay(); 
	}

	public void GetInputOTP(InputField obj){
		signUpOTP = obj.text;
	}

	#endregion

	public void OnClickContinue (){
		CheckInputContents();
	}

	void UpdateGenderOptionDisplay ()
	{
		if (genderOption == 1) {
			genderRadioButtons[0].sprite = radioButtonOnOff[1];
			genderRadioButtons[1].sprite = radioButtonOnOff[0];
		} else {
			genderRadioButtons[0].sprite = radioButtonOnOff[0];
			genderRadioButtons[1].sprite = radioButtonOnOff[1];
		}
	}

	void UpdateNewsletterDisplay ()
	{
		if (tickNewsletter) {
			checkBoxNewsletter.sprite = checkBoxOnOff [1];
		} else {
			checkBoxNewsletter.sprite = checkBoxOnOff[0];
		}
	}

	void UpdateTnCDisplay ()
	{
		if (tickTnC) {
			checkBoxTnC.sprite = checkBoxOnOff [1];
		} else {
			checkBoxTnC.sprite = checkBoxOnOff[0];
		}
	}

	void CheckInputContents () //missing check username/email/phonenumber
	{
		if (string.IsNullOrEmpty (signUpUsername) || string.IsNullOrEmpty (signUpEmail) || string.IsNullOrEmpty (signUpPhone) ||
		    string.IsNullOrEmpty (signUpPassword) || string.IsNullOrEmpty (signUpConfirmPassword) || genderOption == 0) {
			DisplayMessagePopup ("Please fill in all fields");
		} else if (signUpPassword != signUpConfirmPassword) {
			DisplayMessagePopup ("Passwords do not match");
		} else if (!tickTnC) {
			DisplayMessagePopup ("Please agree to the terms and conditions");
		} else {
			DoRegister();
		}
	}

	void DisplayMessagePopup (string msgText)
	{
		popupMsg.text=msgText;
		panelPopupMsg.SetActive(true);
	}

	public void OnClickClosePopup ()
	{
		panelPopupMsg.SetActive (false);
		if (currRegistrationStatus == MessageType.RegisterSuccess) {
			panelPopupVerify.SetActive (true);
		} else if (currRegistrationStatus == MessageType.VerifySuccess) {
			panelSignIn.SetActive(true);
			this.gameObject.SetActive(false);
		}
	}

	public void OnClickSendOTP(){
		DBManager.API.VerifyUser(signUpUsername,signUpOTP,
			(response)=>{
				currRegistrationStatus = MessageType.VerifySuccess;
				panelPopupVerify.SetActive(false);
				DisplayMessagePopup("Verification success!");
			},
			(error)=>{
				JSONNode jsonData = JSON.Parse(error);
				DisplayMessagePopup(jsonData["errors"]["username"]);
				currRegistrationStatus = MessageType.VerifyError;
			}
		);
	}

	void DoRegister(){
		DBManager.API.UserRegistration(tickTnC,genderOption,signUpUsername,signUpPassword,signUpPhone,signUpEmail,
			(response)=>{
				DisplayMessagePopup("Registration success!");
				currRegistrationStatus = MessageType.RegisterSuccess;
			},
			(error)=>{
				JSONNode jsonData = JSON.Parse(error);
				DisplayMessagePopup(jsonData["errors"]["username"]);
				currRegistrationStatus = MessageType.RegisterError;
			}
		);	
	}
}
