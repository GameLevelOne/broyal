using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteFBProfileManager : MonoBehaviour {
	public Image[] genderRadioButtons = new Image[2];
	public Sprite[] radioButtonOnOff = new Sprite[2]; //0 = off, 1 = on

	string inputEmail;
	string inputPhone;
	int genderOption; // 0 = male, 1 = female

	public void GetInputEmail (InputField obj){
		inputEmail = obj.text;
	}

	public void GetInputPhone (InputField obj){
		inputPhone = obj.text;
	}

	public void GetInputGenderOption(int opt){
		genderOption = opt;
		UpdateGenderOptionDisplay();
	}

	public void OnClickSubmit(){
		this.gameObject.SetActive(false);
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
}
