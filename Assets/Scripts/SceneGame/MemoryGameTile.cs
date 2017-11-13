using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameTile : MonoBehaviour {

	public delegate void MemoryGameEvent(MemoryGameTile tile);
	public event MemoryGameEvent OnFinishFlip;

	public Image tileImage;
	public Animator tileAnim;
	Sprite tileAnswer;
	Sprite tileBack;
	bool tileAnimate;

	public void InitTile(Sprite pic){
		tileBack = tileImage.sprite;
		tileAnswer = pic;
		tileAnimate = false;
	}
	public void ClickTile() {
		if (!tileAnimate) {
			SoundManager.Instance.PlaySFX(SFXList.Button01);
			FlipTile ();
		}
	}

	public void FlipTile() {
		tileAnim.SetTrigger ("Flip");
		tileAnimate = true;
	}

	void ChangeImage() {
		if (tileImage.sprite == tileBack) {
			tileImage.sprite = tileAnswer;
		} else {
			tileImage.sprite = tileBack;
		}
	}

	void EndTileFlip() {
		tileAnim.ResetTrigger ("Flip");
		if (tileImage.sprite != tileBack) {
			if (OnFinishFlip != null)
				OnFinishFlip (this);
		} else {
			tileAnimate = false;
		}
	}


}
