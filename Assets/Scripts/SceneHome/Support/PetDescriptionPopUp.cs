using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class PetDescriptionPopUp : BasePage {
	public PetData petData;
	public ConnectingPanel connectingPanel;
	public EditPetNamePopUp petNamePopUp;
	public ImageLoader petImage;
	public RectTransform starsHLG;
	public Text priceLabel;
	public Text modelNameLabel;
	public Text descriptionLabel;

	// Use this for initialization
	public void InitData (PetData _petData) {
		petData = _petData;
		petImage.LoadImageFromUrl (petData.imageUrl);
		priceLabel.text = petData.price.ToString ("N0");
		LayoutRebuilder.ForceRebuildLayoutImmediate(starsHLG);
		modelNameLabel.text = petData.modelName;
		descriptionLabel.text = petData.description;
	}
		
	public void BuyClick() {
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        connectingPanel.Connecting(true);
		DBManager.API.PurchasePet (petData.id,
			(response) => {
				JSONNode jsonData = JSON.Parse(response);
				connectingPanel.Connecting (false);
				Activate(false);
				OnFinishOutro += FinishOutro;
			}, 
			(error) => {
				connectingPanel.Connecting (false);
			}
		);
	}

    public void CancelClick()
    {
        SoundManager.Instance.PlaySFX(SFXList.Button02);
        Activate(false);
    }

	void FinishOutro() {
		OnFinishOutro -= FinishOutro;
		petNamePopUp.Activate (true);
		petNamePopUp.InitData (petData);
	}
}
