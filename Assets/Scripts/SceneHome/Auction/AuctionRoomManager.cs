using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class AuctionRoomManager : BasePage {
	public Fader fader;
	public ConnectingPanel connectingPanel;

	public int auctionId;
	public string[] imageUrl;
	public string productName;
	public int currentPrice;
	public int nextIncrement;
	public int maxPrice;
	public string productCategory;
	public int productLength;
	public int productWidth;
	public int productHeight;
	public string productWeight;
	public string productDescription;
	public int numberBidders;
	public long timeToNextIncrement;

	public Animator roomAnimator;
	public Text auctionIdRoomlabel;
	public ImageLoader roomImage;
	public GameObject biddersLayer;
	public Text numberBiddersLabel;
	public Animator countDownAnim;
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

	void Start()
	{
		scrollSnapDetail.OnChangePage += OnChangePage;
	}

	protected override void Init ()
	{
		roomAnimator.Play ("PanelLeft");
		roomAnimator.ResetTrigger ("GoLeft");
		roomAnimator.ResetTrigger ("GoRight");

		biddersLayer.SetActive (false);
		countDownAnim.Play ("CountDownRun");
		countDownAnim.gameObject.SetActive (false);

		connectingPanel.Connecting (true);
		DBManager.API.GetAuctionDetails (auctionId,
			(response) => {
				connectingPanel.Connecting (false);
				Debug.Log (response);
				roomImage.LoadImageFromUrl (imageUrl [0]);
				JSONNode jsonData = JSON.Parse(response);
				//Data Init
				imageUrl = new string[3];
				imageUrl[0] = jsonData["productImages"][0];
				imageUrl[1] = jsonData["productImages"][1];
				imageUrl[2] = jsonData["productImages"][2];
				productName = jsonData["productName"];
				currentPrice = jsonData["openBid"].AsInt;
				nextIncrement = jsonData["nextIncrement"].AsInt;
				maxPrice = jsonData["maxPrice"].AsInt;
				productCategory = jsonData["category"];
				productLength = jsonData["length"].AsInt;
				productWidth = jsonData["width"].AsInt;
				productHeight = jsonData["height"].AsInt;
				productWeight = jsonData["weight"];
				productDescription = jsonData["description"];
				numberBidders = jsonData["numberBidders"].AsInt;
				timeToNextIncrement = jsonData["timeToNextIncrement"].AsInt;

				//Display Data Room
				auctionIdRoomlabel.text = "<#"+auctionId+">";
				roomImage.LoadImageFromUrl(imageUrl[0]);
				if (numberBidders>0) {
					biddersLayer.gameObject.SetActive(true);
					numberBiddersLabel.text = numberBidders.ToString("N0");
				}
				StartCoroutine(IncrementCountdown());
				productNameLabel.text = productName;
				currentPriceLabel.text = LocalizationService.Instance.GetTextByKey("AuctionRoom.CURRENT_PRICE") + ": " + currentPrice.ToString ("IDR #,0;IDR -#,0;-");
				nextIncrementLabel.text = LocalizationService.Instance.GetTextByKey("AuctionRoom.NEXT_INCREMENT") + ": " + nextIncrement.ToString ("IDR #,0;IDR -#,0;-");
				maxPriceLabel.text = LocalizationService.Instance.GetTextByKey("AuctionRoom.MAX_PRICE") + ": " + maxPrice.ToString ("IDR #,0;IDR -#,0;-");
				//Display Data Details
				auctionIdDetailLabel.text = "<#"+auctionId+">";
				detailsDataLabel.text = LocalizationService.Instance.GetTextByKey("AuctionRoom.CATEGORY") + ": " + productCategory;
				detailsDataLabel.text += "\n" + LocalizationService.Instance.GetTextByKey("AuctionRoom.DIMENSION") + ": ";
				detailsDataLabel.text += productLength + " x " + productWidth + " x " + productHeight;
				detailsDataLabel.text += "\n" + LocalizationService.Instance.GetTextByKey("AuctionRoom.WEIGHT") + ": " + productWeight;
				detailsDescriptionLabel.text = productDescription;

			},
			(error) => {
				connectingPanel.Connecting (false);
			});
	}

	IEnumerator IncrementCountdown()
	{
		bool starting = false;
		while (timeToNextIncrement > 0) {
			starting = true;
			timeToNextIncrement--;

			if (timeToNextIncrement <= 5000) {
				countDownAnim.gameObject.SetActive (true);
			}
			yield return new WaitForSeconds (0.001f);
		}

		if (starting) {
			//Go to Game
		}
	}

	public void ClickBid(){
	}

	public void ClickBack(){		
		NextPage ("LOBBY");
	}

	public void ClickRight(){
		scrollSnapDetail.UpdateContainerSize (0);		
		detailImages [0].LoadImageFromUrl (imageUrl[0]);
		roomAnimator.SetTrigger("GoRight");
	}

	public void ClickLeft(){
		roomAnimator.SetTrigger("GoLeft");
	}

	public void OnChangePage(Transform child)
	{
		int index = child.GetSiblingIndex ();
		detailImages [index].LoadImageFromUrl (imageUrl[index]);
	}

	void OnDestroy()
	{
		scrollSnapDetail.OnChangePage -= OnChangePage;
	}
}
