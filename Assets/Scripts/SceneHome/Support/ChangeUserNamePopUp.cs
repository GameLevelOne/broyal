using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUserNamePopUp : BasePage {
	public NotificationPopUp notifPopUp;
	public ConnectingPanel connectingPanel;
	public Text userNameLabel;
	public Text profileUserName;
	public Button saveButton;

	protected override void Init ()
	{
		base.Init ();
		saveButton.interactable = false;
	}

	public void SaveClicked() {
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		connectingPanel.Connecting (true);
		DBManager.API.UpdateUserName (userNameLabel.text,
			(response) => {
				connectingPanel.Connecting (false);
				profileUserName.text = userNameLabel.text;
				Activate (false);
			}, 
			(error) => {
				connectingPanel.Connecting (false);
				notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("General.SERVER_ERROR"));
				notifPopUp.OnFinishOutro += AfterError;
			}
		);
	}

	void AfterError() {
		notifPopUp.OnFinishOutro-=AfterError;
		Activate (false);
	}

	public void CheckName() {
		if (userNameLabel.text != "") {
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
