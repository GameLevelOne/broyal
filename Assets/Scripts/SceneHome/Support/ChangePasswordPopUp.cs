using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class ChangePasswordPopUp : BasePage {
	public NotificationPopUp notifPopUp;
	public ConnectingPanel connectingPanel;
	public InputField currentPasswordLabel;
	public InputField newPasswordLabel;
	public InputField confirmPasswordLabel;
	public Button saveButton;

	protected override void Init ()
	{
		base.Init ();
		saveButton.interactable = false;
        currentPasswordLabel.text = "";
        newPasswordLabel.text = "";
        confirmPasswordLabel.text = "";

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
                    notifPopUp.ShowPopUp(LocalizationService.Instance.GetTextByKey("ForgotPassword.CHANGE_SUCCESS"));
					notifPopUp.OnFinishOutro += AfterPopUp;
				}, 
				(error) => {
					connectingPanel.Connecting (false);
					JSONNode jsonData = JSON.Parse (error);
					if (jsonData!=null) {
						notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
					} else {
						notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
					}
					notifPopUp.OnFinishOutro += AfterPopUp;
				}
			);
		}
	}

	void AfterPopUp() {
        notifPopUp.OnFinishOutro -= AfterPopUp;
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
