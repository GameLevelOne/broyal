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
	bool closed;

	public void InitTile(Sprite pic, Sprite backPic){
		tileBack = backPic;
        tileImage.sprite = tileBack;
		tileAnswer = pic;
		tileAnimate = false;
		closed = true;
	}
	public void ClickTile() {
		if (!tileAnimate) {
            FlipTile();
		}
	}

	public void FlipTile() {
		tileAnim.SetTrigger ("Flip");
		tileAnimate = true;
        if (tileImage.sprite == tileBack)
        {
            SoundManager.Instance.PlaySFX(SFXList.CardClose);
        }
        else
        {
            SoundManager.Instance.PlaySFX(SFXList.CardOpen);
        }
    }

	void ChangeImage() {
		if (closed) {
			tileImage.sprite = tileAnswer;
			closed = false;
		} else {
			tileImage.sprite = tileBack;
			closed = true;
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
