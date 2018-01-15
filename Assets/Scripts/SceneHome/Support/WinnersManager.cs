using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BidRoyale.Core;
using SimpleJSON;

public class WinnersManager : BasePage {
	public GameObject highlightRumble;
	public GameObject highlightRoyale;
	public NotificationPopUp notifPopUp;
	int auctionMode=2;

	public Transform winnerParent;
	public GameObject winnerItemPrefab;
	public GameObject scrollList;
	public GameObject loadingList;

	protected override void Init ()
	{
		base.Init ();
		LoadWinnerList (auctionMode);
	}

	public void ChangeAuction (int type)
	{
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		LoadWinnerList (type);
	}
	void LoadWinnerList(int type) {
		auctionMode = type;
		if (auctionMode == 1) {
			highlightRumble.SetActive (false);
			highlightRoyale.SetActive (true);
		} else {
			highlightRumble.SetActive (true);
			highlightRoyale.SetActive (false);
		}
		scrollList.SetActive (false);
		loadingList.SetActive (true);
		Utilities.ClearChildren (winnerParent);
		DBManager.API.GetWinnerList (type,
			(response) => {
				scrollList.SetActive (true);
				loadingList.SetActive (false);
				JSONNode jsonData = JSON.Parse (response);
				for (int i=0;i<jsonData["auctionWinnerList"].Count;i++) {
					WinnerItem wi = Instantiate(winnerItemPrefab,winnerParent).GetComponent<WinnerItem>();
					wi.InitWinner(
						jsonData["auctionWinnerList"][i]["winnerUserName"],
						jsonData["auctionWinnerList"][i]["gameType"]==null ? 5 : jsonData["auctionWinnerList"][i]["gameType"].AsInt,
						jsonData["auctionWinnerList"][i]["winnerImageUrl"],
						jsonData["auctionWinnerList"][i]["dateEnd"],
						jsonData["auctionWinnerList"][i]["participantsNumber"].AsInt,
						jsonData["auctionWinnerList"][i]["productImageUrl"],
						jsonData["auctionWinnerList"][i]["productName"],
						jsonData["auctionWinnerList"][i]["winningPrice"].AsInt
					);
				}
				winnerParent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

			}, 
			(error) => {
				loadingList.SetActive (false);
                JSONNode jsonData = JSON.Parse(error);
                if (jsonData != null)
                {
                    notifPopUp.ShowPopUp(LocalizationService.Instance.GetTextByKey("Error." + jsonData["errors"]));
                }
                else
                {
                    notifPopUp.ShowPopUp(LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
                }
            }
		);
	}
}
