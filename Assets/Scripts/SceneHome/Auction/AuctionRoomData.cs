using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BidRoyale.Core;

public enum AuctionState {
	PAST,
	CURRENT,
	FUTURE
}

public class AuctionRoomData : MonoBehaviour {

	public int auctionId;
	public AuctionState auctionState;
	public string[] imageUrl;
	public System.DateTime futureDates;
	public string productName;
	public int openingBid;
	public int nextIncrement;
	public int maxPrice;
	public int starsPrice;
	public string winnerName;
	public int winnerPrice;
	public string dateWin;
	public int participants;
	public bool claimable;

	public ImageLoader productImage;
	public GameObject futureInfo;
	public Text futureDatesLabel;
	public Text timeLeftLabel;
	public Text productNameLabel;
	public Text dateWinLabel;
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
		System.DateTime _futureDate,
		string _productName, 
		int _openingBid, 
		int _nextIncrement, 
		int _maxPrice, 
		int _starsPrice, 
		string _winnerName,
		int _winnerPrice,
		string _dateWin,
		int _participants,
		bool _claimable)
	{
		auctionId = _auctionId;
		auctionState = _auctionState;
		imageUrl = _imageUrl;
		futureDates = _futureDate;
		productName = _productName;
		openingBid = _openingBid;
		nextIncrement = _nextIncrement;
		maxPrice = _maxPrice;
		starsPrice = _starsPrice;
		winnerName = _winnerName;
		winnerPrice = _winnerPrice;
		dateWin = _dateWin;
		participants = _participants;
		claimable = _claimable;

		productImage.SetLoading ();
		if (auctionState == AuctionState.PAST) {
			productNameLabel.text = winnerName;
			dateWinLabel.gameObject.SetActive (true);
			dateWinLabel.text = dateWin;
			openingBidLabel.text = productName;
			nextIncrementLabel.text = LocalizationService.Instance.GetTextByKey("AuctionLobby.WON_PRICE") + ": " + winnerPrice.ToString ("IDR #,0;IDR -#,0;-");
			maxPriceLabel.text = LocalizationService.Instance.GetTextByKey("AuctionLobby.WINNER_OF") + ": " + participants.ToString ("N0") + " " +
				LocalizationService.Instance.GetTextByKey("AuctionLobby.PARTICIPANTS");
			starsPanel.gameObject.SetActive (false);
			if (claimable) {
				actionButton.gameObject.SetActive (true);
				actionButtonLabel.text = LocalizationService.Instance.GetTextByKey("AuctionLobby.CLAIM");
			} else {
				actionButton.gameObject.SetActive (false);
			}
		} else {
			productNameLabel.text = productName;
			dateWinLabel.gameObject.SetActive (false);
			openingBidLabel.text = LocalizationService.Instance.GetTextByKey("AuctionLobby.OPEN_BID") + ": " + openingBid.ToString ("IDR #,0;IDR -#,0;-");
			nextIncrementLabel.text = LocalizationService.Instance.GetTextByKey("AuctionLobby.NEXT_INCREMENT") + ": " + nextIncrement.ToString ("IDR #,0;IDR -#,0;-");
			maxPriceLabel.text = LocalizationService.Instance.GetTextByKey("AuctionLobby.MAX_PRICE") + ": " + maxPrice.ToString ("IDR #,0;IDR -#,0;-");
			actionButton.gameObject.SetActive (true);
			if (starsPrice == 0) {
				actionButtonLabel.text = LocalizationService.Instance.GetTextByKey("General.ENTER");
				starsPanel.gameObject.SetActive (false);
			} else {
				starsPanel.gameObject.SetActive (true);
				actionButtonLabel.text = LocalizationService.Instance.GetTextByKey("AuctionLobby.JOIN_FOR");
				starsPriceLabel.text = starsPrice.ToString ("#,0;-#,0;-");
			}
		}
			
		if (auctionState == AuctionState.FUTURE) {
			futureInfo.SetActive (true);
			futureDatesLabel.text = futureDates.ToString ("MMM dd, yyyy");
			timeLeftLabel.text = Utilities.TimeToNow (futureDates);
			StartCoroutine (AuctionCountDown());
		} else {
			futureInfo.SetActive (false);
		}
	}

	IEnumerator AuctionCountDown() {
		while (true) {
			yield return new WaitForSeconds (1);
			timeLeftLabel.text = Utilities.TimeToNow (futureDates);
		}
	}

	public void OnRoomShow()
	{
//		Debug.Log ("LoadURL: "+imageUrl[0]);
        if (imageUrl.Length>0) 
		    productImage.LoadImageFromUrl (imageUrl[0]);
	}

}
