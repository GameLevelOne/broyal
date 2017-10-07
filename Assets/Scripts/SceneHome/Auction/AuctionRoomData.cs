using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AuctionState {
	PAST,
	CURRENT,
	FUTURE
}

public class AuctionRoomData : MonoBehaviour {

	public int auctionId;
	public AuctionState auctionState;
	public string[] imageUrl;
	public string productName;
	public int openingBid;
	public int nextIncrement;
	public int maxPrice;
	public int starsPrice;
	public bool claimable;

	public ImageLoader productImage;
	public Text productNameLabel;
	public Text openingBidLabel;
	public Text nextIncrementLabel;
	public Text maxPriceLabel;
	public Button actionButton;
	public GameObject starsPanel;
	public Text actionButtonLabel;
	public Text starsPriceLabel;


	public void InitData(int _auctionId, 
		AuctionState _auctionState, 
		string[] _imageUrl, 
		string _productName, 
		int _openingBid, 
		int _nextIncrement, 
		int _maxPrice, 
		int _starsPrice, 
		bool _claimable)
	{
		auctionId = _auctionId;
		auctionState = _auctionState;
		imageUrl = _imageUrl;
		productName = _productName;
		openingBid = _openingBid;
		nextIncrement = _nextIncrement;
		maxPrice = _maxPrice;
		starsPrice = _starsPrice;
		claimable = _claimable;

		productImage.SetLoading ();
		productNameLabel.text = productName;
		openingBidLabel.text = LocalizationService.Instance.GetTextByKey("AuctionLobby.OPEN_BID") + ": " + openingBid.ToString ("IDR #,0;IDR -#,0;-");
		nextIncrementLabel.text = LocalizationService.Instance.GetTextByKey("AuctionLobby.NEXT_INCREMENT") + ": " + nextIncrement.ToString ("IDR #,0;IDR -#,0;-");
		maxPriceLabel.text = LocalizationService.Instance.GetTextByKey("AuctionLobby.MAX_PRICE") + ": " + maxPrice.ToString ("IDR #,0;IDR -#,0;-");

		if (auctionState == AuctionState.PAST) {
			starsPanel.gameObject.SetActive (false);
			if (claimable) {
				actionButton.gameObject.SetActive (true);
				actionButtonLabel.text = LocalizationService.Instance.GetTextByKey("AuctionLobby.CLAIM");
			} else {
				actionButton.gameObject.SetActive (false);
			}
		} else if (auctionState == AuctionState.FUTURE) {
			actionButton.gameObject.SetActive (false);
		} else {
			actionButton.gameObject.SetActive (true);
			actionButtonLabel.text = LocalizationService.Instance.GetTextByKey("AuctionLobby.ENTER");
			if (starsPrice == 0) {
				starsPanel.gameObject.SetActive (false);
			} else {
				starsPanel.gameObject.SetActive (true);
				starsPriceLabel.text = starsPrice.ToString ("#,0;-#,0;-");
			}
		}

	}

	public void OnRoomShow()
	{
//		Debug.Log ("LoadURL: "+imageUrl[0]);
		productImage.LoadImageFromUrl (imageUrl[0]);
	}

}
