using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AuctionMode
{
	BIDROYALE,
	BIDRUMBLE
}

public class AuctionLobbyManager : BasePage {
	public Fader fader;
	public ConnectingPanel connectingPanel;

	public AuctionMode auctionMode;
	public int auctionIndex;
	public Image auctionLogo;
	public Sprite[] spriteLogo;

	public ScrollSnapRect scrollSnap;
	public Transform container;
	public Transform unused;
	public AuctionRoomData[] rooms;

	protected override void Init ()
	{
		base.Init ();
		auctionLogo.sprite = spriteLogo [(int)auctionMode];
	}
		
	public void ClickJoin (){
		auctionIndex = 0;
		NextPage ("AUCTIONROOM");
	}

	public void ClickClaim(){
//		panelClaimConfirmation.SetActive(true);
	}

	public void ClickProductDetail(){
//		panelProductDetail.SetActive(true);
	}

}
