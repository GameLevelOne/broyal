using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoAdsManager : MonoBehaviour {
	public LoadingProgress loadingPanel;
	public ConnectingPanel connectingPanel;
    public VideoStreamer videoStreamer;
    public float waitTime;

    void OnEnable()
    {
		connectingPanel.Connecting (true);
//        DBManager.API.GetVideoAds(
//            (response) =>
//            {
//                JSONNode jsonData = JSON.Parse(response);
//                string url = jsonData["videoURL"];
//                videoStreamer.ReadyVideo(url,waitTime);
//                videoStreamer.OnVideoReady += VideoReady;
//                videoStreamer.OnVideoFinished += VideoFinished;
//            },
//            (error) =>
//            {
//                notifPopUp.ShowPopUp(LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
//                notifPopUp.OnFinishOutro += AfterError;
//            }
//        );
		string url = "http://54.169.68.12:8080/bidroyalweb/advertisement/getAdvertisementImageOrVideoUrl?fileName=e8d56a25-aac2-4938-95f4-9cd70e1cbf38.mp4";
		videoStreamer.ReadyVideo(url,waitTime);
		videoStreamer.OnVideoReady += VideoReady;
		videoStreamer.OnVideoFinished += VideoFinished;
    }

    void VideoReady()
    {
        videoStreamer.OnVideoReady -= VideoReady;
		connectingPanel.Connecting (false);
    }

    void VideoFinished()
    {
//		Debug.Log ("VideoFinished - Time to Go home");
        videoStreamer.OnVideoFinished -= VideoFinished;
		loadingPanel.gameObject.SetActive (true);
    }

}
