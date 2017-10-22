using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using BidRoyale.Core;

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
	public int timeToNextCycle;

	public Animator roomAnimator;
	public Text auctionIdRoomlabel;
	public ImageLoader roomImage;
	public GameObject biddersLayer;
	public Text numberBiddersLabel;
	public GameObject countDownLayer;
	public Text countDownLabel;
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
	public Button bidButton;

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
				productCategory = jsonData["productCategory"];
				productLength = jsonData["length"].AsInt;
				productWidth = jsonData["width"].AsInt;
				productHeight = jsonData["height"].AsInt;
				productWeight = jsonData["weight"] + " " + jsonData["unit"];
				productDescription = jsonData["description"];
				numberBidders = jsonData["noOfLastCycleBidders"].AsInt;
				timeToNextCycle = jsonData["timeToNextCycle"].AsInt;
				bidButton.interactable = jsonData["bidEnable"].AsBool;

				//Display Data Room
				auctionIdRoomlabel.text = "<#"+auctionId+">";
				roomImage.LoadImageFromUrl(imageUrl[0]);
				if (numberBidders>0) {
					biddersLayer.gameObject.SetActive(true);
					numberBiddersLabel.text = numberBidders.ToString("N0");
				}
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

				if ((numberBidders<=0) && (bidButton.interactable)) {
					GoToGame();
				} else {
					if (timeToNextCycle>0)
						StartCoroutine(IncrementCountdown());
				}
					
			},
			(error) => {
				connectingPanel.Connecting (false);
			});
	}

	IEnumerator IncrementCountdown()
	{
		while (timeToNextCycle > 0) {
			timeToNextCycle--;

			if (timeToNextCycle <= 5) {
				countDownAnim.gameObject.SetActive (true);
				countDownLayer.SetActive (false);
			} else if (timeToNextCycle > 5) {
				countDownLayer.SetActive (false);
				countDownLabel.text = Utilities.SecondsToMinutes (timeToNextCycle);
			}
			yield return new WaitForSeconds (1);
		}
		if (nextIncrement == maxPrice) {
			GoToGame ();
		} else {
			Init ();
		}
	}

	public void ClickBid(){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        connectingPanel.Connecting(true);
		DBManager.API.AuctionBidding (auctionId,
			(response)=>{
				JSONNode jsonData = JSON.Parse(response);
				bidButton.interactable = false;
				connectingPanel.Connecting (false);
			},
			(error)=>{
				connectingPanel.Connecting (false);
			}
		);
	}

	public void GoToGame()
	{
		//Go to Game
	}

	public void ClickBack(){
        SoundManager.Instance.PlaySFX(SFXList.Button02);
        NextPage("LOBBY");
	}

	public void ClickRight(){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        scrollSnapDetail.UpdateContainerSize(0);		
		detailImages [0].LoadImageFromUrl (imageUrl[0]);
		roomAnimator.SetTrigger("GoRight");
	}

	public void ClickLeft(){
        SoundManager.Instance.PlaySFX(SFXList.Button01);
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
