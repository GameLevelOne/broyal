using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BidRoyale.Core;
using System.IO;
using System;

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
	string loadedURL;
	Sprite currentSprite;

	void Awake () {
		loadAnim = GetComponent<Animator> ();
		loadImage = GetComponent<Image> ();
		loadedURL = "";

		string dirPath = Application.dataPath + "/ImageCache/";
		bool dirExist = Directory.Exists (dirPath);
		if (!dirExist) {
			Directory.CreateDirectory (dirPath);
		}
	}

	public void LoadImageFromUrl(string url)
	{
		if (loadedURL != url) {
			SetLoading ();
			string filePath = Application.dataPath;
			filePath += "/ImageCache/" + Utilities.GetInt64HashCode (url);

			bool useCached = File.Exists (filePath);
			string wwwFilePath;

			if (useCached) {
				DateTime written = File.GetLastWriteTimeUtc (filePath);
				DateTime now = DateTime.UtcNow;
				double totalHours = now.Subtract (written).TotalHours;
				if (totalHours > 24 * 7) {
					File.Delete (filePath);
					useCached = false;
				}
			} 

			if (useCached) {
				wwwFilePath = "file://" + filePath;
			} else {
				wwwFilePath = url;
			}       
                
			Debug.Log ("Image LoadFrom: " + wwwFilePath);
			StartCoroutine (LoadFromWWW (wwwFilePath, useCached));
			loadedURL = url;
		} else {
			loadAnim.SetInteger ("AnimState",(int)ImageLoaderState.LOADED);
			loadImage.sprite = currentSprite;
		}
	}                     
                          
	public void SetError()
	{                     
		loadAnim.SetInteger ("AnimState",(int)ImageLoaderState.NOIMAGE);
		loadImage.sprite = errorIcon;
	}                      
	public void SetLoading ()
	{                      
		loadAnim.SetInteger ("AnimState",(int)ImageLoaderState.LOADING);
		loadImage.sprite = loadingIcon;
	}

	IEnumerator LoadFromWWW(string url, bool useCached)
	{
//		Debug.Log ("Load URL: "+url);
		yield return null;
		WWW www = new WWW (url);
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
//			Debug.Log ("Success");
			loadAnim.SetInteger ("AnimState",(int)ImageLoaderState.LOADED);
			currentSprite = Sprite.Create (www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
			loadImage.sprite = currentSprite;
			if (!useCached) {
				string filePath = Application.dataPath;
				filePath += "/ImageCache/" + Utilities.GetInt64HashCode (url);
				File.WriteAllBytes (filePath, www.bytes);
				Debug.Log ("Saving done to: "+filePath);
			}
		} else {
			SetError ();
		};
	}

}
