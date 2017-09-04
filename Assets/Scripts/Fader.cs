using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {

	public delegate void FadeInFinished();
	public delegate void FadeOutFinished();
	public static event FadeInFinished OnFadeInFinished;
	public static event FadeOutFinished OnFadeOutFinished;

	Image faderImage;
	float fadeSpeed = 1f;
	float fadeTimer = 0f;

	void Awake(){
		faderImage = GetComponent<Image>();
		faderImage.gameObject.SetActive(true);
	}

	void Start(){
		FadeIn();
	}

	public void FadeIn(){
		faderImage.gameObject.SetActive(true);
		StartCoroutine(DoFade(true));
	}

	public void FadeOut(){
		faderImage.gameObject.SetActive(true);
		StartCoroutine(DoFade(false));
	}

	IEnumerator DoFade(bool fadeIn){
		while(fadeTimer < 1f){
			if(fadeIn){
				faderImage.color = Color.Lerp(Color.black,Color.clear,fadeTimer);
			} else{
				faderImage.color = Color.Lerp(Color.clear,Color.black,fadeTimer);
			}
			fadeTimer += Time.deltaTime * fadeSpeed;
			yield return null;
		}

		fadeTimer = 0f;

		if(fadeIn){
			if(OnFadeInFinished !=null){
				OnFadeInFinished();
			} 
			gameObject.SetActive(false);
		} else{
			if(OnFadeOutFinished !=null){
				OnFadeOutFinished();
			}
		}
	}
}
