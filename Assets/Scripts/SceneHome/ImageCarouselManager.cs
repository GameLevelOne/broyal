using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCarouselManager : MonoBehaviour {
	public Sprite[] carouselIcons = new Sprite[2]; //off,on
	public Image[] carouselImages = new Image[3];
	public RectTransform carousel;

	bool enableClick = true;
	Vector3[] carouselPos = new Vector3[3];

	void Start (){
		carouselPos[0] = new Vector3(0,0,0);
		carouselPos[1] = new Vector3(-400f,0,0);
		carouselPos[2] = new Vector3(-800f,0,0);
	}  
	
	public void OnClickCarousel (int activeIndex)
	{
		if (enableClick) {
			enableClick = false;
			for (int i = 0; i < 3; i++) {
				if (i == (activeIndex-1)) {
					carouselImages [i].sprite = carouselIcons [1];
				} else {
					carouselImages [i].sprite = carouselIcons [0];
				}
			}
			StartCoroutine(SlideImage (activeIndex));
		}
	}

	IEnumerator SlideImage (int index)
	{
		Vector3 currPos = carousel.anchoredPosition;
		Vector3 targetPos = carouselPos[index-1];
		float elapsedTime = 0;
		float time = 1f;

		Debug.Log(currPos);
		Debug.Log(targetPos);

		while (elapsedTime < time) {
			carousel.anchoredPosition = Vector3.LerpUnclamped(currPos,targetPos,(elapsedTime/time));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		enableClick=true;
	}
}
