using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerItem : MonoBehaviour {

	public Text winnerName;
	public Text gameType;
	public string[] gameText;

	public ImageLoader profilePic;
	public Text winDate;
	public Text participants;

	public ImageLoader productPic;
	public Text productName;
	public Text winPrice;

	public void InitWinner(string wName, int gType, string profileUrl, string date, int pNumber, string productUrl, string pName, int wPrice) {
		winnerName.text = LocalizationService.Instance.GetTextByKey("Winner.WINNER") +  wName;
		gameType.text = LocalizationService.Instance.GetTextByKey ("Winner.GAME_TYPE") + (gType!=5 ? LocalizationService.Instance.GetTextByKey (gameText[gType]) : "-");
		profilePic.LoadImageFromUrl (profileUrl);
		winDate.text = date;
		participants.text = LocalizationService.Instance.GetTextByKey ("Winner.PARTICIPANTS") + pNumber.ToString ("N0");
		productPic.LoadImageFromUrl (productUrl);
		productName.text = LocalizationService.Instance.GetTextByKey ("Winner.ITEM") + pName;
		winPrice.text = LocalizationService.Instance.GetTextByKey ("Winner.WIN_PRICE") + wPrice.ToString ("IDR #,0;IDR -#,0;-");
	}
}
