using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPetNamePopUp : BasePage {
	public ConnectingPanel connectingPanel;
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
				ContinueError();
			}
		);
	}

	void ContinueError() {
		ContinueClicked ();
	}

	public void CheckName() {
		if (petNameLabel.text != "") {
			continueButton.interactable = true;
		} else {
			continueButton.interactable = false;
		}
	}
}
