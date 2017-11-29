using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class NewsItem : MonoBehaviour {
	public ImageLoader newsImage;
	public Text newsTitle;
	public Text newsDate;
	public Button readButton;

	public void InitNews(string imageUrl, string title, string date) {
		newsImage.LoadImageFromUrl (imageUrl);
		newsTitle.text = title;
		newsDate.text = date;
	}

}
