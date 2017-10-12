using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using BidRoyale.Core;

public enum AuctionMode
{
	BIDROYALE,
	BIDRUMBLE
}

public class AuctionLobbyManager : BasePage {
	public ConnectingPanel connectingPanel;
	public AuctionCarrouselPopUp carrouselPopUp;
	public AuctionRoomManager auctionRoomManager;
	public HeaderAreaManager header;

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
						DateTime.Now,
						jsonData["pastAuctions"][i]["productName"],
						0,
						0,
						0,
						0,
						jsonData["pastAuctions"][i]["winnerName"],
						jsonData["pastAuctions"][i]["winningPrice"].AsInt,
						jsonData["pastAuctions"][i]["winningDate"],
						jsonData["pastAuctions"][i]["noOfParticipants"].AsInt,
						jsonData["pastAuctions"][i]["claimable"].AsBool
					);
					data.actionButton.onClick.RemoveAllListeners();
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
						DateTime.Now,
						jsonData["currentAuction"]["productName"],
						jsonData["currentAuction"]["openBid"].AsInt,
						jsonData["currentAuction"]["nextIncrement"].AsInt,
						jsonData["currentAuction"]["maxPrice"].AsInt,
						jsonData["currentAuction"]["enterPrice"].AsInt,
						"",
						0,
						"",
						0,
						false
					);
					data.actionButton.onClick.RemoveAllListeners();
					data.actionButton.onClick.AddListener(()=>{ClickJoin(jsonData["currentAuction"]["auctionId"].AsInt,(jsonData["currentAuction"]["enterPrice"].AsInt > 0));});
					totalData++;
				}
				for (int i=0;i<jsonData["futureAuctions"].Count;i++) {
					data = rooms[totalData];
					string[] imgUrl = new string[3];
					imgUrl[0] = jsonData["futureAuctions"][i]["productImages"][0];
					imgUrl[1] = jsonData["futureAuctions"][i]["productImages"][1];
					imgUrl[2] = jsonData["futureAuctions"][i]["productImages"][2];
					data.InitData(
						jsonData["futureAuctions"][i]["auctionId"].AsInt,
						AuctionState.FUTURE,
						imgUrl,
						Utilities.StringLongToDateTime(jsonData["futureAuctions"][i]["dateOpen"]),
						jsonData["futureAuctions"][i]["productName"],
						jsonData["futureAuctions"][i]["openBid"].AsInt,
						jsonData["futureAuctions"][i]["nextIncrement"].AsInt,
						jsonData["futureAuctions"][i]["maxPrice"].AsInt,
						jsonData["futureAuctions"][i]["enterPrice"].AsInt,
						"",
						0,
						"",
						0,
						false
					);
					data.actionButton.onClick.RemoveAllListeners();
					data.actionButton.onClick.AddListener(()=>{ClickJoin(jsonData["futureAuctions"][i]["auctionId"].AsInt,(jsonData["futureAuctions"][i]["enterPrice"].AsInt > 0));});
					totalData++;
				}

				if (end > maxData) {
					maxData = (end > (maxData + (jsonData["futureAuctions"].Count-1)) ) ? end : (maxData + (jsonData["futureAuctions"].Count-1)) ;
				} else {
					maxData = end;
				}
//				Debug.Log("TotalData: "+totalData);
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
//			Debug.Log ("masukSiniiii");
			data.OnRoomShow ();
		}
	}

	public void ClickJoin (int dataAuctionId, bool payment){
		auctionIndex = 0;
		auctionRoomManager.auctionId = dataAuctionId;
		if (payment) {
			connectingPanel.Connecting (true);
			DBManager.API.AuctionJoin (dataAuctionId,
				(response)=>{
					JSONNode jsonData = JSON.Parse(response);
					header.GetUserStars();
					connectingPanel.Connecting (false);
					NextPage ("AUCTIONROOM");
				},
				(error)=>{
					connectingPanel.Connecting (false);
				}
			);
		} else {
			NextPage ("AUCTIONROOM");
		}
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
