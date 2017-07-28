using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpManager : MonoBehaviour {
	public GameObject panelSignIn;

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

	int genderOption; // 0 = male, 1 = female

	bool tickNewsletter = false;
	bool tickTnC = false;

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

	public void OnClickContinue (){ //verification through email, goto sign in page
		this.gameObject.SetActive(false);
		panelSignIn.SetActive(true);
	}

	void UpdateGenderOptionDisplay ()
	{
		if (genderOption == 0) {
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
}
