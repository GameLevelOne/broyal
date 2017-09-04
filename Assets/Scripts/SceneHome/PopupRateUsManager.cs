using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupRateUsManager : MonoBehaviour{
	public Sprite[] starSprite = new Sprite[2];
	public Image[] starObj = new Image[5];

	int stars;
	bool isClicked =false;

	public void OnClickCancelRate(){
		this.gameObject.SetActive(false);
	}

	public void OnClickRate (){
		//TODO: fill with actual function
		this.gameObject.SetActive(false);
	}

	public void OnEnterStar (int idx)
	{
		if (isClicked) {
			PaintStars(stars,false);
			isClicked=false;
		}

		stars = idx;
		PaintStars(stars,true);
	}

	public void OnExitStar ()
	{
		if (!isClicked) {
			PaintStars (stars, false);
			stars = 0;
		}
	}

	public void OnClickStar(int idx){
		stars = idx;
		isClicked=true;
	}

	void PaintStars (int idx, bool onEnter)
	{
		for (int i = 0; i < idx; i++) {
			if (onEnter) {
				starObj [i].sprite = starSprite[1];
			} else {
				starObj [i].sprite = starSprite[0];
			}
		}
	}
}
