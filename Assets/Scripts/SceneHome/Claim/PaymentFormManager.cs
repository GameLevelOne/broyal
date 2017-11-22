using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using SimpleJSON;

public class PaymentFormManager : BasePage {

	public ConnectingPanel connectingPanel;
	public NotificationPopUp notifPopUp;

	public Animator roomAnimator;
	public GameObject methodLayer;
	public GameObject shippingLayer;
	public ToggleGroup methodGroup;

	public Text productNameLabel;
	public Text itemPriceLabel;
	public Text adminFeeLabel;
	public Text paymentFeeLabel;
	public Text taxLabel;
	public Text shippingHandlingLabel;
	public Text grandTotalLabel;
	public Text nameLabel;
	public Text addressLabel;
	public Text cityLabel;
	public Text phoneLabel;

	int methodIdx;
	int auctionId;

	protected override void Init ()
	{
		base.Init ();
		roomAnimator.Play ("PanelLeft");
		roomAnimator.ResetTrigger ("GoLeft");
		roomAnimator.ResetTrigger ("GoRight");
		methodIdx = -1;
		methodGroup.SetAllTogglesOff ();
	}

	public void InitData(int _auctionId) {
		auctionId = _auctionId;
	}

	public void ClickBack() {
		SoundManager.Instance.PlaySFX(SFXList.Button02);
		if (shippingLayer.activeSelf) {
			roomAnimator.SetTrigger ("GoLeft");
		} else {
			NextPage ("LOBBY");
		}
	}

	public void ClickNext() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		if (methodIdx != -1) {
			roomAnimator.SetTrigger ("GoRight");
		}
	}
	public void ClickMethod() {
		Toggle curSelected = methodGroup.ActiveToggles ().FirstOrDefault ();
		methodIdx = curSelected.transform.GetSiblingIndex ();
	}

	public void ClickEdit() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		ProfilesManager futurePage = (ProfilesManager)PagesManager.instance.GetPagesByName("PROFILES");
		futurePage.initProfileType = 0;
		futurePage.prevPage = PagesManager.instance.GetCurrentPage ();	
		NextPage ("PROFILES");
	}

	void LoadShippingData() {
		connectingPanel.Connecting (true);
		DBManager.API.GetPaymentDetails (auctionId,
			(response) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse(response);
				productNameLabel.text = jsonData["productName"];
				itemPriceLabel.text = LocalizationService.Instance.GetTextByKey("PaymentForm.ITEM_PRICE") + jsonData["productPrice"].AsInt.ToString ("IDR #,0;IDR -#,0;-");
				adminFeeLabel.text = LocalizationService.Instance.GetTextByKey("PaymentForm.ADMIN_FEE") + jsonData["adminFee"].AsInt.ToString ("IDR #,0;IDR -#,0;-");
				paymentFeeLabel.text = LocalizationService.Instance.GetTextByKey("PaymentForm.PAYMENT_FEE") + jsonData["paymentFee"].AsInt.ToString ("IDR #,0;IDR -#,0;-");
				taxLabel.text = LocalizationService.Instance.GetTextByKey("PaymentForm.TAX") + jsonData["tax"].AsInt.ToString ("IDR #,0;IDR -#,0;-");
				shippingHandlingLabel.text = LocalizationService.Instance.GetTextByKey("PaymentForm.SHIPPING_HANDLING") + jsonData["shipping"].AsInt.ToString ("IDR #,0;IDR -#,0;-");
				grandTotalLabel.text = LocalizationService.Instance.GetTextByKey("PaymentForm.GRAND_TOTAL") + jsonData["grandTotal"].AsInt.ToString ("IDR #,0;IDR -#,0;-");

				nameLabel.text = jsonData["receiverName"];
				addressLabel.text = jsonData["address1"];
				cityLabel.text = jsonData["address2"];
				phoneLabel.text = jsonData["phone"];
			}, 
			(error) => {
				connectingPanel.Connecting (false);
				notifPopUp.ShowPopUp(LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
			}
		);
	}
}
