using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuctionRoomManager : BasePage {
	public Fader fader;
	public ConnectingPanel connectingPanel;

	public int auctionId;
	public string[] imageUrl;
	public string productName;
	public int currentPrice;
	public int nextIncrement;
	public int maxPrice;
	public int productCategory;
	public int productLength;
	public int productWidth;
	public int productHeight;
	public int productWeight;
	public int productDescription;

	public Animator roomAnimator;
	public Text auctionIdRoomlabel;
	public ImageLoader roomImage;
	public Text productNameLabel;
	public Text currentPriceLabel;
	public Text nextIncrementLabel;
	public Text maxPriceLabel;
	public Text auctionIdDetailLabel;
	public ImageLoader[] detailImages;
	public ScrollSnapRect scrollSnapDetail;
	public Text detailsDataLabel;
	public Text detailsDescriptionLabel;
	public Image bannerImage;

	public void ClickBid(){
	}

	public void ClickBack(){		
		NextPage ("LOBBY");
	}

	public void ClickRight(){
		roomAnimator.SetTrigger("GoRight");
	}

	public void ClickLeft(){
		roomAnimator.SetTrigger("GoLeft");
	}

}
