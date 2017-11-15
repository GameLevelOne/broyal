﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePasswordPopUp : BasePage {
	public NotificationPopUp notifPopUp;
	public ConnectingPanel connectingPanel;
	public Text currentPasswordLabel;
	public Text newPasswordLabel;
	public Text confirmPasswordLabel;
	public Button saveButton;

	protected override void Init ()
	{
		base.Init ();
		saveButton.interactable = false;
	}

	public void SaveClicked() {
		SoundManager.Instance.PlaySFX (SFXList.Button01);
		if (confirmPasswordLabel.text != newPasswordLabel.text) {
			notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("SignUp.ERROR_PASSWORD_MATCH"));
		} else {
			connectingPanel.Connecting (true);
			DBManager.API.UserChangePassword (currentPasswordLabel.text, newPasswordLabel.text, confirmPasswordLabel.text,
				(response) => {
					connectingPanel.Connecting (false);
					Activate (false);
				}, 
				(error) => {
					connectingPanel.Connecting (false);
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("General.SERVER_ERROR"));
					notifPopUp.OnFinishOutro += AfterError;
				}
			);
		}
	}

	void AfterError() {
		notifPopUp.OnFinishOutro-=AfterError;
		Activate (false);
	}

	public void CheckFields() {
		if ((currentPasswordLabel.text != "") && (newPasswordLabel.text != "") && (confirmPasswordLabel.text != "")) {
			saveButton.interactable = true;
		} else {
			saveButton.interactable = false;
		}
	}

	public void CancelClick()
	{
		SoundManager.Instance.PlaySFX(SFXList.Button02);
		Activate(false);
	}
}
