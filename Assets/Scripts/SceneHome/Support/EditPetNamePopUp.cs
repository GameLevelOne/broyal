using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class EditPetNamePopUp : BasePage {
	public NotificationPopUp notifPopUp;
	public ConnectingPanel connectingPanel;
	public ProfilesManager profilesManager;
	public Text priceLabel;
	public InputField petNameLabel;
	public Button saveButton;

	protected override void Init ()
	{
		base.Init ();
		saveButton.interactable = false;
        petNameLabel.text = "";
	}

	public void InitPrice(int price) {
		priceLabel.text = price.ToString ("N0") + " ?";
	}
		
	public void SaveClicked() {
        SoundManager.Instance.PlaySFX(SFXList.Button01);
		connectingPanel.Connecting (true);
		DBManager.API.ChangePetName (petNameLabel.text,
			(response) => {
				connectingPanel.Connecting (false);
				Activate (false);
				profilesManager.LoadPetData();
			}, 
			(error) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse (error);
				if (jsonData!=null) {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
				} else {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
				}
				notifPopUp.OnFinishOutro += AfterError;
			}
		);
	}

	void AfterError() {
		notifPopUp.OnFinishOutro-=AfterError;
		Activate (false);
	}

	public void CheckName() {
		if (petNameLabel.text != "") {
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
