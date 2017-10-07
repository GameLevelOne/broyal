using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemStars : MonoBehaviour {
	public Text priceLabel;
	public ImageLoader itemImage;
	public Text starsValue;
	public int stars;
	public Button buyButton;

	public void InitData(int price, string imageUrl, int _stars) {
		priceLabel.text = price.ToString ("IDR #,0;IDR -#,0;-");
		itemImage.LoadImageFromUrl (imageUrl);
		stars = _stars;
		starsValue.text = stars.ToString ("N0");
	}

}
