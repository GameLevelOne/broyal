using System.Collections;
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
	public Fader fader;
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

	string triggerPanelOpen = "panelOpen";
	string triggerPanelClose = "panelClose";

	MessageType currRegistrationStatus = MessageType.RegisterError;

	int genderOption = 0; // 1 = male, 2 = female

	bool tickNewsletter = false;
	bool tickTnC = false;

	void OnEnable(){
		fader.OnFadeOutFinished += OnFadeOutFinished;
	}

	void OnDisable(){
		fader.OnFadeOutFinished -= OnFadeOutFinished;
	}

	void OnFadeOutFinished ()
	{
		panelSignIn.SetActive(true);
		fader.FadeIn();
		this.gameObject.SetActive(false);
	}

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
		panelPopupMsg.GetComponent<Animator>().SetTrigger(triggerPanelOpen);
	}

	public void OnClickClosePopup ()
	{
//		panelPopupMsg.SetActive (false);
		if (currRegistrationStatus == MessageType.RegisterSuccess) {
			panelPopupVerify.SetActive (true);
			panelPopupVerify.GetComponent<Animator>().SetTrigger(triggerPanelOpen);
		} else if (currRegistrationStatus == MessageType.VerifySuccess) {
			fader.FadeOut();
		}
	}

	public void OnClickSendOTP(){
		DBManager.API.VerifyUser(signUpUsername,signUpOTP,
			(response)=>{
				currRegistrationStatus = MessageType.VerifySuccess;
				panelPopupVerify.GetComponent<Animator>().SetTrigger(triggerPanelClose);
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
				currRegistrationStatus = MessageType.RegisterSuccess;
				DisplayMessagePopup("Registration success!");
			},
			(error)=>{
				JSONNode jsonData = JSON.Parse(error);
				DisplayMessagePopup(jsonData["errors"]["username"]);
				currRegistrationStatus = MessageType.RegisterError;
			}
		);	
	}
}
