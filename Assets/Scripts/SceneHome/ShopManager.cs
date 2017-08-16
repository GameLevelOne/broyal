using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {
	public GameObject panelPetDescription;
	public GameObject panelPetNaming;

	public void OnClickPetDesc(){
		panelPetDescription.SetActive(true);
	}

	public void OnClickBuy (){
		panelPetDescription.SetActive(false);
		panelPetNaming.SetActive(true);
	}

	public void OnClickCancel(){
		panelPetDescription.SetActive(false);
	}

	public void OnClickConfirmPetName(){
		panelPetNaming.SetActive(false);
	}
}
