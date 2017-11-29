using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class InputPetNamePopUp : BasePage {
	public ConnectingPanel connectingPanel;
	public NotificationPopUp notifPopUp;
	public HeaderAreaManager header;
	public Text petNameLabel;
	public Button continueButton;

	protected override void Init ()
	{
		base.Init ();
		continueButton.interactable = false;
	}

	public void ContinueClicked() {
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        connectingPanel.Connecting(true);
		DBManager.API.ChangePetName (petNameLabel.text,
			(response) => {
				connectingPanel.Connecting (false);
				Activate(false);
				header.GetPetProfile();
			}, 
			(error) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse (error);
				if (jsonData!=null) {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
				} else {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
				}
			}
		);
	}

	public void CheckName() {
		if (petNameLabel.text != "") {
			continueButton.interactable = true;
		} else {
			continueButton.interactable = false;
		}
	}
}
