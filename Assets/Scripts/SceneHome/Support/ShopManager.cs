using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using BidRoyale.Core;

public class ShopManager : BasePage {
	public ConnectingPanel connectingPanel;
	public HeaderAreaManager header;
	public PetDescriptionPopUp petDescription;
	public NotificationPopUp notificationPopUp;
	public GameObject starsLoading;
	public GameObject petsLoading;
	public GameObject starsScroll;
	public GameObject petsScroll;

	public RectTransform starsContainer;
	public RectTransform petsContainer;
	public GameObject itemStarsPrefab;
	public GameObject itemPetsPrefab;

	protected override void Init ()
	{
		base.Init ();
		LoadStars ();
		LoadPets ();
	}

	void LoadStars() {
		Utilities.ClearChildren (starsContainer);
		starsLoading.SetActive (true);
		starsScroll.SetActive (false);
		DBManager.API.GetShopList (
			(response) => {
				JSONNode jData = JSON.Parse(response);
				starsLoading.SetActive (false);
				starsScroll.SetActive (true);
				for (int i=0; i<jData["starShopList"].Count;i++) {
					GameObject g = Instantiate(itemStarsPrefab);
					g.transform.SetParent(starsContainer);
					g.transform.localScale = Vector3.one;
					ShopItemStars sis = g.GetComponent<ShopItemStars>();
//					Debug.Log("StarsData: "+jData["starShopList"][i]);
					int starValue = jData["starShopList"][i]["starValue"].AsInt;
					sis.InitData(
						jData["starShopList"][i]["currencyValue"].AsInt,
						jData["starShopList"][i]["itemImage"],
						starValue
					);
					sis.buyButton.onClick.AddListener(()=>{BuyStars(starValue);});
				}
			},
			(error) => {
				notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
			}
		);
	}		

	void LoadPets() {
		Utilities.ClearChildren (petsContainer);
		petsLoading.SetActive (true);
		petsScroll.SetActive (false);
		DBManager.API.PetListing (
			(response) => {
				JSONNode jData = JSON.Parse(response);
				petsLoading.SetActive (false);
				petsScroll.SetActive (true);
				for (int i=0; i<jData["petList"].Count;i++) {
					GameObject g = Instantiate(itemPetsPrefab);
					g.transform.SetParent(petsContainer);
					g.transform.localScale = Vector3.one;
					ShopItemPets sip = g.GetComponent<ShopItemPets>();
					PetData petData = new PetData();
					petData.InitShopProfile(
						jData["petList"][i]["id"].AsInt,
						jData["petList"][i]["petModelImage"],
						jData["petList"][i]["petModelName"],
						jData["petList"][i]["petPrice"].AsInt,
						jData["petList"][i]["petDescription"]
					);
					sip.InitData(petData);
					sip.GetComponent<Button>().onClick.AddListener(()=>{ClickPet(petData);});
				}
			},
			(error) => {
				notificationPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
			}
		);
	}


	public void ClickPet (PetData petData){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        petDescription.Activate(true);
		petDescription.InitData (petData);
	}

	//Cheat
	public void BuyStars(int amount) {
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        connectingPanel.Connecting(true);
		DBManager.API.CreateTopUp (amount,
			(response) => {
				connectingPanel.Connecting (false);
				JSONNode jData = JSON.Parse(response);
				header.SetStars(jData["availableStars"]);
			},
			(error) => {
				connectingPanel.Connecting (false);
			}
		);
	}

}
