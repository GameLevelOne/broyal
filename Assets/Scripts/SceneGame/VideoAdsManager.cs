using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoAdsManager : PagesIntroOutro {
    public GameObject loadingPanel;
    public VideoStreamer videoStreamer;
    public NotificationPopUp notifPopUp;

    new protected void OnEnable()
    {
        base.OnEnable();

        DBManager.API.GetVideoAds(
            (response) =>
            {
                JSONNode jsonData = JSON.Parse(response);
                string url = jsonData["videoURL"];
                videoStreamer.ReadyVideo(url);
                videoStreamer.OnVideoReady += VideoReady;
                videoStreamer.OnVideoFinished += VideoFinished;
            },
            (error) =>
            {
                notifPopUp.ShowPopUp(LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
                notifPopUp.OnFinishOutro += AfterError;
            }
        );
    }

    void VideoReady()
    {
        videoStreamer.OnVideoReady -= VideoReady;
        loadingPanel.SetActive(false);
    }

    void VideoFinished()
    {
        videoStreamer.OnVideoFinished -= VideoFinished;
        Activate(false);
    }

    void AfterError()
    {
        notifPopUp.OnFinishOutro -= AfterError; 
        Activate(false);
    }
}
