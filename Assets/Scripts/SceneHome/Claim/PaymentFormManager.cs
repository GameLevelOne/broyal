using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using SimpleJSON;

public class PaymentFormManager : BasePage {

	public ConnectingPanel connectingPanel;
	public NotificationPopUp notifPopUp;
	public UniWebView uniWebView;

	public Animator roomAnimator;
	public GameObject methodLayer;
	public GameObject shippingLayer;
	public ToggleGroup methodGroup;
	public Toggle[] payments;
	public Text paymentButtonLabel;

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

	string paymentId = "";
	string webViewUrl = "";	
	int itemId;
	int purchaseType;

	protected override void Init ()
	{
		base.Init ();
		if (purchaseType == 1) {
			roomAnimator.Play ("PanelLeft");
			roomAnimator.ResetTrigger ("GoLeft");
			roomAnimator.ResetTrigger ("GoRight");
			methodGroup.SetAllTogglesOff ();
			paymentButtonLabel.text = LocalizationService.Instance.GetTextByKey ("PaymentForm.CHECKOUT");
		} else {
			if (paymentId == "") {
				roomAnimator.Play ("PanelLeft");
				roomAnimator.ResetTrigger ("GoLeft");
				roomAnimator.ResetTrigger ("GoRight");
				methodGroup.SetAllTogglesOff ();
			} else {
				roomAnimator.Play ("PanelRight");
				roomAnimator.ResetTrigger ("GoLeft");
				roomAnimator.ResetTrigger ("GoRight");
			}
			LoadShippingData ();
			paymentButtonLabel.text = LocalizationService.Instance.GetTextByKey ("Game.NEXT");
		}
	}

	public void InitData(int _purchaseType,int _itemId) {
		purchaseType = _purchaseType;
		itemId = _itemId;
	}

	public void ClickBack() {
		SoundManager.Instance.PlaySFX(SFXList.Button02);
		if (shippingLayer.GetComponent<CanvasGroup>().alpha==1f) {
			roomAnimator.SetTrigger ("GoLeft");
		} else {
			Debug.Log ("Helooo");
			if (purchaseType == 1) {
				NextPage ("SHOP");
			} else {
				NextPage ("LOBBY");
			}
		}
	}

	public void ClickNext() {
		if (purchaseType == 1) {
			ClickCheckout ();
		} else {
			SoundManager.Instance.PlaySFX(SFXList.Button01);
			if (paymentId != "") {
				roomAnimator.SetTrigger ("GoRight");
			}
		}
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
		DBManager.API.GeneratePreOrder (purchaseType,itemId,"02",
			(response) => {
				StartCoroutine(GetPaymentDetailsDelayed(0.5f));
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

	IEnumerator GetPaymentDetailsDelayed(float delay) {
		yield return new WaitForSeconds (delay);
		DBManager.API.GetPaymentDetails (itemId,
			(response2) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse(response2);
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
			(error2) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse (error2);
				if (jsonData!=null) {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
				} else {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
				}
			}
		);					
	}

	public void ChangePaymentSelection(string paymentCode) {
		paymentId = paymentCode;
	}

	public void ClickCheckout() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		connectingPanel.Connecting (true);
		DBManager.API.GeneratePreOrder (purchaseType,itemId,paymentId,
			(response) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse(response);
				webViewUrl = jsonData["redirectUrl"];

				if (webViewUrl==uniWebView.url) {
					uniWebView.OnWebViewShouldClose += ShouldClose;
					uniWebView.Show(true);
				} else {
					connectingPanel.Connecting (true); 
					uniWebView.url = webViewUrl;
					uniWebView.Load ();
					uniWebView.OnLoadComplete += LoadComplete;
				}
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

	void LoadComplete(UniWebView webView, bool success, string errorMessage) {
		webView.OnLoadComplete -= LoadComplete;
		connectingPanel.Connecting (false); 
		if (success) {
			ShowHideWebView (true);
		} else {
			notifPopUp.ShowPopUp (errorMessage);
		}
	}

	bool ShouldClose(UniWebView webView) {
		ShowHideWebView (false);
		return false;
	}

	void ReceivedCallback(UniWebView webView, UniWebViewMessage message) {
		ShowHideWebView (false);
		if (message.path == "success") {
			if (purchaseType == 1) {
				NextPage ("SHOP");
			} else {
				notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("PaymentForm.PAYMENT_SUCCESS"));
				notifPopUp.OnFinishOutro += AfterSuccessPopUp;
			}
		} else {
			notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey ("PaymentForm.PAYMENT_FAILED"));
		}
	}

	void AfterSuccessPopUp() {
		notifPopUp.OnFinishOutro -= AfterSuccessPopUp;
		NextPage ("LOBBY");
	}

	void ShowHideWebView(bool show) {
		if (show) {
			uniWebView.OnWebViewShouldClose += ShouldClose;
			uniWebView.OnReceivedMessage += ReceivedCallback;
			uniWebView.Show(true);
		} else {
			uniWebView.OnWebViewShouldClose -= ShouldClose;
			uniWebView.OnReceivedMessage -= ReceivedCallback;
			uniWebView.Hide(true);
		}
	}

}
