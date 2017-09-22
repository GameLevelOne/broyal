using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum ImageLoaderState {
	NOIMAGE,
	LOADING,
	LOADED
}

public class ImageLoader : MonoBehaviour {

	Animator loadAnim;
	Image loadImage;
	public Sprite errorIcon;
	public Sprite loadingIcon;

	void Awake () {
		loadAnim = GetComponent<Animator> ();
		loadImage = GetComponent<Image> ();
	}

	public void LoadImageFromUrl(string url)
	{
		SetLoading ();
		StartCoroutine(LoadFromWWW(url));
	}

	public void SetError()
	{
		loadAnim.SetInteger ("AnimState",(int)ImageLoaderState.NOIMAGE);
		loadImage.sprite = errorIcon;
	}
	public void SetLoading()
	{
		loadAnim.SetInteger ("AnimState",(int)ImageLoaderState.LOADING);
		loadImage.sprite = loadingIcon;
	}

	IEnumerator LoadFromWWW(string url)
	{
//		Debug.Log ("Load URL: "+url);
		yield return null;
		WWW www = new WWW (url);
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
//			Debug.Log ("Success");
			loadAnim.SetInteger ("AnimState",(int)ImageLoaderState.LOADED);
			loadImage.sprite = Sprite.Create (www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
		} else {
			SetError ();
		};
	}

}
