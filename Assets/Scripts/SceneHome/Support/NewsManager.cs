using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using BidRoyale.Core;

public class NewsManager : BasePage {
	public ConnectingPanel connectingPanel;
	public NotificationPopUp notifPopUp;
	public Transform newsParent;
	public GameObject newsItemPrefab;
	public GameObject scrollNews;
	public GameObject loadingNews;

	protected override void Init ()
	{
		base.Init ();
		scrollNews.SetActive (false);
		loadingNews.SetActive (true);
		Utilities.ClearChildren (newsParent);
		DBManager.API.GetNewsList (
			(response) => {
				scrollNews.SetActive (true);
				loadingNews.SetActive (false);
				JSONNode jsonData = JSON.Parse (response);
				for (int i=0;i<jsonData["news"].Count;i++) {
					NewsItem ni = Instantiate(newsItemPrefab,newsParent).GetComponent<NewsItem>();
					ni.InitNews(jsonData["news"][i]["newsImageUrl"],jsonData["news"][i]["description"],jsonData["news"][i]["dateCreated"]);
					int newsId = jsonData["news"][i]["id"].AsInt;
					string newsUrl = jsonData["news"][i]["url"];
					ni.readButton.onClick.AddListener(()=>{OpenNews(newsId,newsUrl);});
				}
				newsParent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

			}, 
			(error) => {
				loadingNews.SetActive (false);
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

	public void OpenNews(int newsId, string url) {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		connectingPanel.Connecting (true);
		DBManager.API.ReadNews (newsId,
			(response) => {
				connectingPanel.Connecting (false);
				Application.OpenURL (url);
			}, 
			(error) => {
				connectingPanel.Connecting (false);
//				JSONNode jsonData = JSON.Parse (error);
//				if (jsonData!=null) {
//					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
//				} else {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
//				}
			}
		);

	}


}
