using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPets : MonoBehaviour {
	public PetData petData;
	public ImageLoader petIcon;
	public Text petNameLabel;
	public Text petPriceLabel;

	public void InitData(PetData _petData) {
		petData = _petData;
		petIcon.LoadImageFromUrl (petData.imageUrl);
		petNameLabel.text = petData.modelName;
		petPriceLabel.text = petData.price.ToString ("#,0;-#,0;-");
	}
}
