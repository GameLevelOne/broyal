using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public enum AuctionMode
{
	BIDROYALE,
	BIDRUMBLE
}

public class AuctionLobbyManager : BasePage {
	public ConnectingPanel connectingPanel;
	public AuctionCarrouselPopUp carrouselPopUp;

	public AuctionMode auctionMode;
	public int auctionIndex;
	public Image auctionLogo;
	public Sprite[] spriteLogo;

	public ScrollSnapRect scrollSnap;
	public Transform container;
	public Transform unused;
	public AuctionRoomData[] rooms;
	public Transform firstLoading;
	public Transform lastLoading;
	int minData;
	int maxData;
	int bufferData = 4;

	void Start()
	{
		scrollSnap.OnChangePage += OnChangeRoomPage;
	}

	protected override void Init ()
	{
		base.Init ();
		auctionLogo.sprite = spriteLogo [(int)auctionMode];
		minData = -3;
		maxData = 3;
		LoadData (minData, maxData);
	}

	void LoadData(int start, int end)
	{
		connectingPanel.Connecting (true);
		DBManager.API.GetAuctionListing ((int)auctionMode + 1, start, end,
			(response) => {
//				Debug.Log("SuccessResult");
				connectingPanel.Connecting(false);
				JSONNode jsonData = JSON.Parse(response);
				ReturnAllContainer();
				Debug.Log(response);
				int totalData = 0;
				AuctionRoomData data;
				for (int i=0;i<jsonData["pastAuctions"].Count;i++) {
					data = rooms[totalData];
					string[] imgUrl = new string[3];
					imgUrl[0] = jsonData["pastAuctions"][i]["productImages"][0];
					imgUrl[1] = jsonData["pastAuctions"][i]["productImages"][1];
					imgUrl[2] = jsonData["pastAuctions"][i]["productImages"][2];
					data.InitData(
						jsonData["pastAuctions"][i]["auctionId"].AsInt,
						AuctionState.PAST,
						imgUrl,
						jsonData["pastAuctions"][i]["productName"],
						0,
						0,
						0,
						0,
						jsonData["pastAuctions"][i]["claimable"].AsBool
					);
					totalData++;
				}

				if (start<minData) {
					minData = (start < (minData - (jsonData["pastAuctions"].Count-1)) ) ? start : (minData - (jsonData["pastAuctions"].Count-1)) ;
				} else {
					minData = start;
				}

				int startPage = totalData;
				if ( (jsonData["currentAuction"].IsNull) || (jsonData["currentAuction"]=="{}") ) {
					Debug.Log("NO Current");
				} else {
					data = rooms[totalData];
					string[] imgUrl = new string[3];
					imgUrl[0] = jsonData["currentAuction"]["productImages"][0];
					imgUrl[1] = jsonData["currentAuction"]["productImages"][1];
					imgUrl[2] = jsonData["currentAuction"]["productImages"][2];
					data.InitData(
						jsonData["currentAuction"]["auctionId"].AsInt,
						AuctionState.CURRENT,
						imgUrl,
						jsonData["currentAuction"]["productName"],
						jsonData["currentAuction"]["openBid"].AsInt,
						jsonData["currentAuction"]["nextIncrement"].AsInt,
						jsonData["currentAuction"]["maxPrice"].AsInt,
						jsonData["currentAuction"]["enterPrice"].AsInt,
						jsonData["currentAuction"]["claimable"].AsBool
					);
					totalData++;
				}
				for (int i=0;i<jsonData["futureAuctions"].Count;i++) {
					data = rooms[totalData];
					string[] imgUrl = new string[3];
					imgUrl[0] = jsonData["pastAuctions"][i]["productImages"][0];
					imgUrl[1] = jsonData["pastAuctions"][i]["productImages"][1];
					imgUrl[2] = jsonData["pastAuctions"][i]["productImages"][2];
					data.InitData(
						jsonData["futureAuctions"][i]["auctionId"].AsInt,
						AuctionState.FUTURE,
						imgUrl,
						jsonData["futureAuctions"][i]["productName"],
						jsonData["futureAuctions"][i]["openBid"].AsInt,
						jsonData["futureAuctions"][i]["nextIncrement"].AsInt,
						jsonData["futureAuctions"][i]["maxPrice"].AsInt,
						jsonData["futureAuctions"][i]["enterPrice"].AsInt,
						jsonData["futureAuctions"][i]["claimable"].AsBool
					);
					totalData++;
				}

				if (end > maxData) {
					maxData = (end > (maxData + (jsonData["futureAuctions"].Count-1)) ) ? end : (maxData + (jsonData["futureAuctions"].Count-1)) ;
				} else {
					maxData = end;
				}
				Debug.Log("TotalData: "+totalData);
				for (int i=totalData;i<rooms.Length;i++)
				{
					rooms[i].transform.SetParent(unused);
				}
				if (startPage<totalData) {
					scrollSnap.UpdateContainerSize(startPage+1);
				} else {
					scrollSnap.UpdateContainerSize(startPage);
				}

			},
			(error) => {
				connectingPanel.Connecting(false);
			});	
	}
		
	public void ReturnAllContainer()
	{
		firstLoading.SetAsFirstSibling ();
		for (int i = 0; i < rooms.Length; i++) {
			rooms [i].transform.SetParent (container);
			rooms [i].transform.SetSiblingIndex (i + 1);
		}
		lastLoading.SetAsLastSibling ();
	}

	void OnChangeRoomPage(Transform child)
	{
		if (child == firstLoading) {
			LoadData (minData-bufferData,maxData-bufferData);
		} else if (child == lastLoading) {
			LoadData (minData+bufferData,maxData+bufferData);
		} else {
			AuctionRoomData data = child.GetComponent<AuctionRoomData> ();
			data.OnRoomShow ();
		}
	}

	public void ClickJoin (){
		auctionIndex = 0;
		NextPage ("AUCTIONROOM");
	}

	public void ClickClaim(){
//		panelClaimConfirmation.SetActive(true);
	}

	public void ClickImageDetail(int index){
		carrouselPopUp.imageUrl = rooms [index].imageUrl;
		Debug.Log ("PopUpIndex: "+index);
		carrouselPopUp.Activate (true);
	}

	void OnDestroy()
	{
		scrollSnap.OnChangePage += OnChangeRoomPage;
	}
}
