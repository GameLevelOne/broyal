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
		openingBidLabel.text = "Open Bid: IDR " + openingBid.ToString ("N0");
		nextIncrementLabel.text = "Next Increment: IDR " + nextIncrement.ToString ("N0");
		maxPriceLabel.text = "Max Price: IDR " + maxPrice.ToString ("N0");

		if (auctionState == AuctionState.PAST) {
			starsPanel.gameObject.SetActive (false);
			if (claimable) {
				actionButton.gameObject.SetActive (true);
				actionButtonLabel.text = "CLAIM";
			} else {
				actionButton.gameObject.SetActive (false);
			}
		} else if (auctionState == AuctionState.FUTURE) {
			actionButton.gameObject.SetActive (false);
		} else {
			actionButton.gameObject.SetActive (true);
			if (starsPrice == 0) {
				actionButtonLabel.text = "JOIN";
				starsPanel.gameObject.SetActive (false);
			} else {
				actionButtonLabel.text = "ENTER";
				starsPanel.gameObject.SetActive (true);
				starsPriceLabel.text = starsPrice.ToString ("N0");
			}
		}

	}

	public void OnRoomShow()
	{
		productImage.LoadImageFromUrl (imageUrl[0]);
	}

}
