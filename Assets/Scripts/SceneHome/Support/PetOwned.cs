using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetOwned : MonoBehaviour {
	public PetEquipPopUp petEquipPopUp;
	public ImageLoader petPicture;
	public Text petName;
	public Text petRank;
	public Button equipButton;

	PetData petData;

	public void InitData(PetData _petData) {
		if (_petData != null) {
//			Debug.Log("Pet Name: "+_petData.name);
			gameObject.SetActive (true);
			petData = _petData;
			petPicture.LoadImageFromUrl (petData.imageUrl);
			petName.text = petData.name;
			petRank.text = LocalizationService.Instance.GetTextByKey ("Header.PET_RANK") + petData.rank;
			equipButton.gameObject.SetActive(!petData.equipped);
		} else {
			gameObject.SetActive (false);
		}
	}

	public void EquipClick() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		petEquipPopUp.InitData (petData);
		petEquipPopUp.Activate (true);
	}
}
