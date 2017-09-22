using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class TestVideoPlayer : MonoBehaviour {
	public GameObject panel;

	RawImage image;
	VideoPlayer video;
	VideoClip videoClip;
	AudioSource audio;

	bool isDone = false;
	int videoLength = 0;

	void Start(){
		panel.SetActive(true);
		image = GetComponent<RawImage>();
		video = GetComponent<VideoPlayer>();
		audio = GetComponent<AudioSource>();
		videoClip = video.clip;
		videoLength = Mathf.FloorToInt((float)videoClip.length);
		Debug.Log("video length:"+videoLength);
		video.loopPointReached += Video_loopPointReached;
		StartCoroutine(PrepareVideo());
	}

	void OnDisable(){
		video.loopPointReached -= Video_loopPointReached;
	}

	void Video_loopPointReached (VideoPlayer source)
	{
		Debug.Log("Done playing video");
		video.Stop();
		panel.SetActive(true);
	}

	IEnumerator PrepareVideo ()
	{
		video.Prepare ();

		while (!video.isPrepared) {
			Debug.Log ("Preparing video");
			yield return null;
		}
		Debug.Log ("Done preparing video");
		panel.SetActive (false);
		image.texture = video.texture;

		video.Play ();
		audio.Play ();

		video.isLooping = true;
	}
}
