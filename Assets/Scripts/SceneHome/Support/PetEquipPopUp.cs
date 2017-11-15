using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class PetEquipPopUp : BasePage {
	public ProfilesManager profilesPanel;
	public NotificationPopUp notifPopUp;
	public PetData petData;
	public ConnectingPanel connectingPanel;
	public ImageLoader petImage;
	public Text nameLabel;
	public Text descriptionLabel;

	// Use this for initialization
	public void InitData (PetData _petData) {
		petData = _petData;
		petImage.LoadImageFromUrl (petData.imageUrl);
		nameLabel.text = petData.name;
		descriptionLabel.text = petData.description;
	}

	public void EquipClick() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		connectingPanel.Connecting(true);
		DBManager.API.EquipPet (petData.id,
			(response) => {
				JSONNode jsonData = JSON.Parse(response);
				profilesPanel.LoadPetData(()=>{Activate(false);});
			}, 
			(error) => {
				connectingPanel.Connecting (false);
				notifPopUp.ShowPopUp(LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
				notifPopUp.OnFinishOutro+=AfterError;
			}
		);
	}

	void AfterError() {
		notifPopUp.OnFinishOutro-=AfterError;
		Activate (false);
	}

	public void CancelClick()
	{
		SoundManager.Instance.PlaySFX(SFXList.Button02);
		Activate(false);
	}

}
