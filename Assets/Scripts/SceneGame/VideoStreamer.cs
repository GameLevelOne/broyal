using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoStreamer : MonoBehaviour {
    public delegate void VideoStreamerEvent();
    public event VideoStreamerEvent OnVideoReady;
	public delegate void VideoStreamerEventBool(bool correct);
    public event VideoStreamerEventBool OnVideoFinished;

    public RawImage rawImage;
    public Text timeLabel;

    VideoPlayer videoPlayer;
    AudioSource audioSource;

    public void ReadyVideo(string url, float waitTime = 1f)
    {
        rawImage.enabled = false;
        gameObject.SetActive(true);
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;

        Application.runInBackground = true;
        Debug.Log("Play Video url=" + url);
        StartCoroutine(PlayVideo(url,waitTime));
    }

    IEnumerator PlayVideo(string url, float waitTime=1f)
    {
        audioSource.volume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.aspectRatio = VideoAspectRatio.FitVertically;
		yield return null;
        videoPlayer.Prepare();

        Debug.Log("Prepare Video");
        float timeElapsed = Time.timeSinceLevelLoad;
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(waitTime);
            break;
//            yield return null;
        }
        timeElapsed = Time.timeSinceLevelLoad - timeElapsed;
        Debug.Log("Finish Preparing in " + timeElapsed.ToString("00.000") + "s");
        rawImage.texture = videoPlayer.texture;
        rawImage.enabled = true;
        timeLabel.enabled = true;
        timeLabel.text = "";
        SoundManager.Instance.SetVolume(0f);

        if (OnVideoReady != null)
            OnVideoReady();
        videoPlayer.Play();
        audioSource.Play();
        Debug.Log("Playing video...");

//        while ((videoPlayer.isPlaying) && ((long)videoPlayer.frameCount > videoPlayer.frame))
		while ((long)videoPlayer.frameCount > videoPlayer.frame )
        //while (videoPlayer.isPlaying)
        {
            float timeLeft = (float)((long)videoPlayer.frameCount - videoPlayer.frame) / videoPlayer.frameRate;
            timeLabel.text = timeLeft.ToString("00");
            Debug.Log("Frame: " + videoPlayer.frame + "/" + videoPlayer.frameCount);
            yield return new WaitForSeconds(1f);
        }
		yield return null;
		Debug.Log("Done Playing with total frame: "+(long)videoPlayer.frameCount);
        if (OnVideoFinished != null)
			OnVideoFinished(((long)videoPlayer.frameCount > 0));

        SoundManager.Instance.SetVolume(PlayerPrefs.GetFloat("SoundVolume", 1f));
		gameObject.SetActive (false);
    }
}
