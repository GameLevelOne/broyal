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
	public AuctionItemAreaManager auctionItemAreaManager;
	public GameObject panelAuctionScreen;
	public GameObject panelProductDetail;
	public GameObject panelClaimConfirmation;
	public Transform buttonJoin;
	public AuctionMode auctionMode;
	public int auctionIndex;
	public Image auctionLogo;
	public Sprite[] spriteLogo;

	protected override void Init ()
	{
		base.Init ();
		auctionLogo.sprite = spriteLogo [(int)auctionMode];
	}
		
	public void OnClickEnter(){
		fader.FadeOut();
	}

	public void ClickJoin (){
		auctionIndex = 0;
		NextPage ("AUCTIONROOM");
	}

	public void ClickClaim(){
		panelClaimConfirmation.SetActive(true);
	}

	public void ClickProductDetail(){
		panelProductDetail.SetActive(true);
	}

}
