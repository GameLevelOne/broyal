using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPairingTile : MonoBehaviour {
	Color[] tileColors = new Color[2]{new Color(0.91f,0.29f,0.29f,1),new Color(0.07f,0.71f,0.83f,1)};
	public delegate void ColorPairingEvent();
	public event ColorPairingEvent OnFinishFlip;

	public Image tileImage;
	public Animator tileAnim;
	bool tileAnimate;
    GameObject tileOverlay;

	public void InitTile(int colIndex,GameObject overlay){
		tileImage.color = tileColors [colIndex];
		tileAnimate = false;
        tileOverlay = overlay;
	}
	public void ClickTile() {
        tileOverlay.SetActive(true);
		if (!tileAnimate) {
			SoundManager.Instance.PlaySFX(SFXList.CardOpen);
			FlipTile ();
		}
	}

	public void FlipTile() {
		tileAnim.SetTrigger ("Flip");
		tileAnimate = true;
	}

	void ChangeImage() {
		if (tileImage.color == tileColors [0]) {
			tileImage.color = tileColors [1];
		} else {
			tileImage.color = tileColors [0];
		}
	}

	void EndTileFlip() {
        tileOverlay.SetActive(false);
		tileAnim.ResetTrigger ("Flip");
		if (OnFinishFlip != null)
			OnFinishFlip ();
		tileAnimate = false;
	}
}
